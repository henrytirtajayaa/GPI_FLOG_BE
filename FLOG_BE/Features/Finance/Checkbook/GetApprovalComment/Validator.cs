using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;

namespace FLOG_BE.Features.Finance.Checkbook.GetApprovalComment
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
            if (string.IsNullOrEmpty(request.Filter.CheckbookTransactionId.ToString()))
                return ValidationResult.ValidationError($"{nameof(request.Filter.CheckbookTransactionId)} cannot be null");

            if(request.Filter.CheckbookTransactionId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                return ValidationResult.ValidationError($"{nameof(request.Filter.CheckbookTransactionId)} cannot be null");

            return ValidationResult.Ok();
        }
    }
}
