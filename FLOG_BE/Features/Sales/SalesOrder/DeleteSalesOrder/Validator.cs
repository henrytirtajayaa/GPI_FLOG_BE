using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Sales.SalesOrder.DeleteSalesOrder
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
            if (string.IsNullOrEmpty(request.Body.SalesOrderId.ToString()))
                return ValidationResult.ValidationError($"{nameof(request.Body.SalesOrderId)} cannot be null");

            if (!Guid.TryParse(request.Body.SalesOrderId.ToString(), out var newGuid))
                return ValidationResult.ValidationError($"{nameof(request.Body.SalesOrderId)} Incorrect Guid Format");

            return ValidationResult.Ok();
        }
    }
}
