using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Companies.Customer.PutCustomer
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
            if(request.Body.CustomerId == Guid.Empty)
                return ValidationResult.ValidationError($"{nameof(request.Body.CustomerId)} cannot be null");

            if (string.IsNullOrEmpty(request.Body.CustomerCode))
                return ValidationResult.ValidationError($"{nameof(request.Body.CustomerCode)} cannot be null");

            if (request.Body.CustomerCode.Length > 50)
                return ValidationResult.ValidationError($"{nameof(request.Body.CustomerCode)} must not over 50 characters");

            if (string.IsNullOrEmpty(request.Body.CustomerName))
                return ValidationResult.ValidationError($"{nameof(request.Body.CustomerName)} cannot be null");

      

            return ValidationResult.Ok();
        }
    }
}
