using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model.Central;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Infrastructure.Utils;
using Entities = FLOG_BE.Model.Companies.Entities;
using LinqKit;
using FLOG_BE.Model.Companies;
using FLOG.Core;
using FLOG_BE.Model.Central.Entities;
using FLOG_BE.Model.Companies.Entities;
using ViewEntities = FLOG_BE.Model.Companies.View;
using System.Data;
using FLOG_BE.Helper;

namespace FLOG_BE.Features.Finance.GLStatement.GetReportStatementTB
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FlogContext _contextCentral;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;

        public Handler(IHttpContextAccessor httpContextAccessor, FlogContext contextCentral, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _contextCentral = contextCentral;
            _login = login;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            if (string.IsNullOrEmpty(request.Initiator.UserId))
                return ApiResult<Response>.Unauthorized();

            var companySetup = await _context.CompanySetups.FirstOrDefaultAsync();

            var reportTitle = new ResponseReportTitle { Title="TRIAL BALANCE" };

            if (companySetup != null)
            {
                reportTitle.CompanyName = companySetup.CompanyName;
                //responseCompanySetup.LogoImageUrl = Helper.ImageBlob.ToImageUrl(companySetup.LogoImageData);
            }

            var finSetup = await (from f in _context.FinancialSetups
                                  join c in _context.Currencies on f.FuncCurrencyCode equals c.CurrencyCode
                                  orderby c.CurrencyCode
                                  select c).FirstOrDefaultAsync();
            if(finSetup != null)
            {
                reportTitle.Subtitle = string.Format("Amounts in {0} {1}", finSetup.CurrencyCode, finSetup.Description );
                reportTitle.DecimalPlaces = finSetup.DecimalPlaces;
            }

            reportTitle.PeriodOf = string.Format("Period Of {0}/{1}", request.Filter.PeriodIndex, request.Filter.PeriodYear);

            var trials = await getTrialBalances(request.Filter);

            return ApiResult<Response>.Ok(new Response()
            {
                ReportTitle = reportTitle,
                TrialBalances = trials,
                ListInfo = null
            }) ;
        }

        private async Task<List<ViewEntities.TrialBalance>> getTrialBalances(RequestFilter filter)
        {
            List<ViewEntities.TrialBalance> tb = new List<ViewEntities.TrialBalance>();

            if (filter.PeriodYear > 1000 && filter.PeriodIndex > 0)
            {
                string qry = string.Format("SELECT * FROM fxnGLTrialBalance({0},{1},'{2}') WHERE Balance <> 0 ORDER BY AccountId", filter.PeriodYear, filter.PeriodIndex, filter.BranchCode);

                if(filter.ShowZeroValue)
                    qry = string.Format("SELECT * FROM fxnGLTrialBalance({0},{1},'{2}') ORDER BY AccountId", filter.PeriodYear, filter.PeriodIndex, filter.BranchCode);

                tb = RawQuery.Select<ViewEntities.TrialBalance>(_context.Database, qry, null);
            }

            return tb;
        }

    }
}
