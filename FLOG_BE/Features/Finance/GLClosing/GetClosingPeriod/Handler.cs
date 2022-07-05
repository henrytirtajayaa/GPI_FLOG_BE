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

namespace FLOG_BE.Features.Finance.GLClosing.GetClosingPeriod
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

            var fiscalPeriod = _context.FiscalPeriodHeaders.Where(f => f.PeriodYear == DateTime.Now.Year).FirstOrDefault();

            if(fiscalPeriod != null)
            {
                var lastClosing = await (from cl in _context.GLClosingHeaders
                                         where cl.Status == DOCSTATUS.NEW && !cl.IsYearly
                                         orderby cl.PeriodYear descending, cl.PeriodIndex descending
                                         select cl).FirstOrDefaultAsync();

                if (lastClosing != null)
                {
                    if (lastClosing.PeriodIndex < fiscalPeriod.TotalPeriod)
                    {
                        List<int> periodIndexs = new List<int> { lastClosing.PeriodIndex+1 };

                        period.PeriodYear = lastClosing.PeriodYear;
                        period.PeriodIndexs = periodIndexs;
                    }
                    else
                    {
                        List<int> periodIndexs = new List<int> { 1 };

                        period.PeriodYear = lastClosing.PeriodYear + 1;
                        period.PeriodIndexs = periodIndexs;
                    }
                }
                else
                {
                    period.PeriodYear = fiscalPeriod.PeriodYear;
                    period.PeriodIndexs = new List<int> { 1 };
                }

                return ApiResult<Response>.Ok(new Response()
                {
                    Period = period,
                    ListInfo = null
                });
            }
            else
            {
                return ApiResult<Response>.ValidationError(string.Format("Fiscal Period of {0} not found !", DateTime.Now.Year));
            }
            
        }
    }
}
