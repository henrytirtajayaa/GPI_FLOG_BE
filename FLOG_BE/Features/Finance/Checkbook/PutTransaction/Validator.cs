using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Finance.Checkbook.PutTransaction
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

            if (string.IsNullOrEmpty(request.Body.CheckbookTransactionId.ToString()))
                return ValidationResult.ValidationError($"{nameof(request.Body.CheckbookTransactionId)} cannot be null");

            if (string.IsNullOrEmpty(request.Body.DocumentType))
                return ValidationResult.ValidationError($"{nameof(request.Body.DocumentType)} cannot be null");

            if (string.IsNullOrEmpty(request.Body.CurrencyCode))
                return ValidationResult.ValidationError($"{nameof(request.Body.CurrencyCode)} cannot be null");

            if (request.Body.CheckbookCode == null)
                return ValidationResult.ValidationError($"{nameof(request.Body.CheckbookCode)} cannot be null");

            if (string.IsNullOrEmpty(request.Body.PaidSubject))
                return ValidationResult.ValidationError($"{nameof(request.Body.PaidSubject)} cannot be null");

            if (request.Body.OriginatingTotalAmount < 1)
                return ValidationResult.ValidationError($"{nameof(request.Body.OriginatingTotalAmount)} must greater than 0");

            if (request.Body.FunctionalTotalAmount < 1)
                return ValidationResult.ValidationError($"{nameof(request.Body.FunctionalTotalAmount)} must greater than 0");


            return ValidationResult.Ok();
        }
    }
}
