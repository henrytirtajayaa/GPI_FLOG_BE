using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FLOG_BE.Model.Companies;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Entities = FLOG_BE.Model.Companies.Entities;

namespace FLOG_BE.Features.Companies.FinancialSetup.PostFinancialSetup
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        public readonly IHttpContextAccessor _httpContextAccessor;
        public readonly CompanyContext _context;
        public readonly ILogin _login;
        public readonly HATEOASLinkCollection _linkCollection;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _login = login;
            _linkCollection = linkCollection;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            if (await _context.FinancialSetups.AnyAsync(x => x.FuncCurrencyCode == request.Body.FuncCurrencyCode))
            {
                return ApiResult<Response>.ValidationError("Currency Code already exist");
            }

            var setup = new Entities.FinancialSetup()
            {
                FinancialSetupId = Guid.NewGuid(),
                FuncCurrencyCode = request.Body.FuncCurrencyCode,
                DefaultRateType = request.Body.DefaultRateType,
                TaxRateType = request.Body.TaxRateType,
                DeptSegmentNo = request.Body.DeptSegmentNo,
                CheckbookChargesType = request.Body.CheckbookChargesType,
                UomScheduleCode = request.Body.UomScheduleCode,
                Status = request.Body.Status,
                CreatedBy = request.Initiator.UserId,
                CreatedDate = DateTime.Now
            };

            _context.FinancialSetups.Add(setup);

            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response() 
            {
                FinancialSetupId = setup.FinancialSetupId.ToString(),
                FuncCurrencyCode = setup.FuncCurrencyCode,
                DefaultRateType = setup.DefaultRateType,
                TaxRateType = setup.TaxRateType,
                DeptSegmentNo = setup.DeptSegmentNo,
                Status = setup.Status
            });
        }
    }
}
