using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model.Central;
using FLOG_BE.Model.Companies;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Infrastructure.Utils;
using Entities = FLOG_BE.Model.Companies.Entities;
using LinqKit;
using FLOG_BE.Model.Central.Entities;
using FLOG.Core.Finance.Util;
using FLOG.Core.Finance;

namespace FLOG_BE.Features.Companies.ExchangeRateHeader.GetCurrentExchangeRate
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FlogContext _contextCentral;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;
        private readonly IFiscalManager _fiscalManager;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, FlogContext contextCentral, ILogin login, HATEOASLinkCollection linkCollection) 
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _contextCentral = contextCentral;
            _login = login;
            _fiscalManager = new FiscalManager(_context);
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            if (string.IsNullOrEmpty(request.Initiator.UserId))
                return ApiResult<Response>.Unauthorized();

            var excRate = _fiscalManager.CurrentExchangeRate(request.Filter.CurrencyCode, request.Filter.TransactionDate, request.Filter.RateType);

            if (excRate.Count > 0)
            {
                return ApiResult<Response>.Ok(new Response()
                {
                    ExcRate = excRate.ElementAt(0).Key,
                    IsMultiply = excRate.ElementAt(0).Value,
                });
            }
            else
            {
                return ApiResult<Response>.Ok(new Response()
                {
                    ExcRate = 0,
                    IsMultiply = true,
                });
            }
            
        }

    }
}
