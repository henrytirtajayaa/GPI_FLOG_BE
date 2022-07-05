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

namespace FLOG_BE.Features.Finance.Payable.PutTrxDeletePayable
{
    public class Handler : IAsyncRequestHandler<Request,Response>
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
                PayableTransactionId = request.Body.PayableTransactionId
            };

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var Payable = await _context.PayableTransactionHeaders.FirstOrDefaultAsync(x => x.PayableTransactionId == request.Body.PayableTransactionId);
                    if (Payable != null)
                    {
                        if (Payable.Status == DOCSTATUS.NEW)
                        {
                            //DELETE DISTRIBUTIONS
                            var resp = await _financeManager.DeleteDistributionJournalAsync(Payable.DocumentNo, FLOG.Core.TRX_MODULE.TRX_PAYABLE);

                            if (resp.Valid)
                            {
                                _context.PayableTransactionTaxes.Where(x => x.PayableTransactionId == request.Body.PayableTransactionId).ToList().ForEach(p => _context.Remove(p));
                                _context.PayableTransactionDetails.Where(x => x.PayableTransactionId == request.Body.PayableTransactionId).ToList().ForEach(p => _context.Remove(p));
                                _context.PayableTransactionHeaders.Where(x => x.PayableTransactionId == request.Body.PayableTransactionId).ToList().ForEach(p => _context.Remove(p));

                                //UPDATE LAST NO
                                _docGenerator.DocNoDelete(Payable.DocumentNo, transaction.GetDbTransaction());

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
                            return ApiResult<Response>.ValidationError("Only New record can be deleted !");
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        return ApiResult<Response>.ValidationError("Payable Transaction not found !");
                    }

                    return ApiResult<Response>.Ok(response);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    return ApiResult<Response>.InternalServerError("Payable Transaction can not be deleted !");
                    //throw;
                }
            }
        }
    }
}
