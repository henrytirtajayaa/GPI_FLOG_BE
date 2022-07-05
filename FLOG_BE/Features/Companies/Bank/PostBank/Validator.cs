using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Companies.Bank.PostBank
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
            if (string.IsNullOrEmpty(request.Body.BankCode))
                return ValidationResult.ValidationError($"{nameof(request.Body.BankCode)} cannot be null");

            if ((request.Body.BankName).Length > 100)
                return ValidationResult.ValidationError($"{nameof(request.Body.BankName)} cannot more than 100 characters");

            //^([a-zA-Z0-9]+|[a-zA-Z0-9]+\s[a-zA-Z0-9]+)$
            if (!Regex.Match(request.Body.BankName, @"^[a-zA-Z_ ][a-zA-Z0-9_ ]*$").Success)
                return ValidationResult.ValidationError($"{nameof(request.Body.BankName)} cannot contains special charaters");

            return ValidationResult.Ok();
        }
    }
}
