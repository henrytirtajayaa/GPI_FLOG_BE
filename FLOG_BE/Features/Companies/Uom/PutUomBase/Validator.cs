using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;
namespace FLOG_BE.Features.Companies.Uom.PutUomBase
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
            if (request.Body.UomBaseId.Equals(Guid.Empty))
            {
                return ValidationResult.ValidationError($"Incorrect format {nameof(request.Body.UomBaseId)}");
            }

            if (string.IsNullOrEmpty(request.Body.UomCode))
                return ValidationResult.ValidationError($"{nameof(request.Body.UomCode)} cannot be null");
            if (string.IsNullOrEmpty(request.Body.UomName))
                return ValidationResult.ValidationError($"{nameof(request.Body.UomName)} cannot be null");
                        
            return ValidationResult.Ok();
        }
    }
}
