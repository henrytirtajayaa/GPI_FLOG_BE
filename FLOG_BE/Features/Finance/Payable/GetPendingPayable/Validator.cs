using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;

namespace FLOG_BE.Features.Finance.Payable.GetPendingPayable
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
            if (request.Filter.VendorId == null && request.Filter.VendorId != Guid.Empty)
                return ValidationResult.ValidationError($"{nameof(request.Filter.VendorId)} cannot be null");

            return ValidationResult.Ok();
        }
    }
}
