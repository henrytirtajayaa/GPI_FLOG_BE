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
using FLOG_BE.Model.Companies.Entities;

namespace FLOG_BE.Features.Companies.Bank.PutBank
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
                BankCode = request.Body.BankCode,
                BankName = request.Body.BankName
            };
            var bank = await _context.Banks.FirstOrDefaultAsync(x => x.BankId == Guid.Parse(request.Body.BankId));
            if(bank != null)
            {
                bank.BankCode = request.Body.BankCode;
                bank.BankName = request.Body.BankName;
                bank.Address = request.Body.Address;
                bank.CityCode = request.Body.CityCode;
                bank.InActive = request.Body.Inactive;
                bank.ModifiedBy = request.Initiator.UserId;
                bank.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();
                response.BankName = bank.BankName;
            }
            
            return ApiResult<Response>.Ok(response);
        }
    }
}
