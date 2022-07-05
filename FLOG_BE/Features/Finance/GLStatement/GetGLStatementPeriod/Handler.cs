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

namespace FLOG_BE.Features.Finance.GLStatement.GetGLStatementPeriod
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
            if (string.IsNullOrEmpty(request.Initiator.UserId))
                return ApiResult<Response>.Unauthorized();

            var closings = _context.GLClosingHeaders.Where(x => x.Status == DOCSTATUS.NEW || x.Status == DOCSTATUS.POST).OrderByDescending(x => x.PeriodYear).AsQueryable();

            List<int> closedYears = closings.GroupBy(x => x.PeriodYear).OrderByDescending(o => o.Key).Select(s => s.Key).ToList();

            List<PeriodResponse> periods = new List<PeriodResponse>();

            if(closedYears.Count > 0)
            {
                foreach(int periodYear in closedYears)
                {
                    PeriodResponse period = new PeriodResponse();
                    period.PeriodYear = periodYear;
                    period.PeriodIndexs = closings.Where(x => x.PeriodYear == periodYear).OrderBy(x => x.PeriodIndex).Select(s => s.PeriodIndex).Distinct().ToList();

                    periods.Add(period);
                }
                
                return ApiResult<Response>.Ok(new Response()
                {
                    ClosedYears = closedYears,
                    Periods = periods,
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
