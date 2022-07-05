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

namespace FLOG_BE.Features.Central.Smartview.GetSmartviewByRoleId
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
            var result = new List<ResponseItem>();
            var smartRole = await _contextCentral.SmartRoles.Where(x => x.SecurityRoleId == request.SecurityRoleId).ToListAsync();
            foreach (var item in smartRole)
            {
                var smart = await _contextCentral.Smartviews.FirstOrDefaultAsync(x => x.SmartviewId == item.SmartviewId);
                var res = new ResponseItem()
                {
                    SmartviewId = smart.SmartviewId,
                    SmartTitle = smart.SmartTitle,
                    SqlViewName = smart.SqlViewName,
                    GroupName = smart.GroupName,
                    isFunction = smart.isFunction
                };

                result.Add(res);
            }

            return ApiResult<Response>.Ok(new Response()
            {
                Smartviews = result
            });
        }
    }
}
