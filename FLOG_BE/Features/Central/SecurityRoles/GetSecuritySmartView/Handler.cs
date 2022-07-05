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
using FLOG_BE.Model.Central.Entities;
using LinqKit;

namespace FLOG_BE.Features.Central.SecurityRoles.GetSecuritySmartView
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FlogContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;

        public Handler(IHttpContextAccessor httpContextAccessor, FlogContext context, ILogin login, HATEOASLinkCollection linkCollection)
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

            var query = getSmartRole(request.Filter);
          
            var list = await PaginatedList<SmartRole, ReponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());




            return ApiResult<Response>.Ok(new Response()
            {
                SmartRoles = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<SmartRole> getSmartRole(RequestFilter filter)

        {
            List<SmartView> ListSmartView = _context.Smartviews.ToList();

            var query = (from x in _context.SmartRoles
                         .Where(x => x.SecurityRoleId == Guid.Parse(filter.SecurityRoleId))
                         select new Model.Central.Entities.SmartRole
                         {
                             Id = x.Id,                             
                             SmartviewId = x.SmartviewId,
                             SecurityRoleId = x.SecurityRoleId,
                             SmartView = ListSmartView.Where(p => p.SmartviewId== x.SmartviewId).FirstOrDefault(),
                         }).AsEnumerable().ToList().AsQueryable();


            return query;
        }

    }
}
