using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Companies.Account.PostAccount
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
            if (string.IsNullOrEmpty(request.Body.AccountId))
                return ValidationResult.ValidationError($"{nameof(request.Body.AccountId)} cannot be null");

            if (request.Body.PostingType == null)
                return ValidationResult.ValidationError($"{nameof(request.Body.PostingType)} cannot be null");
            if (request.Body.NormalBalance == null)
                return ValidationResult.ValidationError($"{nameof(request.Body.NormalBalance)} cannot be null");



            return ValidationResult.Ok();
        }
    }
}
