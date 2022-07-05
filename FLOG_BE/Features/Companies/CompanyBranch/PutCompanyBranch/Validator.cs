using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;
namespace FLOG_BE.Features.Companies.CompanyBranch.PutCompanyBranch
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
            if (string.IsNullOrEmpty(request.Body.CompanyBranchId.ToString()))
                return ValidationResult.ValidationError($"{nameof(request.Body.CompanyBranchId)} cannot be null");
            if (string.IsNullOrEmpty(request.Body.BranchCode))
                return ValidationResult.ValidationError($"{nameof(request.Body.BranchCode)} cannot be null");
            if (string.IsNullOrEmpty(request.Body.BranchName))
                return ValidationResult.ValidationError($"{nameof(request.Body.BranchName)} cannot be null");
            
            if ((request.Body.BranchCode).Length > 50)
                return ValidationResult.ValidationError($"{nameof(request.Body.BranchCode)} cannot more than 50 characters");
            if ((request.Body.BranchName).Length > 250)
                return ValidationResult.ValidationError($"{nameof(request.Body.BranchName)} cannot more than 100 characters");

            return ValidationResult.Ok();
        }
    }
}
