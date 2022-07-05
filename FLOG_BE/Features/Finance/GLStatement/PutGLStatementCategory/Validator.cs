using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;


namespace FLOG_BE.Features.Finance.GLStatement.PutGLStatementCategory
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
            if (request.Body.CategoryId == null || request.Body.CategoryId <= 0)
                return ValidationResult.ValidationError($"{nameof(request.Body.CategoryId)} cannot be null");

            if (request.Body.CategoryKey == null)
                return ValidationResult.ValidationError($"{nameof(request.Body.CategoryKey)} cannot be null");

            if (request.Body.CategoryCaption == null)
                return ValidationResult.ValidationError($"{nameof(request.Body.CategoryCaption)} cannot be null");

            if (request.Body.StatementType == null)
                return ValidationResult.ValidationError($"{nameof(request.Body.StatementType)} cannot be null");

            return ValidationResult.Ok();
        }
    }
}
