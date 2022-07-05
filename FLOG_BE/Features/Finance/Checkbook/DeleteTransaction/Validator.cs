using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Finance.Checkbook.DeleteTransaction
{
    public class Validator : AsyncRequestValidator<Request>
    {
        public readonly IHttpContextAccessor _httpContextAccessor;

        public Validator(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<ValidationResult> Validate(Request request)
        {
            if (string.IsNullOrEmpty(request.Body.CheckbookTransactionId.ToString()))
                return ValidationResult.ValidationError($"{nameof(request.Body.CheckbookTransactionId)} cannot be null");

            if (!Guid.TryParse(request.Body.CheckbookTransactionId.ToString(), out var newGuid))
                return ValidationResult.ValidationError($"{nameof(request.Body.CheckbookTransactionId)} Incorrect Guid Format");

            return ValidationResult.Ok();
        }
    }
}
