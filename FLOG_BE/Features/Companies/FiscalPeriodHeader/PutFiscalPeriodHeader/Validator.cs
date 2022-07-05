using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Companies.FiscalPeriodHeader.PutFiscalPeriodHeader
{
    public class Validator : AsyncRequestValidator<Request>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Validator(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override async Task<ValidationResult> Validate(Request request)
        {
            if (string.IsNullOrEmpty(request.Body.FiscalHeaderId.ToString()))
                return ValidationResult.ValidationError($"{nameof(request.Body.FiscalHeaderId)} cannot be null");

            if (request.Body.PeriodYear < 0)
                return ValidationResult.ValidationError($"{nameof(request.Body.PeriodYear)} cannot be null");

            if (request.Body.TotalPeriod < 0)
                return ValidationResult.ValidationError($"{nameof(request.Body.TotalPeriod)} cannot be null");

            if (request.Body.TotalPeriod < 12)
                return ValidationResult.ValidationError($"{nameof(request.Body.TotalPeriod)} min value 12");

            if (string.IsNullOrEmpty(request.Body.StartDate.ToString()))
                return ValidationResult.ValidationError($"{nameof(request.Body.StartDate)} cannot be null");

            if (string.IsNullOrEmpty(request.Body.EndDate.ToString()))
                return ValidationResult.ValidationError($"{nameof(request.Body.EndDate)} cannot be null");

            return ValidationResult.Ok();
        }
    }
}
