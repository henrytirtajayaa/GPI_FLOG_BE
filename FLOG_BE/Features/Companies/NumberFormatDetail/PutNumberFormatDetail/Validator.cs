using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Companies.NumberFormatDetail.PutNumberFormatDetail
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
            if (string.IsNullOrEmpty(request.Body.FormatHeaderId))
                return ValidationResult.ValidationError($"{nameof(request.Body.FormatHeaderId)} cannot be null");

            if ((request.Body.MaskFormat).Length > 50)
                return ValidationResult.ValidationError($"{nameof(request.Body.MaskFormat)} cannot more than 50 characters");

            return ValidationResult.Ok();
        }
    }
}
