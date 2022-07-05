using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Companies.FiscalPeriodDetail.PostFiscalPeriodDetail
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
            foreach (var item in request.Body)
            {
                if (string.IsNullOrEmpty(item.FiscalHeaderId.ToString()))
                    return ValidationResult.ValidationError($"{nameof(item.FiscalHeaderId)} cannot be null");

                if (item.PeriodIndex < 0)
                    return ValidationResult.ValidationError($"{nameof(item.PeriodIndex)} cannot be null");

                if (string.IsNullOrEmpty(item.PeriodStart.ToString()))
                    return ValidationResult.ValidationError($"{nameof(item.PeriodStart)} cannot be null");

                if (string.IsNullOrEmpty(item.PeriodEnd.ToString()))
                    return ValidationResult.ValidationError($"{nameof(item.PeriodEnd)} cannot be null");

                if (string.IsNullOrEmpty(item.IsClosePurchasing.ToString()))
                    return ValidationResult.ValidationError($"{nameof(item.IsClosePurchasing)} cannot be null");

                if (string.IsNullOrEmpty(item.IsCloseSales.ToString()))
                    return ValidationResult.ValidationError($"{nameof(item.IsCloseSales)} cannot be null");

                if (string.IsNullOrEmpty(item.IsCloseInventory.ToString()))
                    return ValidationResult.ValidationError($"{nameof(item.IsCloseInventory)} cannot be null");

                if (string.IsNullOrEmpty(item.IsCloseFinancial.ToString()))
                    return ValidationResult.ValidationError($"{nameof(item.IsCloseFinancial)} cannot be null");

                if (string.IsNullOrEmpty(item.IsCloseAsset.ToString()))
                    return ValidationResult.ValidationError($"{nameof(item.IsCloseAsset)} cannot be null");
            }
            return ValidationResult.Ok();
        }
    }
}
