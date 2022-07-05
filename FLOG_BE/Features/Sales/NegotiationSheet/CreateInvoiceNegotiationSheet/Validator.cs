using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Sales.NegotiationSheet.CreateInvoiceNegotiationSheet
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
            if (string.IsNullOrEmpty(request.Body.NegotiationSheetId.ToString()))
                return ValidationResult.ValidationError($"{nameof(request.Body.NegotiationSheetId)} cannot be null");

            if (request.Body.InvoiceDate == null)
                return ValidationResult.ValidationError($"{nameof(request.Body.InvoiceDate)} cannot be null");

            return ValidationResult.Ok();
        }
    }
}
