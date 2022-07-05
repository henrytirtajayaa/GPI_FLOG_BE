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

namespace FLOG_BE.Features.Finance.JournalEntry.PutJournalEntry
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
                    var entryHeader = await _repository.UpdateHeader(request.Body, request.Initiator);

                    if (entryHeader != null)
                    {
                        entryHeader.Status = DOCSTATUS.NEW;
                        entryHeader.ModifiedBy = request.Initiator.UserId;
                        entryHeader.ModifiedDate = DateTime.Now;

                        _context.JournalEntryHeaders.Update(entryHeader);

                        var details = await _repository.InsertDetails(request.Body);

                        //CREATE DISTRIBUTION JOURNAL HERE
                        var resp = await _financeManager.CreateDistributionJournalAsync(entryHeader, details);

                        if (resp.Valid)
                        {
                            int iSuccess = await _context.SaveChangesAsync();

                            if (iSuccess > 0)
                            {
                                transaction.Commit();
                                return ApiResult<Response>.Ok(new Response()
                                {
                                    JournalEntryHeaderId = entryHeader.JournalEntryHeaderId,
                                    Message = "Journal Entry successfully updated."
                                });
                            }
                            else
                            {
                                transaction.Rollback();
                                return ApiResult<Response>.ValidationError("Journal Entry can not be updated.");
                            }
                        }
                        else
                        {
                            transaction.Rollback();
                            return ApiResult<Response>.ValidationError(!string.IsNullOrEmpty(resp.ErrorMessage) ? resp.ErrorMessage : "Entry details can not be updated.");
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        return ApiResult<Response>.ValidationError("Journal Entry not found.");
                    }

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return ApiResult<Response>.InternalServerError("[PutJournalEntry] " + ex.Message);
                }

            }
        }

        private async Task<List<Entities.JournalEntryDetail>> InsertJournalEntryDetails(RequestEntryHeader body, Guid headerId)
        {
            List<Entities.JournalEntryDetail> entryDetails = new List<Entities.JournalEntryDetail>();

            var mapping = _context.Model.FindEntityType(typeof(JournalEntryDetail)).Relational();
            
            //REMOVE EXISTING
            int deletedRows = await _context.Database.ExecuteSqlCommandAsync("DELETE FROM " + mapping.TableName + " WHERE journal_entry_header_id = {0} ", headerId);

            //INSERT NEW ROWS DETAIL
            if (body.RequestEntryDetails != null)
            {
                foreach (var item in body.RequestEntryDetails)
                {
                    var journalEntryDetail = new Entities.JournalEntryDetail()
                    {
                        JournalEntryDetailId = Guid.NewGuid(),
                        JournalEntryHeaderId = headerId,
                        AccountId = item.AccountId,
                        Description = item.Description,
                        OriginatingDebit = item.OriginatingDebit,
                        OriginatingCredit = item.OriginatingCredit,
                        FunctionalDebit = item.FunctionalDebit,
                        FunctionalCredit = item.FunctionalCredit,
                        Status = item.Status,
                    };
                    entryDetails.Add(journalEntryDetail);
                }

                await _context.JournalEntryDetails.AddRangeAsync(entryDetails.ToArray());

            }

            return entryDetails;

        }

        private async Task<int> CreateDistributionJournals(RequestEntryHeader body, JournalEntryHeader header)
        {
            int iRows = 0;

            return iRows;
        }
    }
}
