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

namespace FLOG_BE.Features.Companies.Bank.PostBank
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

            if (await _context.Banks.AnyAsync(x => x.BankCode == request.Body.BankCode))
                return ApiResult<Response>.ValidationError("Bank already exist.");
            
            var bank = new Entities.Bank()
            {
                BankId = Guid.NewGuid(),
                BankCode = request.Body.BankCode,
                BankName = request.Body.BankName,
                Address = request.Body.Address,
                CityCode = request.Body.CityCode,
                InActive = request.Body.Inactive,
                CreatedBy = request.Initiator.UserId,
                CreatedDate = DateTime.Now
            };

            _context.Banks.Add(bank);

            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response()
            {
                BankId = bank.BankId.ToString(),
                BankCode = bank.BankCode,
                BankName = bank.BankName
            });
        }
    }
}
