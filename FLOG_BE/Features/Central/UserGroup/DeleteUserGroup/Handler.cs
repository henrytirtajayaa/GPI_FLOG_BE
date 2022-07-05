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

namespace FLOG_BE.Features.Central.UserGroup.DeleteUserGroup
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
            var record = _context.PersonCategories.FirstOrDefault(x => x.PersonCategoryId == request.Body.UserGroupId);
            
            if (record == null)
            {
                return ApiResult<Response>.ValidationError("UserGroup Not Found!");  
            } else {
                _context.Attach(record);
                _context.Remove(record);
                await _context.SaveChangesAsync();

                return ApiResult<Response>.Ok(new Response()
                {
                    UserGroupId = request.Body.UserGroupId
                });
            };
            
        }
    }
}
