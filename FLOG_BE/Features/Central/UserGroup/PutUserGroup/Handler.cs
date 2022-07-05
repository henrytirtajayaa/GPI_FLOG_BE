using FLOG_BE.Model.Central;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Central.UserGroup.PutUserGroup
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
            _context = context;
            _login = login;
            _linkCollection = linkCollection;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            var response = new Response()
            {
                UserGroupId = request.Body.UserGroupId,
                UserGroupName = request.Body.UserGroupName
            };

            var usergroup = await _context.PersonCategories.FirstOrDefaultAsync(x => x.PersonCategoryId == request.Body.UserGroupId);
            if (usergroup != null)
            {
                usergroup.PersonCategoryName = request.Body.UserGroupName;
                usergroup.UpdatedBy = request.Initiator.UserId;
                usergroup.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                response.UserGroupName = usergroup.PersonCategoryName;
            }

            return ApiResult<Response>.Ok(response);
        }
    }
}
