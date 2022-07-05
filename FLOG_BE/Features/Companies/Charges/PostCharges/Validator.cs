using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Companies.Charges.PostCharges
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
            if (string.IsNullOrEmpty(request.Body.ChargesCode))
                return ValidationResult.ValidationError($"{nameof(request.Body.ChargesCode)} cannot be null");
            if (request.Body.ChargesName == null)
                return ValidationResult.ValidationError($"{nameof(request.Body.ChargesName)} cannot be null");
            if (request.Body.TransactionType == null)
                return ValidationResult.ValidationError($"{nameof(request.Body.TransactionType)} cannot be null");
            if (request.Body.IsPurchasing == null)
                return ValidationResult.ValidationError($"{nameof(request.Body.IsPurchasing)} invalid!");
            if (request.Body.IsSales == null)
                return ValidationResult.ValidationError($"{nameof(request.Body.IsSales)} invalid!");
            if (request.Body.IsInventory == null)
                return ValidationResult.ValidationError($"{nameof(request.Body.IsInventory)} invalid!");
            if (request.Body.IsFinancial == null)
                return ValidationResult.ValidationError($"{nameof(request.Body.IsFinancial)} invalid!");
            if (request.Body.IsAsset == null)
                return ValidationResult.ValidationError($"{nameof(request.Body.IsAsset)} invalid!");
            if (request.Body.IsDeposit == null)
                return ValidationResult.ValidationError($"{nameof(request.Body.IsDeposit)} invalid!");

            return ValidationResult.Ok();
        }
    }
}
