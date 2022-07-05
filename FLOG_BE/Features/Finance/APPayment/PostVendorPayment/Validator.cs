using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;


namespace FLOG_BE.Features.Finance.ApPayment.PostVendorPayment
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
            //if (string.IsNullOrEmpty(request.Body.DocumentNo))
            //    return ValidationResult.ValidationError($"{nameof(request.Body.DocumentNo)} cannot be null");

            if (string.IsNullOrEmpty(request.Body.CurrencyCode))
                return ValidationResult.ValidationError($"{nameof(request.Body.CurrencyCode)} cannot be null");

            if (request.Body.VendorId == null)
                return ValidationResult.ValidationError($"{nameof(request.Body.VendorId)} cannot be null");


            return ValidationResult.Ok();
        }
    }
}
