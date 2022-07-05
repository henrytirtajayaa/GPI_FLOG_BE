using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;
namespace FLOG_BE.Features.Companies.Uom.PutUomHeader
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
            if (request.Body.UomHeaderId.Equals(Guid.Empty))
            {
                return ValidationResult.ValidationError($"Incorrect format {nameof(request.Body.UomHeaderId)}");
            }

            if (string.IsNullOrEmpty(request.Body.UomScheduleCode))
                return ValidationResult.ValidationError($"{nameof(request.Body.UomScheduleCode)} cannot be null");
                        
            return ValidationResult.Ok();
        }
    }
}
