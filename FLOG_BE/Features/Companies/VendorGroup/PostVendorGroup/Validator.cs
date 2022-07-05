using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Companies.VendorGroup.PostVendorGroup
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
            if (string.IsNullOrEmpty(request.Body.VendorGroupCode))
                return ValidationResult.ValidationError($"{nameof(request.Body.VendorGroupCode)} cannot be null");

            if (string.IsNullOrEmpty(request.Body.VendorGroupName))
                return ValidationResult.ValidationError($"{nameof(request.Body.VendorGroupName)} cannot be null");

            if ((request.Body.VendorGroupCode).Length > 20)
                return ValidationResult.ValidationError($"{nameof(request.Body.VendorGroupCode)} cannot more than 20 characters");

            if ((request.Body.VendorGroupName).Length > 200)
                return ValidationResult.ValidationError($"{nameof(request.Body.VendorGroupName)} cannot more than 200 characters");

            if ((request.Body.PaymentTermCode).Length > 50)
                return ValidationResult.ValidationError($"{nameof(request.Body.PaymentTermCode)} cannot more than 50 characters");

            if ((request.Body.PayableAccountNo).Length > 50)
                return ValidationResult.ValidationError($"{nameof(request.Body.PayableAccountNo)} cannot more than 50 characters");

            if ((request.Body.AccruedPayableAccountNo).Length > 50)
                return ValidationResult.ValidationError($"{nameof(request.Body.AccruedPayableAccountNo)} cannot more than 50 characters");

            return ValidationResult.Ok();
        }
    }
}
