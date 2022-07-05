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
using System.Data;

namespace FLOG_BE.Features.Central.Smartview.GetDefaultSmartview
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
            FLOG.Core.Smartview.SmartviewQuery SmartViewQuery = new FLOG.Core.Smartview.SmartviewQuery(_context);

            var GetSmartView = await _contextCentral.Smartviews.FirstOrDefaultAsync(x => x.SmartviewId == request.SmartviewId);
            var GetData = SmartViewQuery.DefaultSmartview(GetSmartView.SqlViewName);

            var Column = new List<ResponseColumn>();
            foreach (DataColumn item in GetData.Columns)
            {
                if (item.ColumnName != null)
                {
                    var columns = new ResponseColumn()
                    {
                        Column = item.ColumnName
                    };
                    Column.Add(columns);
                }
            }

            return ApiResult<Response>.Ok(new Response()
            {
                Smartviews = GetData,
                Columns = Column
            });
        }
    }
}
