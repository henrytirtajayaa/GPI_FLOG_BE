using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;
namespace FLOG_BE.Features.Companies.CompanySetup.PutCompanySetup
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
            if (string.IsNullOrEmpty(request.Body.CompanySetupId))
                return ValidationResult.ValidationError($"{nameof(request.Body.CompanySetupId)} cannot be null");
            if (string.IsNullOrEmpty(request.Body.CompanyId))
                return ValidationResult.ValidationError($"{nameof(request.Body.CompanyId)} cannot be null");
            if (string.IsNullOrEmpty(request.Body.CompanyName))
                return ValidationResult.ValidationError($"{nameof(request.Body.CompanyName)} cannot be null");
            if (string.IsNullOrEmpty(request.Body.CompanyAddressId.ToString()))
                return ValidationResult.ValidationError($"{nameof(request.Body.CompanyAddressId)} cannot be null");

            if (!Guid.TryParse(request.Body.CompanySetupId, out Guid guid))
            {
                return ValidationResult.ValidationError($"Incorrect format {nameof(request.Body.CompanySetupId)}");
            }

            if ((request.Body.TaxRegistrationNo).Length > 100)
                return ValidationResult.ValidationError($"{nameof(request.Body.TaxRegistrationNo)} cannot more than 100 characters");
            if ((request.Body.CompanyTaxName).Length > 100)
                return ValidationResult.ValidationError($"{nameof(request.Body.CompanyTaxName)} cannot more than 100 characters");           
            return ValidationResult.Ok();
        }
    }
}
