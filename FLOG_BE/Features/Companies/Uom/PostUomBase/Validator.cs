using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Companies.Uom.PostUomBase
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
            if (string.IsNullOrEmpty(request.Body.UomCode))
                return ValidationResult.ValidationError($"{nameof(request.Body.UomCode)} cannot be null");
            if (string.IsNullOrEmpty(request.Body.UomName))
                return ValidationResult.ValidationError($"{nameof(request.Body.UomName)} cannot be null");
            if ((request.Body.UomCode).Length > 50)
                return ValidationResult.ValidationError($"{nameof(request.Body.UomCode)} cannot more than 50 characters");
            if ((request.Body.UomName).Length > 250)
                return ValidationResult.ValidationError($"{nameof(request.Body.UomName)} cannot more than 250 characters");

            return ValidationResult.Ok();
        }
    }
}
