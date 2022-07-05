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

namespace FLOG_BE.Features.Central.User.PostUser
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
            string CategoryCode = "";
            var CekEmail = await _context.Persons.FirstOrDefaultAsync(x => x.EmailAddress == request.Body.EmailAddress);
            if (CekEmail == null)
            {
                var person = new Entities.Person()
                {
                    PersonFullName = request.Body.UserFullName,
                    PersonPassword = _login.Encrypt(request.Body.UserPassword),
                    EmailAddress = request.Body.EmailAddress,
                    PersonCategoryId = request.Body.UserGroupId,
                    InActive = request.Body.InActive,
                    CreatedBy = request.Initiator.UserId,
                    CreatedDate = DateTime.Now,
                };

                _context.Persons.Add(person);
                await _context.SaveChangesAsync();

                return ApiResult<Response>.Ok(new Response()
                {
                    UserId = person.PersonId,
                    UserFullName = person.PersonFullName,
                    EmailAddress = person.EmailAddress,
                    InActive = person.InActive
                });
            }
            else
            {
                return ApiResult<Response>.ValidationError($"{nameof(request.Body.EmailAddress)} is already in use");
            }
        }
    }
}
