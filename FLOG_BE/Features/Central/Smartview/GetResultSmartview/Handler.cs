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

namespace FLOG_BE.Features.Central.Smartview.GetResultSmartview
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
            
            string query = "";

            if(request.Filter.Count > 0)
            {
                query = " WHERE ";
            }

            var index = 0;
            foreach (var item in request.Filter)
            {
                if(index > 0)
                {
                    query += " AND ";
                }

                if (item.Filter == "Between")
                    query += $" {item.Field} BETWEEN {item.Param1} AND {item.Param2}";

                if (item.Filter == "Contains")
                    query += $" {item.Field} LIKE '%{item.Param1}%'";
                
                index++;
            }

            var GetSmartView = await _contextCentral.Smartviews.FirstOrDefaultAsync(x => x.SmartviewId == request.SmartviewId);
            var GetData = SmartViewQuery.ResultSmartview(GetSmartView.SqlViewName, query);

            return ApiResult<Response>.Ok(new Response()
            {
                Smartviews = GetData
            });
        }
    }
}
