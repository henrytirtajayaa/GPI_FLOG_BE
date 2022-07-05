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

namespace FLOG_BE.Features.Authentication.ChangePassword
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
            var response = new Response()
            {
                password = ""
            };
            var person = await _context.Persons.FirstOrDefaultAsync(x => x.PersonId == request.Initiator.UserId);

            if (_login.PasswordIsMatch(request.Body.OldPassword, person.PersonPassword))
            {}else
            {
                return ApiResult<Response>.ValidationError($"Incorrect {nameof(request.Body.OldPassword)}!");
            }

            if (person != null)
            {
                person.PersonPassword = _login.Encrypt(request.Body.NewPassword);
                person.ModifiedBy = request.Initiator.UserId;
                person.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                response.password = person.PersonPassword;
            }

            return ApiResult<Response>.Ok(response);
        }
    }
}
