using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;


namespace FLOG_BE.Features.Finance.BankReconcile.PutBankReconcile
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
            if (request.Body.BankReconcileId == null || request.Body.BankReconcileId == Guid.Empty)
                return ValidationResult.ValidationError($"{nameof(request.Body.BankReconcileId)} cannot be null");

            if (request.Body.CheckbookCode == null)
                return ValidationResult.ValidationError($"{nameof(request.Body.CheckbookCode)} cannot be null");

            if (request.Body.BankCutoffStart == null)
                return ValidationResult.ValidationError($"{nameof(request.Body.BankCutoffStart)} cannot be null");

            if (request.Body.BankCutoffEnd == null)
                return ValidationResult.ValidationError($"{nameof(request.Body.BankCutoffEnd)} cannot be null");

            if (request.Body.BankEndingOrgBalance == 0)
                return ValidationResult.ValidationError($"{nameof(request.Body.BankEndingOrgBalance)} must not 0");

            if (request.Body.CheckbookEndingOrgBalance == 0)
                return ValidationResult.ValidationError($"{nameof(request.Body.CheckbookEndingOrgBalance)} must not 0");

            if (request.Body.ReconcileDetails == null)
                return ValidationResult.ValidationError($"{nameof(request.Body.ReconcileDetails)} must not empty");

            return ValidationResult.Ok();
        }
    }
}
