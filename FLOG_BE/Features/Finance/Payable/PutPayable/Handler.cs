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

namespace FLOG_BE.Features.Finance.Payable.PutPayable
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
            _financeManager = new FinanceManager(context);
            _repository = new Repository(context);
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
                    var payableTrx = await _repository.UpdateHeader(request.Body, request.Initiator);
                    if (payableTrx != null)
                    {
                        var payableDetails = await _repository.InsertPayableDetails(request.Body);
                        var payableTaxes = await _repository.InsertPayableTax(request.Body);

                       JournalResponse jResponse = await _financeManager.CreateDistributionJournalAsync(payableTrx, payableDetails, payableTaxes);

                        if (jResponse.Valid)
                        {
                            await _context.SaveChangesAsync();

                            transaction.Commit();

                            return ApiResult<Response>.Ok(response);
                        }
                        else
                        {
                            transaction.Rollback();

                            return ApiResult<Response>.ValidationError(jResponse.ErrorMessage);
                        }
                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return ApiResult<Response>.ValidationError($"Payable Transaction error : " + e.Message);
                }
            }

            return ApiResult<Response>.ValidationError($"Record can not be saved ! ");
        }

    }
}
