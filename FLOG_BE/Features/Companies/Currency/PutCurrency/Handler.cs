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

namespace FLOG_BE.Features.Companies.Currency.PutCurrency
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

            var response = new Response()
            {
              CurrencyId = request.Body.CurrencyId
            };

            var currency = await _context.Currencies.FirstOrDefaultAsync(x => x.CurrencyId == request.Body.CurrencyId);
            if (currency != null)
            {
                currency.Description = request.Body.Description;
                currency.DecimalPlaces = request.Body.DecimalPlaces;
                currency.Symbol = request.Body.Symbol;
                currency.RealizedGainAcc = request.Body.RealizedGainAcc;
                currency.RealizedLossAcc = request.Body.RealizedLossAcc;
                currency.UnrealizedGainAcc = request.Body.UnrealizedGainAcc;
                currency.UnrealizedLossAcc = request.Body.UnrealizedLossAcc;
                currency.Inactive = request.Body.Inactive;
                currency.ModifiedBy = request.Initiator.UserId;
                currency.ModifiedDate = DateTime.Now;
                currency.CurrencyUnit = request.Body.CurrencyUnit;
                currency.CurrencySubUnit = request.Body.CurrencySubUnit;

                await _context.SaveChangesAsync();
                response.CurrencyId = currency.CurrencyId;
                response.CurrencyCode = currency.CurrencyCode;
            }
            else {
                return ApiResult<Response>.ValidationError("Currency not found.");
            }

                return ApiResult<Response>.Ok(response);
          
            
        }
    }
}
