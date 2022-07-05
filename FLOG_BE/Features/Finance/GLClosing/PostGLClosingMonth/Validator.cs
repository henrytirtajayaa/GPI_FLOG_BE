using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;


namespace FLOG_BE.Features.Finance.GLClosing.PostGLClosingMonth
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
            if (request.Body.PeriodYear <= 0)
                return ValidationResult.ValidationError($"{nameof(request.Body.PeriodYear)} must not null");

            if (request.Body.PeriodIndex == null)
                return ValidationResult.ValidationError($"{nameof(request.Body.PeriodIndex)} must not null");

            return ValidationResult.Ok();
        }
    }
}
