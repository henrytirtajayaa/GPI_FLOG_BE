using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Sales.Quotation.DeleteQuotation
{
    public class Validator : AsyncRequestValidator<Request>
    {
        public readonly IHttpContextAccessor _httpContextAccessor;

        public Validator(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<ValidationResult> Validate(Request request)
        {
            if (string.IsNullOrEmpty(request.Body.SalesQuotationId.ToString()))
                return ValidationResult.ValidationError($"{nameof(request.Body.SalesQuotationId)} cannot be null");

            if (!Guid.TryParse(request.Body.SalesQuotationId.ToString(), out var newGuid))
                return ValidationResult.ValidationError($"{nameof(request.Body.SalesQuotationId)} Incorrect Guid Format");

            return ValidationResult.Ok();
        }
    }
}
