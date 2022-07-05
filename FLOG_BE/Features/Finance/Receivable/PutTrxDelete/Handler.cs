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

namespace FLOG_BE.Features.Finance.Receivable.PutTrxDelete
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
            var response = new Response()
            {
                ReceiveTransactionId = request.Body.ReceiveTransactionId
            };
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var Receivable = await _context.ReceivableTransactionHeaders.FirstOrDefaultAsync(x => x.ReceiveTransactionId == request.Body.ReceiveTransactionId);
                    if (Receivable != null)
                    {
                        if (Receivable.Status == DOCSTATUS.NEW)
                        {
                            //DELETE DISTRIBUTIONS
                            var resp = await _financeManager.DeleteDistributionJournalAsync(Receivable.DocumentNo, FLOG.Core.TRX_MODULE.TRX_RECEIVABLE);

                            if (resp.Valid)
                            {
                                _context.ReceivableTransactionTaxes.Where(x => x.ReceiveTransactionId == request.Body.ReceiveTransactionId).ToList().ForEach(p => _context.Remove(p));
                                _context.ReceivableTransactionDetails.Where(x => x.ReceiveTransactionId == request.Body.ReceiveTransactionId).ToList().ForEach(p => _context.Remove(p));
                                _context.ReceivableTransactionHeaders.Where(x => x.ReceiveTransactionId == request.Body.ReceiveTransactionId).ToList().ForEach(p => _context.Remove(p));

                                //UPDATE LAST NO
                                _docGenerator.DocNoDelete(Receivable.DocumentNo, transaction.GetDbTransaction());

                                await _context.SaveChangesAsync();

                                transaction.Commit();
                            }
                            else
                            {
                                transaction.Rollback();

                                return ApiResult<Response>.ValidationError(!string.IsNullOrEmpty(resp.ErrorMessage) ? resp.ErrorMessage : "Record can not be deleted.");
                            }                            
                        }
                        else {
                            return ApiResult<Response>.ValidationError("Only New record can be deleted.");
                        }
                    }
                    else{
                        transaction.Rollback();
                        return ApiResult<Response>.ValidationError("Receivable Transaction not found.");
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
