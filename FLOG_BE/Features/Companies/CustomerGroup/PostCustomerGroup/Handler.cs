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

namespace FLOG_BE.Features.Companies.CustomerGroup.PostCustomerGroup
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

            var customerGroup = new Entities.CustomerGroup()
            {
                CustomerGroupId = Guid.NewGuid(),
                CustomerGroupCode = request.Body.CustomerGroupCode,
                CustomerGroupName = request.Body.CustomerGroupName,
                PaymentTermCode = request.Body.PaymentTermCode,
                ReceivableAccountNo = request.Body.ReceivableAccountNo,
                AccruedReceivableAccountNo = request.Body.AccruedReceivableAccountNo,
                Inactive = request.Body.Inactive,

                CreatedBy = request.Initiator.UserId,
                CreatedDate = DateTime.Now
            };

            _context.CustomerGroups.Add(customerGroup);
            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response()
            {
                CustomerGroupId = customerGroup.CustomerGroupId,
                CustomerGroupCode = request.Body.CustomerGroupCode,
                CustomerGroupName = request.Body.CustomerGroupName,
            });
        }
    }
}
