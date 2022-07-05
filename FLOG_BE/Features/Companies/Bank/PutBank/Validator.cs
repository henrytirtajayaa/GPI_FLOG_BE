using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Companies.Bank.PutBank
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

            if (!Guid.TryParse(request.Body.BankId.ToString(), out var newGuid))
                return ValidationResult.ValidationError($"{nameof(request.Body.BankId)} Incorrect Guid Format");

            if (string.IsNullOrEmpty(request.Body.BankCode))
                return ValidationResult.ValidationError($"{nameof(request.Body.BankCode)} cannot be null");

            if ((request.Body.BankName).Length > 100)
                return ValidationResult.ValidationError($"{nameof(request.Body.BankName)} cannot more than 100 characters");

            return ValidationResult.Ok();
        }
    }
}
