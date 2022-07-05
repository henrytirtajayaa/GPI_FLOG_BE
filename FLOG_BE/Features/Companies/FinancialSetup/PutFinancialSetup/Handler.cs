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

namespace FLOG_BE.Features.Companies.FinancialSetup.PutFinancialSetup
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
                FuncCurrencyCode = request.Body.FuncCurrencyCode,
                DefaultRateType = request.Body.DefaultRateType,
                TaxRateType = request.Body.TaxRateType,
                DeptSegmentNo = request.Body.DeptSegmentNo,
                CheckbookChargesType = request.Body.CheckbookChargesType,
                UomScheduleCode = request.Body.UomScheduleCode,           
                Status = request.Body.Status,
            };

            var setup = await _context.FinancialSetups.FirstOrDefaultAsync(x => x.FinancialSetupId == Guid.Parse(request.Body.FinancialSetupId));
            if (setup != null)
            {
                setup.FuncCurrencyCode = request.Body.FuncCurrencyCode;
                setup.DefaultRateType = request.Body.DefaultRateType;
                setup.TaxRateType = request.Body.TaxRateType;
                setup.DeptSegmentNo = request.Body.DeptSegmentNo;
                setup.CheckbookChargesType = request.Body.CheckbookChargesType;
                setup.UomScheduleCode = request.Body.UomScheduleCode;
                setup.Status = request.Body.Status;
                setup.ModifiedBy = request.Initiator.UserId;
                setup.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                response.FuncCurrencyCode = setup.FuncCurrencyCode;
            }

            return ApiResult<Response>.Ok(response);
        }
    }
}
