using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Companies.Checkbook.DeleteCheckbook
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
            if (string.IsNullOrEmpty(request.Body.CheckbookId.ToString()))
                return ValidationResult.ValidationError($"{nameof(request.Body.CheckbookId)} cannot be null");

            //if (!Guid.TryParse(request.Body.CheckbookId.ToString(), out var newGuid))
            //    return ValidationResult.ValidationError($"{nameof(request.Body.CheckbookId)} Incorrect Guid Format");

            return ValidationResult.Ok();
        }
    }
}
