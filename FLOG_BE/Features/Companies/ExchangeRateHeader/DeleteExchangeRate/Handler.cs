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

namespace FLOG_BE.Features.Companies.ExchangeRateHeader.DeleteExchangeRate
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
            using (var trx = _context.Database.BeginTransaction())
            {

                var details = _context.ExchangeRateDetails.Where(x => x.ExchangeRateHeaderId == request.Body.ExchangeRateHeaderId);
                _context.RemoveRange(details);

                var record = _context.ExchangeRateHeaders.FirstOrDefault(x => x.ExchangeRateHeaderId == request.Body.ExchangeRateHeaderId);
                _context.Attach(record);
                _context.Remove(record);

                int iRes = await _context.SaveChangesAsync();

                if (iRes > 0)
                {
                    trx.Commit();
                }
                else
                {
                    trx.Rollback();
                }
            }

            return ApiResult<Response>.Ok(new Response()
            {
                ExchangeRateHeaderId = request.Body.ExchangeRateHeaderId
            });
        }
    }
}
