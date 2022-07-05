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
using Entities = FLOG_BE.Model.Companies.Entities;
using FLOG.Core;
using FLOG.Core.Finance.Util;
using FLOG_BE.Features.Finance.Payable.PutPayable;

namespace FLOG_BE.Features.Finance.Payable.PutStatusPayable
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;
        private IFinanceManager _financeManager;
        private Repository _repository;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
            _financeManager = new FinanceManager(_context);
            _repository = new Repository(_context);
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var payableHeader = await _context.PayableTransactionHeaders.FirstOrDefaultAsync(x => x.PayableTransactionId == request.Body.PayableTransactionId);

                    string docNo = "";
                    if (payableHeader != null)
                    {
                        docNo = payableHeader.DocumentNo;

                        if (request.Body.Status == DOCSTATUS.DELETE)
                        {
                            if (payableHeader.Status == DOCSTATUS.NEW)
                            {
                                payableHeader.Status = DOCSTATUS.DELETE;
                                payableHeader.ModifiedBy = request.Initiator.UserId;
                                payableHeader.ModifiedDate = DateTime.Now;

                                _context.PayableTransactionHeaders.Update(payableHeader);

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
                            if (payableHeader.Status == DOCSTATUS.NEW)
                            {
                                payableHeader = await _repository.UpdateHeader(request.Body, request.Initiator);

                                if (payableHeader != null)
                                {
                                    payableHeader.Status = DOCSTATUS.POST;
                                    payableHeader.ModifiedBy = request.Initiator.UserId;
                                    payableHeader.ModifiedDate = DateTime.Now;

                                    _context.PayableTransactionHeaders.Update(payableHeader);

                                    var payableDetails = await _repository.InsertPayableDetails(request.Body);

                                    var payableTaxes = await _repository.InsertPayableTax(request.Body);

                                    JournalResponse jResponse = await _financeManager.CreateDistributionJournalAsync(payableHeader, payableDetails, payableTaxes);

                                    if (jResponse.Valid)
                                    {
                                        await _context.SaveChangesAsync();

                                        //DO SOME POSTING HERE
                                        var resp = await _financeManager.PostJournalAsync(payableHeader.DocumentNo, TRX_MODULE.TRX_PAYABLE, request.Initiator.UserId, payableHeader.TransactionDate);

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
                            if (payableHeader.Status == DOCSTATUS.POST)
                            {
                                var updated = await this.UpdatePayableRelated(payableHeader);

                                if (updated.Valid)
                                {
                                    payableHeader.Status = DOCSTATUS.VOID;
                                    payableHeader.VoidBy = request.Initiator.UserId;
                                    payableHeader.VoidDate = DateTime.Now;
                                    payableHeader.StatusComment = request.Body.StatusComment;

                                    _context.PayableTransactionHeaders.Update(payableHeader);

                                    //DO SOME POSTING TO REVERSE JOURNAL WITH SAME DOCUMENT
                                    var resp = await _financeManager.VoidJournalAsync(payableHeader.DocumentNo, TRX_MODULE.TRX_PAYABLE, request.Initiator.UserId, request.Body.ActionDate);

                                    if (resp.Valid)
                                    {
                                        await _context.SaveChangesAsync();

                                        transaction.Commit();
                                    }
                                    else
                                    {
                                        transaction.Rollback();

                                        return ApiResult<Response>.ValidationError(!string.IsNullOrEmpty(resp.ErrorMessage) ? resp.ErrorMessage : "Void Payable can not be created.");
                                    }
                                }
                                else
                                {
                                    transaction.Rollback();

                                    return ApiResult<Response>.ValidationError("Void Payable can not be created !");
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
                        PayableTransactionId = request.Body.PayableTransactionId,
                        Message = string.Format("{0} status successfully updated to {1}", docNo, DOCSTATUS.Caption(request.Body.Status))
                    };

                    return ApiResult<Response>.Ok(response);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[PutStatusPayable] ************* " + ex.Message);
                    Console.WriteLine("[PutStatusPayable] ************* " + ex.StackTrace);

                    transaction.Rollback();

                    return ApiResult<Response>.InternalServerError("[PutStatusPayable] " + ex.Message);
                }
            }
        }

        private async Task<JournalResponse> UpdatePayableRelated(Entities.PayableTransactionHeader header)
        {
            JournalResponse response = new JournalResponse { Valid = true, ErrorMessage = "", ValidStatus = 0 };

            //RESET NEGOTIATION SHEET SELLING DETAIL
            var negoBuyings = _context.NegotiationSheetBuyings.Where(x => x.PayableTransactionId == header.PayableTransactionId).ToList();

            if (negoBuyings != null)
            {
                foreach (var nss in negoBuyings)
                {
                    nss.PayableTransactionId = Guid.Empty;
                }
            }

            return response;
        }

    }
}
