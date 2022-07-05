using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;


namespace FLOG_BE.Features.Sales.NegotiationSheet.PostNegotiationSheet
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

            if (string.IsNullOrEmpty(request.Body.TransactionType.ToString()))
                return ValidationResult.ValidationError($"{nameof(request.Body.TransactionType)} cannot be null");
            if (string.IsNullOrEmpty(request.Body.CustomerId.ToString()))
                return ValidationResult.ValidationError($"{nameof(request.Body.CustomerId)} cannot be null");
            if (string.IsNullOrEmpty(request.Body.SalesCode.ToString()))
                return ValidationResult.ValidationError($"{nameof(request.Body.SalesCode)} cannot be null");
            if (string.IsNullOrEmpty(request.Body.CustomerAddressCode.ToString()))
                return ValidationResult.ValidationError($"{nameof(request.Body.CustomerAddressCode)} cannot be null");
            if (string.IsNullOrEmpty(request.Body.ShippingLineId.ToString()))
                return ValidationResult.ValidationError($"{nameof(request.Body.ShippingLineId)} cannot be null");
            if (string.IsNullOrEmpty(request.Body.PortOfLoading.ToString()))
                return ValidationResult.ValidationError($"{nameof(request.Body.PortOfLoading)} cannot be null");
            if (string.IsNullOrEmpty(request.Body.PortOfDischarge.ToString()))
                return ValidationResult.ValidationError($"{nameof(request.Body.PortOfDischarge)} cannot be null");
            if (string.IsNullOrEmpty(request.Body.TermOfShipment.ToString()))
                return ValidationResult.ValidationError($"{nameof(request.Body.TermOfShipment)} cannot be null");
            if (string.IsNullOrEmpty(request.Body.FinalDestination.ToString()))
                return ValidationResult.ValidationError($"{nameof(request.Body.FinalDestination)} cannot be null");
            if (string.IsNullOrEmpty(request.Body.Commodity.ToString()))
                return ValidationResult.ValidationError($"{nameof(request.Body.Commodity)} cannot be null");
            if (string.IsNullOrEmpty(request.Body.CargoDescription.ToString()))
                return ValidationResult.ValidationError($"{nameof(request.Body.CargoDescription)} cannot be null");

          

            return ValidationResult.Ok();
        }
    }
}
