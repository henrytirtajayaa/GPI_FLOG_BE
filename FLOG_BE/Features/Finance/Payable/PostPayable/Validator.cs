using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;


namespace FLOG_BE.Features.Finance.Payable.PostPayable
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
            if (string.IsNullOrEmpty(request.Body.DocumentType))
                return ValidationResult.ValidationError($"{nameof(request.Body.DocumentType)} cannot be null");

            if (string.IsNullOrEmpty(request.Body.CurrencyCode))
                return ValidationResult.ValidationError($"{nameof(request.Body.CurrencyCode)} cannot be null");

            if (request.Body.VendorId == null)
                return ValidationResult.ValidationError($"{nameof(request.Body.VendorId)} cannot be null");

            if (string.IsNullOrEmpty(request.Body.VendorAddressCode))
                return ValidationResult.ValidationError($"{nameof(request.Body.VendorAddressCode)} cannot be null");

            if (request.Body.SubtotalAmount < 1)
                return ValidationResult.ValidationError($"{nameof(request.Body.SubtotalAmount)} must greater than 0");

            return ValidationResult.Ok();
        }
    }
}
