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
using FLOG.Core.DocumentNo;
using FLOG_BE.Model.Companies.Entities;
using FLOG.Core.Finance.Util;
using Infrastructure;

namespace FLOG_BE.Features.Finance.JournalEntry.PostJournalEntry
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;
        private readonly IDocumentGenerator _docGenerator;
        private IFinanceManager _financeManager;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
            _docGenerator = new DocumentGenerator(_context);
            _financeManager = new FinanceManager(_context);
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                string documentUniqueNo = _docGenerator.UniqueDocumentNoByTrxType(request.Body.TransactionDate, TRX_MODULE.TRX_GENERAL_JOURNAL, DOCNO_FEATURE.NOTRX_GJE, "", transaction.GetDbTransaction());

                if (!string.IsNullOrEmpty(documentUniqueNo))
                {
                    var journalEntryHeader = new Entities.JournalEntryHeader()
                    {
                        JournalEntryHeaderId = Guid.NewGuid(),
                        DocumentNo = documentUniqueNo,
                        BranchCode = request.Body.BranchCode,
                        TransactionDate = request.Body.TransactionDate,
                        CurrencyCode = request.Body.CurrencyCode,
                        ExchangeRate = request.Body.ExchangeRate,
                        IsMultiply = request.Body.IsMultiply,
                        SourceDocument = request.Body.SourceDocument,
                        Description = request.Body.Description,
                        OriginatingTotal = request.Body.OriginatingTotal,
                        FunctionalTotal = request.Body.FunctionalTotal,
                        Status = DOCSTATUS.NEW,
                        CreatedBy = request.Initiator.UserId,
                        CreatedDate = DateTime.Now
                    };

                    _context.JournalEntryHeaders.Add(journalEntryHeader);

                    int iDetailCount = 0;

                    if (journalEntryHeader != null && journalEntryHeader.JournalEntryHeaderId != Guid.Empty)
                    {
                        var details = await InsertJournalEntryDetails(request.Body, journalEntryHeader.JournalEntryHeaderId);

                        //CREATE DISTRIBUTION JOURNAL HERE
                        var resp = await _financeManager.CreateDistributionJournalAsync(journalEntryHeader, details);
                        iDetailCount = (resp.Valid ? 8 : 0);
                    }

                    if (iDetailCount > 0)
                    {
                        int iSuccess = await _context.SaveChangesAsync();

                        if (iSuccess > 0)
                        {
                            transaction.Commit();
                            return ApiResult<Response>.Ok(new Response()
                            {
                                JournalEntryHeaderId = journalEntryHeader.JournalEntryHeaderId,
                                DocumentNo = journalEntryHeader.DocumentNo,
                                Message = "Journal Entry successfully stored."
                            });
                        }
                        else
                        {
                            transaction.Rollback();
                            return ApiResult<Response>.ValidationError("Journal Entry can not be stored.");
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        return ApiResult<Response>.ValidationError("No entry details found.");
                    }
                }
                else
                {
                    transaction.Rollback();
                    return ApiResult<Response>.ValidationError("Document No can not be created. Please check Document No Setup!");
                }                
            }
        }
        private async Task<List<Entities.JournalEntryDetail>> InsertJournalEntryDetails(RequestEntryHeader body, Guid headerId)
        {
            List<Entities.JournalEntryDetail> entryDetails = new List<Entities.JournalEntryDetail>();

            if (body.RequestEntryDetails != null)
            {
                //INSERT NEW ROWS DETAIL
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
                        RowIndex = item.RowIndex,
                    };
                    entryDetails.Add(journalEntryDetail);                    
                }

                await _context.JournalEntryDetails.AddRangeAsync(entryDetails.ToArray());
                
            }

            return entryDetails;

        }

    }
}
