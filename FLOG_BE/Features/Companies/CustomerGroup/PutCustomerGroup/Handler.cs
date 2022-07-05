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

namespace FLOG_BE.Features.Companies.CustomerGroup.PutCustomerGroup
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
              CustomerGroupId = request.Body.CustomerGroupId
            };

            var editted = await _context.CustomerGroups.FirstOrDefaultAsync(x => x.CustomerGroupId == request.Body.CustomerGroupId);
            if (editted != null)
            {
                editted.CustomerGroupCode = request.Body.CustomerGroupCode;
                editted.CustomerGroupName = request.Body.CustomerGroupName;
                editted.PaymentTermCode = request.Body.PaymentTermCode;
                editted.ReceivableAccountNo = request.Body.ReceivableAccountNo;
                editted.AccruedReceivableAccountNo = request.Body.AccruedReceivableAccountNo;
                editted.Inactive = request.Body.Inactive;
                editted.ModifiedBy = request.Initiator.UserId;
                editted.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();
                response.CustomerGroupId = editted.CustomerGroupId;
            }
            else {
                return ApiResult<Response>.ValidationError("Customer Group not found.");
            }

            return ApiResult<Response>.Ok(response);
        }
    }
}
