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

namespace FLOG_BE.Features.Finance.APApply.PutStatusApplyPayable
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;
        private IFinanceManager _financeManager;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
            _financeManager = new FinanceManager(_context);

        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var entryHeader = await _context.APApplyHeaders.FirstOrDefaultAsync(x => x.PayableApplyId == request.Body.PayableApplyId);

                    string docNo = "";
                    if(entryHeader != null)
                    {
                        docNo = entryHeader.DocumentNo;

                        if (request.Body.ActionDocStatus == DOCSTATUS.DELETE)
                        {
                            if(entryHeader.Status == DOCSTATUS.NEW)
                            {
                                entryHeader.Status = DOCSTATUS.DELETE;
                                entryHeader.ModifiedBy = request.Initiator.UserId;
                                entryHeader.ModifiedDate = DateTime.Now;

                                _context.APApplyHeaders.Update(entryHeader);

                                await _context.SaveChangesAsync();

                                transaction.Commit();
                            }
                            else
                            {
                                transaction.Rollback();

                                return ApiResult<Response>.ValidationError("Record can not be deleted.");
                            }
                        }else if(request.Body.ActionDocStatus == DOCSTATUS.POST)
                        {
                            if (entryHeader.Status == DOCSTATUS.NEW)
                            {
                                entryHeader.Status = DOCSTATUS.POST;
                                entryHeader.ModifiedBy = request.Initiator.UserId;
                                entryHeader.ModifiedDate = DateTime.Now;

                                _context.APApplyHeaders.Update(entryHeader);

                                //DO SOME POSTING HERE
                                var validJournal = await _financeManager.PostJournalAsync(entryHeader.DocumentNo, TRX_MODULE.TRX_APPLY_PAYABLE, request.Initiator.UserId, entryHeader.TransactionDate);

                                if (validJournal.Valid)
                                {
                                    await _context.SaveChangesAsync();

                                    transaction.Commit();
                                }
                                else
                                {
                                    transaction.Rollback();

                                    return ApiResult<Response>.ValidationError(!string.IsNullOrEmpty(validJournal.ErrorMessage) ? validJournal.ErrorMessage : "Post Journals can not be created.");
                                }                                
                            }
                            else
                            {
                                transaction.Rollback();

                                return ApiResult<Response>.ValidationError("Record can not be posted.");
                            }
                        }
                        else if(request.Body.ActionDocStatus == DOCSTATUS.VOID)
                        {
                            if (entryHeader.Status == DOCSTATUS.POST)
                            {
                                entryHeader.Status = DOCSTATUS.VOID;
                                entryHeader.VoidBy = request.Initiator.UserId;
                                entryHeader.VoidDate = DateTime.Now;
                                entryHeader.StatusComment = request.Body.Comments;

                                _context.APApplyHeaders.Update(entryHeader);

                                //DO SOME POSTING TO REVERSE JOURNAL WITH SAME DOCUMENT
                                var resp = await _financeManager.VoidJournalAsync(entryHeader.DocumentNo, TRX_MODULE.TRX_APPLY_PAYABLE, request.Initiator.UserId, request.Body.ActionDate);

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
                        PayableApplyId = request.Body.PayableApplyId,
                        Message = string.Format("{0} status successfully updated to {1}", docNo, DOCSTATUS.Caption(request.Body.ActionDocStatus)) 
                    };

                    return ApiResult<Response>.Ok(response);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    return ApiResult<Response>.InternalServerError("[PutStatusApplyPayment] " + ex.Message);
                }
            }
        }
    }
}
