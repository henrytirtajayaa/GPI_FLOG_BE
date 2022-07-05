using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Finance.APApply.PutApplyPayable
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
            if (request.Body.PayableApplyId == null)
                return ValidationResult.ValidationError($"{nameof(request.Body.PayableApplyId)} cannot be null");

            if (request.Body.VendorId == null)
                return ValidationResult.ValidationError($"{nameof(request.Body.VendorId)} cannot be null");

            if (request.Body.OriginatingTotalPaid <= 0)
                return ValidationResult.ValidationError($"{nameof(request.Body.OriginatingTotalPaid)} must not 0");

            if (request.Body.APApplyDetails == null || request.Body.APApplyDetails.Count <= 0)
                return ValidationResult.ValidationError($"{nameof(request.Body.APApplyDetails)} must not empty");

            return ValidationResult.Ok();
        }
    }
}
