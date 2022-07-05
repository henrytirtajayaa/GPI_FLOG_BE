using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Companies.MSDepartment.PutMsDepartment
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
            if (string.IsNullOrEmpty(request.Body.DepartmentCode))
                return ValidationResult.ValidationError($"{nameof(request.Body.DepartmentCode)} cannot be null");

            if (string.IsNullOrEmpty(request.Body.DepartmentName))
                return ValidationResult.ValidationError($"{nameof(request.Body.DepartmentName)} cannot be null");

            return ValidationResult.Ok();
        }
    }
}
