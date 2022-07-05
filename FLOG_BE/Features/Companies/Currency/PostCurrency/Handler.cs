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

namespace FLOG_BE.Features.Companies.Currency.PostCurrency
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {


            var CurrencyCode = _context.Currencies.Where(x => x.CurrencyCode == request.Body.CurrencyCode).FirstOrDefault();

            if (CurrencyCode != null)
                return ApiResult<Response>.ValidationError("Currency Code already exist");

            var currency = new Entities.Currency()
            {
                CurrencyId = Guid.NewGuid(),
                CurrencyCode = request.Body.CurrencyCode,
                Description = request.Body.Description,
                DecimalPlaces = request.Body.DecimalPlaces,
                Symbol = request.Body.Symbol,
                RealizedGainAcc = request.Body.RealizedGainAcc,
                RealizedLossAcc = request.Body.RealizedLossAcc,
                UnrealizedGainAcc = request.Body.UnrealizedGainAcc,
                UnrealizedLossAcc = request.Body.UnrealizedLossAcc,
                Inactive = request.Body.Inactive,
                CurrencyUnit = request.Body.CurrencyUnit,
                CurrencySubUnit = request.Body.CurrencySubUnit,

                CreatedBy = request.Initiator.UserId,
                CreatedDate = DateTime.Now
            };

            _context.Currencies.Add(currency);
            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response()
            {
                CurrencyId = currency.CurrencyId,
                Description = currency.Description,
                DecimalPlaces = currency.DecimalPlaces,
                Symbol = currency.Symbol,
                CurrencyCode = currency.CurrencyCode,
                RealizedGainAcc = currency.RealizedGainAcc,
                RealizedLossAcc = currency.RealizedLossAcc,
                UnrealizedGainAcc = currency.UnrealizedGainAcc,
                UnrealizedLossAcc = currency.UnrealizedLossAcc,
                Inactive = currency.Inactive,
                CurrencyUnit = currency.CurrencyUnit,
                CurrencySubUnit = currency.CurrencySubUnit,
            });
        }
    }
}
