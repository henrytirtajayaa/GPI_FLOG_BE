using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Companies.CustomerGroup.PutCustomerGroup
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
            if(request.Body.CustomerGroupId == Guid.Empty)
                return ValidationResult.ValidationError($"{nameof(request.Body.CustomerGroupId)} cannot be null");

            if (string.IsNullOrEmpty(request.Body.CustomerGroupCode))
                return ValidationResult.ValidationError($"{nameof(request.Body.CustomerGroupCode)} cannot be null");

            if (request.Body.CustomerGroupCode.Length > 20)
                return ValidationResult.ValidationError($"{nameof(request.Body.CustomerGroupCode)} must not over 20 characters");

            if (string.IsNullOrEmpty(request.Body.CustomerGroupName))
                return ValidationResult.ValidationError($"{nameof(request.Body.CustomerGroupName)} cannot be null");

            return ValidationResult.Ok();
        }
    }
}
