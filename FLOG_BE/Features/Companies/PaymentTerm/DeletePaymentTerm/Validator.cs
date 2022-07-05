using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Companies.PaymentTerm.DeletePaymentTerm
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
            if (string.IsNullOrEmpty(request.Body.PaymentTermId))
                return ValidationResult.ValidationError($"{nameof(request.Body.PaymentTermId)} cannot be null");

            return ValidationResult.Ok();
        }
    }
}
