using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;

namespace FLOG_BE.Features.Finance.BankReconcile.GetPrevBankReconcile
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
            if (string.IsNullOrEmpty(request.Filter.CheckbookCode))
                return ValidationResult.ValidationError($"{nameof(request.Filter.CheckbookCode)} cannot be null");

            if (request.Filter.BankCutoffStart == null)
                return ValidationResult.ValidationError($"{nameof(request.Filter.BankCutoffStart)} cannot be null");

            return ValidationResult.Ok();
        }
    }
}
