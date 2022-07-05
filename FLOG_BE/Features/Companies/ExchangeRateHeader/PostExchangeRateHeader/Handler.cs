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


namespace FLOG_BE.Features.Companies.ExchangeRateHeader.PostExchangeRateHeader
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
            if (await _context.ExchangeRateHeaders.AnyAsync(x => x.ExchangeRateCode == request.Body.ExchangeRateCode))
            {
                return ApiResult<Response>.ValidationError("Exchange Code already exist");
            }

            if (await _context.ExchangeRateHeaders.AnyAsync(x => x.CurrencyCode == request.Body.CurrencyCode && x.RateType == request.Body.RateType))
            {
                return ApiResult<Response>.ValidationError("Exchange Rate already exist");
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                var app = new Entities.ExchangeRateHeader()
                {
                    ExchangeRateHeaderId = Guid.NewGuid(),
                    ExchangeRateCode = request.Body.ExchangeRateCode,
                    Description = request.Body.Description,
                    CurrencyCode = request.Body.CurrencyCode,
                    RateType = request.Body.RateType,
                    ExpiredPeriod = request.Body.ExpiredPeriod,
                    CalculationType = request.Body.CalculationType,
                    Status = request.Body.Status,
                    CreatedBy = request.Initiator.UserId,
                    CreatedDate = DateTime.Now
                };

                _context.ExchangeRateHeaders.Add(app);
                await _context.SaveChangesAsync();


                if (app.ExchangeRateHeaderId != null && app.ExchangeRateHeaderId != Guid.Empty)
                {
                    if (request.Body.ExchangeRateDetails.Count > 0)
                    {
                        foreach (var item in request.Body.ExchangeRateDetails)
                        {

                            var ExchangeRateDetails = new Entities.ExchangeRateDetail()
                            {
                                ExchangeRateDetailId = Guid.NewGuid(),
                                ExchangeRateHeaderId = app.ExchangeRateHeaderId,
                                RateDate = item.RateDate,
                                ExpiredDate = item.ExpiredDate,
                                RateAmount = item.RateAmount,
                                Status = item.Status
                            };
                            _context.ExchangeRateDetails.Add(ExchangeRateDetails);
                        }

                        await _context.SaveChangesAsync();

                        transaction.Commit();
                        return ApiResult<Response>.Ok(new Response()
                        {
                            ExchangeRateHeaderId = app.ExchangeRateHeaderId,
                            ExchangeRateCode = app.ExchangeRateCode,
                            Description = app.Description
                        });
                    }
                    else
                    {
                        transaction.Rollback();
                        return ApiResult<Response>.ValidationError("Exchange Rate not exist.");
                    }
                }
                else
                {

                    transaction.Rollback();
                    return ApiResult<Response>.ValidationError("Exchange Rate can not be stored.");
                }
            }
        }
    }
}

