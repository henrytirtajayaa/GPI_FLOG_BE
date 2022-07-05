using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Finance.JournalEntry.DeleteJournalEntry
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

            if (string.IsNullOrEmpty(request.Body.JournalEntryHeaderId.ToString()))
                return ValidationResult.ValidationError($"{nameof(request.Body.JournalEntryHeaderId)} cannot be null");

            if (!Guid.TryParse(request.Body.JournalEntryHeaderId.ToString(), out var newGuid))
                return ValidationResult.ValidationError($"{nameof(request.Body.JournalEntryHeaderId)} Incorrect Guid Format");
            
            return ValidationResult.Ok();
        }
    }
}
