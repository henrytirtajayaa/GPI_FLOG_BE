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
using FLOG.Core.Finance.Util;
using FLOG.Core.DocumentNo;
using Infrastructure;

namespace FLOG_BE.Features.Finance.JournalEntry.DeleteJournalEntry
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;
        private IFinanceManager _financeManager;
        private IDocumentGenerator _docGenerator;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
            _financeManager = new FinanceManager(_context);
            _docGenerator = new DocumentGenerator(_context);
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            var record = _context.JournalEntryHeaders.FirstOrDefault(x => x.JournalEntryHeaderId == request.Body.JournalEntryHeaderId);

            if(record.Status == FLOG.Core.DOCSTATUS.NEW)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    //DELETE DETAILS
                    var mapping = _context.Model.FindEntityType(typeof(JournalEntryDetail)).Relational();
                    int isDeleted = await _context.Database.ExecuteSqlCommandAsync("DELETE FROM " + mapping.TableName  + " WHERE journal_entry_header_id = {0} ", request.Body.JournalEntryHeaderId);

                    //DELETE DISTRIBUTIONS
                    var resp = await _financeManager.DeleteDistributionJournalAsync(record.DocumentNo, FLOG.Core.TRX_MODULE.TRX_GENERAL_JOURNAL);

                    //DELETE HEADER
                    if (resp.Valid)
                    {
                        _context.Attach(record);
                        _context.Remove(record);

                        //UPDATE LAST NO
                        _docGenerator.DocNoDelete(record.DocumentNo, transaction.GetDbTransaction());

                        await _context.SaveChangesAsync();

                        transaction.Commit();

                        return ApiResult<Response>.Ok(new Response()
                        {
                            JournalEntryHeaderId = request.Body.JournalEntryHeaderId,
                            Message = string.Format("# {0} successfully deleted.", record.DocumentNo)
                        });
                    }
                    else
                    {
                        transaction.Rollback();

                        return ApiResult<Response>.ValidationError(!string.IsNullOrEmpty(resp.ErrorMessage) ? resp.ErrorMessage : "Journal Details can not be deleted !");
                    }                    
                }                 

            }
            else
            {
                return ApiResult<Response>.ValidationError("Only NEW Journal Entry can be deleted !");
            }            
        }
    }
}
