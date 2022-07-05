using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Companies.NumberFormatHeader.DeleteNumberFormatHeader
{
    public class Validator : AsyncRequestValidator<Request>
    {
        public readonly IHttpContextAccessor _httpContextAccessor;

        public Validator(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<ValidationResult> Validate(Request request)
        {
            if (string.IsNullOrEmpty(request.Body.FormatHeaderId.ToString()))
                return ValidationResult.ValidationError($"{nameof(request.Body.FormatHeaderId)} cannot be null");

            if (!Guid.TryParse(request.Body.FormatHeaderId.ToString(), out var newGuid))
                return ValidationResult.ValidationError($"{nameof(request.Body.FormatHeaderId)} Incorrect Guid Format");

            return ValidationResult.Ok();
        }
    }
}
