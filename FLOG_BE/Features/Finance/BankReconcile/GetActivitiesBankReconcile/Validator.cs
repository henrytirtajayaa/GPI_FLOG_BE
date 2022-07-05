using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;

namespace FLOG_BE.Features.Finance.BankReconcile.GetActivitiesBankReconcile
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
            if (request.Filter.CheckbookCode == null)
                return ValidationResult.ValidationError($"{nameof(request.Filter.CheckbookCode)} cannot be null");

            if (request.Filter.BankCutoffEnd == null)
                return ValidationResult.ValidationError($"{nameof(request.Filter.BankCutoffEnd)} cannot be null");

            return ValidationResult.Ok();
        }
    }
}
