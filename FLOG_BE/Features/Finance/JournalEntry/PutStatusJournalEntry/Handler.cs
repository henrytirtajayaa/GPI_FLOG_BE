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
using FLOG_BE.Features.Finance.JournalEntry.PutJournalEntry;

namespace FLOG_BE.Features.Finance.JournalEntry.PutStatusJournalEntry
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
                    var entryHeader = await _context.JournalEntryHeaders.FirstOrDefaultAsync(x => x.JournalEntryHeaderId == request.Body.JournalEntryHeaderId);

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

                                _context.JournalEntryHeaders.Update(entryHeader);

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
                                entryHeader = await _repository.UpdateHeader(request.Body, request.Initiator);

                                var entryDetails = await _repository.InsertDetails(request.Body);

                                await _context.SaveChangesAsync();

                                //CREATE DISTRIBUTION JOURNAL HERE
                                var updateDistribution = await _financeManager.CreateDistributionJournalAsync(entryHeader, entryDetails);

                                await _context.SaveChangesAsync();

                                if (updateDistribution.Valid)
                                {
                                    //DO SOME POSTING HERE
                                    var validJournal = await _financeManager.PostJournalAsync(entryHeader.DocumentNo, TRX_MODULE.TRX_GENERAL_JOURNAL, request.Initiator.UserId, entryHeader.TransactionDate);

                                    entryHeader.Status = DOCSTATUS.POST;
                                    entryHeader.ModifiedBy = request.Initiator.UserId;
                                    entryHeader.ModifiedDate = DateTime.Now;

                                    _context.JournalEntryHeaders.Update(entryHeader);

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

                                    return ApiResult<Response>.ValidationError(string.Format("Update record before posted failed ! {0}", updateDistribution.ErrorMessage));
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

                                _context.JournalEntryHeaders.Update(entryHeader);

                                //DO SOME POSTING TO REVERSE JOURNAL WITH SAME DOCUMENT
                                var resp = await _financeManager.VoidJournalAsync(entryHeader.DocumentNo, TRX_MODULE.TRX_GENERAL_JOURNAL, request.Initiator.UserId, request.Body.ActionDate);

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
                        JournalEntryHeaderId = request.Body.JournalEntryHeaderId,
                        Message = string.Format("{0} status successfully updated to {1}", docNo, DOCSTATUS.Caption(request.Body.ActionDocStatus)) 
                    };

                    return ApiResult<Response>.Ok(response);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    return ApiResult<Response>.InternalServerError("[PutStatusJournalEntry] " + ex.Message);
                }
            }
        }
    }
}
