using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model.Companies;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Infrastructure.Utils;
using Entities = FLOG_BE.Model.Companies.Entities;

namespace FLOG_BE.Features.Companies.SalesPerson.PostSalesPerson
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
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
            if (await _context.SalesPersons.AnyAsync(x => x.SalesCode == request.Body.SalesCode))
                return ApiResult<Response>.ValidationError("Sales Code already exist.");

            var SalesPerson = new Entities.SalesPerson()
            {
                SalesPersonId = Guid.NewGuid(),
                SalesCode = request.Body.SalesCode,
                SalesName = request.Body.SalesName,
                PersonId = request.Body.PersonId
            };

            _context.SalesPersons.Add(SalesPerson);
            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response()
            {
                SalesPersonId = SalesPerson.SalesPersonId,
                SalesCode = SalesPerson.SalesCode,
                SalesName = SalesPerson.SalesName,
                PersonId = SalesPerson.PersonId
            });
        }
    }
}
