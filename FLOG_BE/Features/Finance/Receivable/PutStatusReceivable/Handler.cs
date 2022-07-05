using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model.Companies;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Infrastructure.Utils;
using FLOG_BE.Model.Companies.Entities;
using Entities = FLOG_BE.Model.Companies.Entities;
using FLOG.Core;
using FLOG.Core.Finance.Util;

namespace FLOG_BE.Features.Finance.Receivable.PutStatusReceivable
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private FLOG.Core.Finance.Util.IFinanceManager _financeManager;
        private readonly HATEOASLinkCollection _linkCollection;
        private PutReceivableTransaction.Repository _repository;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
            _financeManager = new FinanceManager(_context);
            _repository = new PutReceivableTransaction.Repository(_context);
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
           
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var entryHeader = await _context.ReceivableTransactionHeaders.FirstOrDefaultAsync(x => x.ReceiveTransactionId == request.Body.ReceiveTransactionId);

                    string docNo = "";
                    if (entryHeader != null)
                    {
                        docNo = entryHeader.DocumentNo;

                        if (request.Body.Status == DOCSTATUS.DELETE)
                        {
                            if (entryHeader.Status == DOCSTATUS.NEW)
                            {
                                entryHeader.Status = DOCSTATUS.DELETE;
                                entryHeader.ModifiedBy = request.Initiator.UserId;
                                entryHeader.ModifiedDate = DateTime.Now;

                                _context.ReceivableTransactionHeaders.Update(entryHeader);

                                await _context.SaveChangesAsync();

                                transaction.Commit();
                            }
                            else
                            {
                                transaction.Rollback();

                                return ApiResult<Response>.ValidationError("Record can not be deleted.");
                            }
                        }
                        else if (request.Body.Status == DOCSTATUS.POST)
                        {
                            if (entryHeader.Status == DOCSTATUS.NEW)
                            {
                                entryHeader = await _repository.UpdateHeader(request.Body, request.Initiator);

                                if(entryHeader != null)
                                {
                                    entryHeader.Status = DOCSTATUS.POST;
                                    entryHeader.ModifiedBy = request.Initiator.UserId;
                                    entryHeader.ModifiedDate = DateTime.Now;

                                    _context.ReceivableTransactionHeaders.Update(entryHeader);

                                    var receivableDetails = await _repository.InsertReceivableDetails(request.Body);

                                    var receivableTaxes = await _repository.InsertReceivableTax(request.Body);

                                    await _context.SaveChangesAsync();

                                    JournalResponse jResponse = await _financeManager.CreateDistributionJournalAsync(entryHeader, receivableDetails, receivableTaxes);

                                    if (jResponse.Valid)
                                    {
                                        await _context.SaveChangesAsync();

                                        //DO SOME POSTING HERE
                                        var resp = await _financeManager.PostJournalAsync(entryHeader.DocumentNo, TRX_MODULE.TRX_RECEIVABLE, request.Initiator.UserId, entryHeader.TransactionDate);

                                        if (resp.Valid)
                                        {
                                            await _context.SaveChangesAsync();

                                            transaction.Commit();
                                        }
                                        else
                                        {
                                            transaction.Rollback();

                                            return ApiResult<Response>.ValidationError(!string.IsNullOrEmpty(resp.ErrorMessage) ? resp.ErrorMessage : "Post Journals can not be created.");
                                        }
                                    }
                                    else
                                    {
                                        transaction.Rollback();

                                        return ApiResult<Response>.ValidationError(!string.IsNullOrEmpty(jResponse.ErrorMessage) ? jResponse.ErrorMessage : "Post Journals can not be created.");
                                    } 
                                }
                                else
                                {
                                    transaction.Rollback();

                                    return ApiResult<Response>.ValidationError("Receivable not exist !");
                                }   
                            }
                            else
                            {
                                transaction.Rollback();

                                return ApiResult<Response>.ValidationError("Record can not be posted.");
                            }
                        }
                        else if (request.Body.Status == DOCSTATUS.VOID)
                        {
                            if (entryHeader.Status == DOCSTATUS.POST)
                            {
                                //UPDATE RELATED DOCUMENT AFFECTED
                                var updated = await this.UpdateReceivableRelated(entryHeader);

                                if (updated.Valid)
                                {
                                    //UPDATE HEADER
                                    entryHeader.Status = DOCSTATUS.VOID;
                                    entryHeader.VoidBy = request.Initiator.UserId;
                                    entryHeader.VoidDate = request.Body.ActionDate;
                                    entryHeader.StatusComment = request.Body.StatusComment;

                                    _context.ReceivableTransactionHeaders.Update(entryHeader);

                                    //DO SOME POSTING TO REVERSE JOURNAL WITH SAME DOCUMENT
                                    var resp = await _financeManager.VoidJournalAsync(entryHeader.DocumentNo, TRX_MODULE.TRX_RECEIVABLE, request.Initiator.UserId, request.Body.ActionDate);

                                    if (resp.Valid)
                                    {
                                        await _context.SaveChangesAsync();

                                        transaction.Commit();
                                    }
                                    else
                                    {
                                        transaction.Rollback();

                                        return ApiResult<Response>.ValidationError(!string.IsNullOrEmpty(resp.ErrorMessage) ? resp.ErrorMessage : "Void Receivable can not be created.");
                                    }
                                }
                                else
                                {
                                    transaction.Rollback();

                                    return ApiResult<Response>.ValidationError("Void Receivable can not be created !");
                                }                               
                            }
                            else
                            {
                                transaction.Rollback();

                                return ApiResult<Response>.ValidationError("Record can not be void.");
                            }
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        return ApiResult<Response>.ValidationError("Document not available.");
                    }

                    var response = new Response()
                    {
                        ReceiveTransactionId = request.Body.ReceiveTransactionId,
                        Message = string.Format("{0} status successfully updated to {1}", docNo, DOCSTATUS.Caption(request.Body.Status))
                    };

                    return ApiResult<Response>.Ok(response);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    return ApiResult<Response>.InternalServerError( ex.Message);
                }
            }
        }
    
        private async Task<JournalResponse> UpdateReceivableRelated(Entities.ReceivableTransactionHeader header)
        {
            JournalResponse response = new JournalResponse { Valid = true, ErrorMessage = "", ValidStatus = 0 };

            //RESET NEGOTIATION SHEET SELLING DETAIL
            var negoSellings = _context.NegotiationSheetSellings.Where(x => x.ReceiveTransactionId == header.ReceiveTransactionId).ToList();

            if (negoSellings != null)
            {
                foreach(var nss in negoSellings)
                {
                    nss.ReceiveTransactionId = Guid.Empty;                    
                }
            }

            return response;
        }
    }
}
