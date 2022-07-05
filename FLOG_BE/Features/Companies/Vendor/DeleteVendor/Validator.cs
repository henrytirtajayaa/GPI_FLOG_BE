using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Companies.Vendor.DeleteVendor
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

            if (string.IsNullOrEmpty(request.Body.VendorId.ToString()))
                return ValidationResult.ValidationError($"{nameof(request.Body.VendorId)} cannot be null");
            
            return ValidationResult.Ok();
        }
    }
}
