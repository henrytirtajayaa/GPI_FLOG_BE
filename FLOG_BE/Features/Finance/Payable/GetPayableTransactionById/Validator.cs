using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;

namespace FLOG_BE.Features.Finance.Payable.GetPayableTransactionById
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
            if (request.PayableTransactionId == null || request.PayableTransactionId == Guid.Empty)
                return ValidationResult.ValidationError($"{nameof(request.PayableTransactionId)} cannot be null");

            return ValidationResult.Ok();
        }
    }
}
