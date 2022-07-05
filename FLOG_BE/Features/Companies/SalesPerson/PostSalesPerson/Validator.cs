using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Companies.SalesPerson.PostSalesPerson
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
            if (string.IsNullOrEmpty(request.Body.SalesCode))
                return ValidationResult.ValidationError($"{nameof(request.Body.SalesCode)} cannot be null");

            if (string.IsNullOrEmpty(request.Body.SalesName))
                return ValidationResult.ValidationError($"{nameof(request.Body.SalesName)} cannot be null");

            return ValidationResult.Ok();
        }
    }
}
