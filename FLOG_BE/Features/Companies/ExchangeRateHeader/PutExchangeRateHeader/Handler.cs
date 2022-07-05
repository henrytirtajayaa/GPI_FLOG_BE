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

namespace FLOG_BE.Features.Companies.ExchangeRateHeader.PutExchangeRateHeader
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
                ExchangeRateCode = request.Body.ExchangeRateCode,
                Description = request.Body.Description,
                Status = request.Body.Status
            };

            var header = await _context.ExchangeRateHeaders.FirstOrDefaultAsync(x => x.ExchangeRateHeaderId == request.Body.ExchangeRateHeaderId);

            if (header != null)
            {
                header.ExchangeRateCode = request.Body.ExchangeRateCode;
                header.Description = request.Body.Description;
                header.CurrencyCode = request.Body.CurrencyCode;
                header.RateType = request.Body.RateType;
                header.ExpiredPeriod = request.Body.ExpiredPeriod;
                header.CalculationType = request.Body.CalculationType;
                header.Status = request.Body.Status;
                header.ModifiedBy = request.Initiator.UserId;
                header.ModifiedDate = DateTime.Now;

                //REMOVE EXISTING
                _context.ExchangeRateDetails
                .Where(x => x.ExchangeRateHeaderId == header.ExchangeRateHeaderId).ToList().ForEach(p => _context.Remove(p));

                foreach (var item in request.Body.ExchangeRateDetails)
                {
                    var ExchangeRateDetail = new Entities.ExchangeRateDetail()
                    {
                        ExchangeRateDetailId = Guid.NewGuid(),
                        ExchangeRateHeaderId = header.ExchangeRateHeaderId,
                        RateDate = item.RateDate,
                        ExpiredDate = item.ExpiredDate,
                        RateAmount = item.RateAmount,
                        Status = item.Status
                    };

                    _context.ExchangeRateDetails.Add(ExchangeRateDetail);
                }
            }
            
            await _context.SaveChangesAsync();

            response.ExchangeRateCode = header.ExchangeRateCode;

            return ApiResult<Response>.Ok(response);
        }
    }
}
