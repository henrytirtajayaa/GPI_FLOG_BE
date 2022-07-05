using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Finance.ARApply.PutApplyReceivable
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
            if (request.Body.ReceivableApplyId == null)
                return ValidationResult.ValidationError($"{nameof(request.Body.ReceivableApplyId)} cannot be null");

            if (request.Body.CustomerId == null)
                return ValidationResult.ValidationError($"{nameof(request.Body.CustomerId)} cannot be null");

            if (request.Body.OriginatingTotalPaid <= 0)
                return ValidationResult.ValidationError($"{nameof(request.Body.OriginatingTotalPaid)} must not 0");

            if (request.Body.ARApplyDetails == null || request.Body.ARApplyDetails.Count <= 0)
                return ValidationResult.ValidationError($"{nameof(request.Body.ARApplyDetails)} must not empty");

            return ValidationResult.Ok();
        }
    }
}
