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

namespace FLOG_BE.Features.Companies.SalesPerson.PutSalesPerson
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
            var response = new Response()
            {
                SalesPersonId = request.Body.SalesPersonId,
                SalesCode = request.Body.SalesCode,
                SalesName = request.Body.SalesName
            };

            var SalesPerson = await _context.SalesPersons.FirstOrDefaultAsync(x => x.SalesPersonId== request.Body.SalesPersonId);
            if (SalesPerson != null)
            {
                SalesPerson.SalesPersonId = request.Body.SalesPersonId;
                SalesPerson.SalesCode = request.Body.SalesCode;
                SalesPerson.SalesName = request.Body.SalesName;
                SalesPerson.PersonId = request.Body.PersonId;

                _context.SalesPersons.Update(SalesPerson);

                await _context.SaveChangesAsync();
                response.SalesPersonId = SalesPerson.SalesPersonId;
                response.SalesName = SalesPerson.SalesName;
            }
            else {
                return ApiResult<Response>.ValidationError("Sales Person Id not found.");
            }

            return ApiResult<Response>.Ok(response);
            
        }
    }
}
