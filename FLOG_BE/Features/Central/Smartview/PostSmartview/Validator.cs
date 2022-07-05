using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Central.Smartview.PostSmartview
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
            if (string.IsNullOrEmpty(request.Body.SmartTitle))
                return ValidationResult.ValidationError($"{nameof(request.Body.SmartTitle)} cannot be null");

            if ((request.Body.SmartTitle).Length > 250)
                return ValidationResult.ValidationError($"{nameof(request.Body.SmartTitle)} cannot more than 250 characters");
            
            if (string.IsNullOrEmpty(request.Body.SqlViewName))
                return ValidationResult.ValidationError($"{nameof(request.Body.SqlViewName)} cannot be null");

            if ((request.Body.SqlViewName).Length > 250)
                return ValidationResult.ValidationError($"{nameof(request.Body.SqlViewName)} cannot more than 250 characters");

            if (string.IsNullOrEmpty(request.Body.GroupName))
                return ValidationResult.ValidationError($"{nameof(request.Body.GroupName)} cannot be null");

           
            return ValidationResult.Ok();
        }
    }
}
