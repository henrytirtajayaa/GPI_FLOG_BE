using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Companies.ApprovalSetup.PutApprovalSetup
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
            if (string.IsNullOrEmpty(request.Body.ApprovalCode))
                return ValidationResult.ValidationError($"{nameof(request.Body.ApprovalCode)} cannot be null");

            if ((request.Body.Description).Length > 200)
                return ValidationResult.ValidationError($"{nameof(request.Body.Description)} cannot more than 200 characters");

            if (request.Body.Status == null)
                return ValidationResult.ValidationError($"{nameof(request.Body.Status)} cannot be null");

            return ValidationResult.Ok();
        }
    }
}
