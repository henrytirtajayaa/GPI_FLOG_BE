using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Companies.TaxSchedule.PutTaxSchedule
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


            if (string.IsNullOrEmpty(request.Body.TaxScheduleCode))
                return ValidationResult.ValidationError($"{nameof(request.Body.TaxScheduleCode)} cannot be null");

            if (request.Body.IsSales == null)
                return ValidationResult.ValidationError($"{nameof(request.Body.IsSales)} cannot be null");

            if (request.Body.PercentOfSalesPurchase < 1)
                return ValidationResult.ValidationError($"{nameof(request.Body.PercentOfSalesPurchase)} must be  greater than 0");

            if (request.Body.PercentOfSalesPurchase > 100)
                return ValidationResult.ValidationError($"{nameof(request.Body.PercentOfSalesPurchase)} max value 100");

            if (request.Body.TaxablePercent < 0)
                return ValidationResult.ValidationError($"{nameof(request.Body.TaxablePercent)} must be  greater than 0 or equal  to 0");

            if (request.Body.TaxInclude == null)
                return ValidationResult.ValidationError($"{nameof(request.Body.TaxInclude)} cannot be null");

            if (request.Body.WithHoldingTax == null)
                return ValidationResult.ValidationError($"{nameof(request.Body.WithHoldingTax)} cannot be null");


            if ((request.Body.Description).Length > 100)
                return ValidationResult.ValidationError($"{nameof(request.Body.Description)} cannot more than 100 characters");

            return ValidationResult.Ok();
        }
    }
}
