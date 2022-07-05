using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Companies.CustomerVendorRelation.PostCustomerVendorRelation
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
            if (request.Body.CustomerId == null)
                return ValidationResult.ValidationError($"{nameof(request.Body.CustomerId)} cannot be null");

            if (request.Body.VendorId == null )
                return ValidationResult.ValidationError($"{nameof(request.Body.VendorId)} cannot be null");

            return ValidationResult.Ok();
        }
    }
}
