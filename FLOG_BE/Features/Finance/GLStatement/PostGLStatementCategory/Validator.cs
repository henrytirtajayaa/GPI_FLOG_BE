using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;


namespace FLOG_BE.Features.Finance.GLStatement.PostGLStatementCategory
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
            if (request.Body.StatementType <= 0)
                return ValidationResult.ValidationError($"{nameof(request.Body.StatementType)} must not null");

            if (request.Body.CategoryKey == null)
                return ValidationResult.ValidationError($"{nameof(request.Body.CategoryKey)} cannot be null");

            if (request.Body.CategoryCaption == null)
                return ValidationResult.ValidationError($"{nameof(request.Body.CategoryCaption)} cannot be null");

            return ValidationResult.Ok();
        }
    }
}
