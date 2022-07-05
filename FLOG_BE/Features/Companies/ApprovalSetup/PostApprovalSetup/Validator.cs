using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Companies.ApprovalSetup.PostApprovalSetup
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

            if ((request.Body.ApprovalCode).Length > 50)
                return ValidationResult.ValidationError($"{nameof(request.Body.ApprovalCode)} cannot more than 50 characters");

            if ((request.Body.Description).Length > 200)
                return ValidationResult.ValidationError($"{nameof(request.Body.Description)} cannot more than 200 characters");

            return ValidationResult.Ok();
        }
    }
}
