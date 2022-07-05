using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Companies.Uom.PostUomHeader
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
            if (string.IsNullOrEmpty(request.Body.UomScheduleCode))
                return ValidationResult.ValidationError($"{nameof(request.Body.UomScheduleCode)} cannot be null");
            
            if (request.Body.UomBaseId == null)
                return ValidationResult.ValidationError($"{nameof(request.Body.UomBaseId)} must not empty");
            
            return ValidationResult.Ok();
        }
    }
}
