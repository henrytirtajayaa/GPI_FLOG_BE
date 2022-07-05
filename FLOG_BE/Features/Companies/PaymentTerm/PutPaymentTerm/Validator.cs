using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Companies.PaymentTerm.PutPaymentTerm
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
            if (string.IsNullOrEmpty(request.Body.PaymentTermCode))
                return ValidationResult.ValidationError($"{nameof(request.Body.PaymentTermCode)} cannot be null");

            if (request.Body.Due.Value < 0)
                return ValidationResult.ValidationError($"{nameof(request.Body.Due)} cannot be less than 0");

            if (request.Body.Unit.Value < 0)
                return ValidationResult.ValidationError($"{nameof(request.Body.Unit)} cannot be less than 0");


            if ((request.Body.PaymentTermCode).Length > 50)
                return ValidationResult.ValidationError($"{nameof(request.Body.PaymentTermCode)} cannot more than 50 characters");

             if ((request.Body.PaymentTermDesc).Length > 100)
                return ValidationResult.ValidationError($"{nameof(request.Body.PaymentTermDesc)} cannot more than 100 characters");




            return ValidationResult.Ok();
        }
    }
}
