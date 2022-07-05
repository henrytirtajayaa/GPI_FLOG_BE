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
using Entities = FLOG_BE.Model.Central.Entities;

namespace FLOG_BE.Features.Central.User.PutUser
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
            var User = await _context.Persons.FirstOrDefaultAsync(x => x.PersonId == request.Body.UserId);
            var CekEmail = await _context.Persons.FirstOrDefaultAsync(x => x.EmailAddress == request.Body.EmailAddress && x.PersonId != request.Body.UserId);

            if (CekEmail != null)       
                return ApiResult<Response>.ValidationError($"{nameof(request.Body.EmailAddress)} is already in use");
     
            if (User != null)
            {
                User.PersonFullName = request.Body.UserFullName;
                if (User.PersonPassword != request.Body.UserPassword)
                    User.PersonPassword = _login.Encrypt(request.Body.UserPassword);
                else
                    User.PersonPassword = request.Body.UserPassword;

                User.EmailAddress = request.Body.EmailAddress;
                User.PersonCategoryId = request.Body.UserGroupId;
                User.InActive = request.Body.InActive;
                User.ModifiedBy = request.Initiator.UserId;
                User.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();
            }

            return ApiResult<Response>.Ok(new Response()
            {
                UserFullName = request.Body.UserFullName,
                EmailAddress = request.Body.EmailAddress,
                InActive = request.Body.InActive
            });
        }
    }
}
