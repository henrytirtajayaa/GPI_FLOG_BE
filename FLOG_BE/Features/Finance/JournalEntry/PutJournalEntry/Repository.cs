using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using FLOG_BE.Model.Companies;
using Entities = FLOG_BE.Model.Companies.Entities;
using Infrastructure;
using System.Collections.Generic;
using FLOG.Core;

namespace FLOG_BE.Features.Finance.JournalEntry.PutJournalEntry
{
    public class Repository
    {
        private readonly CompanyContext _context;

        public Repository(CompanyContext context)
        {
            _context = context;
        }
        
        public async Task<Entities.JournalEntryHeader> UpdateHeader(RequestEntryHeader body, UserLogin user)
        {
            var entryHeader = await _context.JournalEntryHeaders.FirstOrDefaultAsync(x => x.JournalEntryHeaderId == body.JournalEntryHeaderId);

            if (entryHeader != null)
            {
                entryHeader.BranchCode = body.BranchCode;
                entryHeader.TransactionDate = body.TransactionDate;
                entryHeader.CurrencyCode = body.CurrencyCode;
                entryHeader.ExchangeRate = body.ExchangeRate;
                entryHeader.IsMultiply = body.IsMultiply;
                entryHeader.SourceDocument = body.SourceDocument;
                entryHeader.Description = body.Description;
                entryHeader.OriginatingTotal = body.OriginatingTotal;
                entryHeader.FunctionalTotal = body.FunctionalTotal;
                entryHeader.Status = DOCSTATUS.NEW;
                entryHeader.ModifiedBy = user.UserId;
                entryHeader.ModifiedDate = DateTime.Now;

                _context.JournalEntryHeaders.Update(entryHeader);
            }

            return entryHeader;
        }

        public async Task<List<Entities.JournalEntryDetail>> InsertDetails(RequestEntryHeader body)
        {
            List<Entities.JournalEntryDetail> details = new List<Entities.JournalEntryDetail>();

            if (body.RequestEntryDetails != null)
            {
                //REMOVE EXISTING
                _context.JournalEntryDetails
               .Where(x => x.JournalEntryHeaderId == body.JournalEntryHeaderId).ToList().ForEach(p => _context.Remove(p));

                //INSERT NEW ROWS DETAIL
                foreach (var item in body.RequestEntryDetails)
                {
                    var journalEntryDetail = new Entities.JournalEntryDetail()
                    {
                        JournalEntryDetailId = Guid.NewGuid(),
                        JournalEntryHeaderId = body.JournalEntryHeaderId,
                        AccountId = item.AccountId,
                        Description = item.Description,
                        OriginatingDebit = item.OriginatingDebit,
                        OriginatingCredit = item.OriginatingCredit,
                        FunctionalDebit = item.FunctionalDebit,
                        FunctionalCredit = item.FunctionalCredit,
                        Status = DOCSTATUS.NEW,
                        RowIndex = item.RowIndex,
                    };
                    details.Add(journalEntryDetail);
                }

                if (details.Count > 0)
                    await _context.JournalEntryDetails.AddRangeAsync(details);
            }

            return details;
        }

    }
}
