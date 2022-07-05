using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;


namespace FLOG_BE.Features.Finance.ArReceipt.PutStatusCustomerReceipt
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
            if (string.IsNullOrEmpty(request.Body.ReceiptHeaderId.ToString()))
                return ValidationResult.ValidationError($"{nameof(request.Body.ReceiptHeaderId)} cannot be null");

            if (request.Body.Status <= 0)
                return ValidationResult.ValidationError($"{nameof(request.Body.Status)} invalid ");

            return ValidationResult.Ok();
        }
    }
}
