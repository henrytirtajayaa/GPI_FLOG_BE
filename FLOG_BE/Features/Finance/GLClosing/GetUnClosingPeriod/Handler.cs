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
using LinqKit;
using FLOG.Core.Finance;
using FLOG.Core.Finance.Util;
using FLOG.Core;

namespace FLOG_BE.Features.Finance.GLClosing.GetUnClosingPeriod
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;
        private readonly IFiscalManager _fiscalManager;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
            _fiscalManager = new FiscalManager(_context);
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            if (string.IsNullOrEmpty(request.Initiator.UserId))
                return ApiResult<Response>.Unauthorized();

            PeriodResponse period = new PeriodResponse();

            var lastClosingMonth = await (from cl in _context.GLClosingHeaders
                                     where cl.Status == DOCSTATUS.NEW && !cl.IsYearly
                                     orderby cl.PeriodYear descending, cl.PeriodIndex descending
                                     select cl).FirstOrDefaultAsync();

            if (lastClosingMonth != null)
            {
                List<int> periodIndexs = new List<int>();
                for (int i = 1; i <= lastClosingMonth.PeriodIndex; i++)
                {
                    periodIndexs.Add(i);
                }

                period.PeriodYear = lastClosingMonth.PeriodYear;
                period.PeriodIndexs = periodIndexs;

                return ApiResult<Response>.Ok(new Response()
                {
                    Period = period,
                    ListInfo = null
                });
            }
            else
            {
                return ApiResult<Response>.ValidationError(string.Format("No Closed Periods exist !"));
            }            
        }
    
    }
}
