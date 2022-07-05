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

namespace FLOG_BE.Features.Companies.TaxSchedule.PutTaxSchedule
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
                TaxScheduleId = request.Body.TaxScheduleId,
                TaxScheduleCode = request.Body.TaxScheduleCode,
                Description = request.Body.Description
            };

            var Tax = await _context.TaxSchedules.FirstOrDefaultAsync(x => x.TaxScheduleId == request.Body.TaxScheduleId);
            if (Tax != null)
            {
                Tax.TaxScheduleCode = request.Body.TaxScheduleCode;
                Tax.Description = request.Body.Description;
                Tax.IsSales = request.Body.IsSales;
                Tax.PercentOfSalesPurchase = request.Body.PercentOfSalesPurchase;
                Tax.TaxablePercent = request.Body.TaxablePercent;
                Tax.RoundingType = request.Body.RoundingType;
                Tax.RoundingLimitAmount = request.Body.RoundingLimitAmount;
                Tax.TaxInclude = request.Body.TaxInclude;
                Tax.WithHoldingTax = request.Body.WithHoldingTax;
                Tax.TaxAccountNo = request.Body.TaxAccountNo;
                Tax.Inactive = request.Body.Inactive;
                Tax.ModifiedBy = request.Initiator.UserId;
                Tax.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();
                response.TaxScheduleId = Tax.TaxScheduleId;

            }
            else
            {
                return ApiResult<Response>.ValidationError("Tax Schedule not found.");
            }

            return ApiResult<Response>.Ok(response);


        }
    }
}
