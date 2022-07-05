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

namespace FLOG_BE.Features.Central.UserGroup.PostUserGroup
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
            if (await _context.PersonCategories.AnyAsync(x => x.PersonCategoryCode == request.Body.UserGroupCode))
            {
                return ApiResult<Response>.ValidationError("User Group Code already exist.");            
            }

            Guid obj = Guid.NewGuid();
            var personcategory = new PersonCategory()
            {
                PersonCategoryId = obj.ToString(),
                PersonCategoryCode = request.Body.UserGroupCode,
                PersonCategoryName = request.Body.UserGroupName,
                CreatedBy = request.Initiator.UserId,
                CreatedDate = DateTime.Now
            };

            _context.PersonCategories.Add(personcategory);

            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response()
            {
                UserGroupId = personcategory.PersonCategoryId,
                UserGroupCode = personcategory.PersonCategoryCode,
                UserGroupName = personcategory.PersonCategoryName
            });
        }
    }
}
