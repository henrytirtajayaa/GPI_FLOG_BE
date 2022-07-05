using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;

namespace FLOG_BE.Features.Finance.DepositSettlement.GetUnapplyDeposit
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
            if (request.Filter.CustomerId == null && request.Filter.CustomerId != Guid.Empty)
                return ValidationResult.ValidationError($"{nameof(request.Filter.CustomerId)} cannot be null");

            return ValidationResult.Ok();
        }
    }
}
