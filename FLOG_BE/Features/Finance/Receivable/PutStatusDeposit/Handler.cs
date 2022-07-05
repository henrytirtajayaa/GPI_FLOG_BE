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

namespace FLOG_BE.Features.Finance.Receivable.PutStatusDeposit
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

                                if (entryHeader != null)
                                {
                                    entryHeader.Status = DOCSTATUS.POST;
                                    entryHeader.ModifiedBy = request.Initiator.UserId;
                                    entryHeader.ModifiedDate = DateTime.Now;

                                    _context.ReceivableTransactionHeaders.Update(entryHeader);

                                    await _context.SaveChangesAsync();

                                    transaction.Commit();
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
                                entryHeader.Status = DOCSTATUS.VOID;
                                entryHeader.VoidBy = request.Initiator.UserId;
                                entryHeader.VoidDate = request.Body.ActionDate;
                                entryHeader.StatusComment = request.Body.StatusComment;

                                _context.ReceivableTransactionHeaders.Update(entryHeader);

                                await _context.SaveChangesAsync();

                                transaction.Commit();
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

                    return ApiResult<Response>.InternalServerError(ex.Message);
                }
            }


        }

    }
}
