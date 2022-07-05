using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Finance.Checkbook.PutStatusApproval
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

            if (string.IsNullOrEmpty(request.Body.PersonId.ToString()))
                return ValidationResult.ValidationError($"{nameof(request.Body.PersonId)} cannot be null");

            if (request.Body.ActionDocStatus <= 0)
                return ValidationResult.ValidationError($"{nameof(request.Body.ActionDocStatus)} invalid ");

            return ValidationResult.Ok();
        }
    }
}
