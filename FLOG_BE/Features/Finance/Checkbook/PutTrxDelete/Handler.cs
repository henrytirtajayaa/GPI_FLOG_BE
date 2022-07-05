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
using FLOG.Core.DocumentNo;
using Infrastructure;

namespace FLOG_BE.Features.Finance.Checkbook.PutTrxDelete
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;
        private IFinanceManager _financeManager;
        private IDocumentGenerator _documentGenerator;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
            _financeManager = new FinanceManager(_context);
            _documentGenerator = new DocumentGenerator(_context);
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            var response = new Response()
            {
                CheckbookTransactionId = request.Body.CheckbookTransactionId
            };
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var checkbook = await _context.CheckbookTransactionHeaders.FirstOrDefaultAsync(x => x.CheckbookTransactionId == request.Body.CheckbookTransactionId);
                    if (checkbook != null && checkbook.Status == DOCSTATUS.NEW)
                    {
                        //DELETE DISTRIBUTIONS
                        var resp = await _financeManager.DeleteDistributionJournalAsync(checkbook.DocumentNo, FLOG.Core.TRX_MODULE.TRX_CHECKBOOK);

                        if (resp.Valid)
                        {
                            _context.CheckbookTransactionApprovals.Where(x => x.CheckbookTransactionId == request.Body.CheckbookTransactionId).ToList().ForEach(p => _context.Remove(p));
                            _context.CheckbookTransactionDetails.Where(x => x.CheckbookTransactionId == request.Body.CheckbookTransactionId).ToList().ForEach(p => _context.Remove(p));
                            _context.CheckbookTransactionHeaders.Where(x => x.CheckbookTransactionId == request.Body.CheckbookTransactionId).ToList().ForEach(p => _context.Remove(p));

                            _documentGenerator.DocNoDelete(checkbook.DocumentNo, transaction.GetDbTransaction());

                            await _context.SaveChangesAsync();
                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();

                            return ApiResult<Response>.ValidationError(!string.IsNullOrEmpty(resp.ErrorMessage) ? resp.ErrorMessage : "Record can not be deleted.");
                        }                                                                       
                    }
                    else
                    {
                        transaction.Rollback();
                        return ApiResult<Response>.ValidationError("Only Checkbook NEW can be deleted.");
                    }

                    return ApiResult<Response>.Ok(response);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
