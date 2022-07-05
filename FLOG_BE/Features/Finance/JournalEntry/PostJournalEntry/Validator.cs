using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Finance.JournalEntry.PostJournalEntry
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
            if (string.IsNullOrEmpty(request.Body.BranchCode))
                return ValidationResult.ValidationError($"{nameof(request.Body.BranchCode)} cannot be null");

            if (request.Body.TransactionDate == null) 
                return ValidationResult.ValidationError($"{nameof(request.Body.TransactionDate)} cannot be null");

            if (string.IsNullOrEmpty(request.Body.CurrencyCode))
                return ValidationResult.ValidationError($"{nameof(request.Body.CurrencyCode)} cannot be null");

            if (request.Body.ExchangeRate < 1)
                return ValidationResult.ValidationError($"{nameof(request.Body.ExchangeRate)} must greater than 0");

            if (request.Body.OriginatingTotal < 1)
                return ValidationResult.ValidationError($"{nameof(request.Body.OriginatingTotal)} must greater than 0");

            if (request.Body.FunctionalTotal < 1)
                return ValidationResult.ValidationError($"{nameof(request.Body.FunctionalTotal)} must greater than 0");

            if (request.Body.RequestEntryDetails.Count < 0)
                return ValidationResult.ValidationError($"{nameof(request.Body.RequestEntryDetails)} must not empty");

            return ValidationResult.Ok();
        }
    }
}
