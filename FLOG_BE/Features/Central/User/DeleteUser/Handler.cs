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

namespace FLOG_BE.Features.Central.User.DeleteUser
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
            var CekCompanySecurity = _context.CompanySecurities.FirstOrDefault(x => x.PersonId == request.Body.UserId);
            if (CekCompanySecurity == null)
            {
                var record = _context.Persons.FirstOrDefault(x => x.PersonId == request.Body.UserId);
                _context.Attach(record);
                _context.Remove(record);
                await _context.SaveChangesAsync();

                return ApiResult<Response>.Ok(new Response()
                {
                    UserId = request.Body.UserId,
                    UserCategoryCode = record.PersonCategoryId,
                    UserFullName = record.PersonFullName,
                    EmailAddress = record.EmailAddress,
                    InActive = record.InActive
                });
            }
            else
            {
                return ApiResult<Response>.ValidationError("User already in use");
            }
        }
    }
}
