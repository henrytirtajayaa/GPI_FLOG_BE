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

namespace FLOG_BE.Features.Companies.TaxSchedule.PostTaxSchedule
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

            if (await _context.TaxSchedules.AnyAsync(x => x.TaxScheduleCode == request.Body.TaxScheduleCode))
                return ApiResult<Response>.ValidationError("Tax Schedule Code already exist.");
            

            var Tax = new Entities.TaxSchedule()
            {

                TaxScheduleId = Guid.NewGuid(),
                TaxScheduleCode = request.Body.TaxScheduleCode,
                Description = request.Body.Description,
                IsSales = request.Body.IsSales,
                PercentOfSalesPurchase = request.Body.PercentOfSalesPurchase,
                TaxablePercent = request.Body.TaxablePercent,
                RoundingType = request.Body.RoundingType,
                RoundingLimitAmount = request.Body.RoundingLimitAmount,
                TaxInclude = request.Body.TaxInclude,
                WithHoldingTax = request.Body.WithHoldingTax,
                TaxAccountNo = request.Body.TaxAccountNo,
                Inactive = request.Body.Inactive,
                CreatedBy = request.Initiator.UserId,
                CreatedDate = DateTime.Now
            };

            _context.TaxSchedules.Add(Tax);

            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response()
            {
                TaxScheduleId = Tax.TaxScheduleId,
                TaxScheduleCode = Tax.TaxScheduleCode,
                Description = Tax.Description
            });
        }
    }
}
