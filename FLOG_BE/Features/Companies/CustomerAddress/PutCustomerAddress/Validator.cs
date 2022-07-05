using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Companies.CustomerAddress.PutCustomerAddress
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
            if (string.IsNullOrEmpty(request.Body.AddressCode))
                return ValidationResult.ValidationError($"{nameof(request.Body.AddressCode)} cannot be null");

            if (string.IsNullOrEmpty(request.Body.AddressName))
                return ValidationResult.ValidationError($"{nameof(request.Body.AddressName)} cannot be null");

            if (string.IsNullOrEmpty(request.Body.Address))
                return ValidationResult.ValidationError($"{nameof(request.Body.Address)} cannot be null");

            if ((request.Body.AddressCode).Length > 50)
                return ValidationResult.ValidationError($"{nameof(request.Body.AddressCode)} cannot more than 50 characters");

            if ((request.Body.AddressName).Length > 100)
                return ValidationResult.ValidationError($"{nameof(request.Body.AddressName)} cannot more than 100 characters");

            if ((request.Body.ContactPerson).Length > 100)
                return ValidationResult.ValidationError($"{nameof(request.Body.ContactPerson)} cannot more than 100 characters");

            if ((request.Body.Address).Length > 300)
                return ValidationResult.ValidationError($"{nameof(request.Body.Address)} cannot more than 300 characters");

            if ((request.Body.Handphone).Length > 50)
                return ValidationResult.ValidationError($"{nameof(request.Body.Handphone)} cannot more than 50 characters");

            if ((request.Body.Phone1).Length > 50)
                return ValidationResult.ValidationError($"{nameof(request.Body.Phone1)} cannot more than 50 characters");

            if ((request.Body.Extension1).Length > 10)
                return ValidationResult.ValidationError($"{nameof(request.Body.Extension1)} cannot more than 10 characters");

            if ((request.Body.Phone2).Length > 50)
                return ValidationResult.ValidationError($"{nameof(request.Body.Phone2)} cannot more than 50 characters");

            if ((request.Body.Extension2).Length > 10)
                return ValidationResult.ValidationError($"{nameof(request.Body.Extension2)} cannot more than 10 characters");

            if ((request.Body.Fax).Length > 50)
                return ValidationResult.ValidationError($"{nameof(request.Body.Fax)} cannot more than 50 characters");

            if ((request.Body.EmailAddress).Length > 100)
                return ValidationResult.ValidationError($"{nameof(request.Body.EmailAddress)} cannot more than 100 characters");

            if ((request.Body.HomePage).Length > 200)
                return ValidationResult.ValidationError($"{nameof(request.Body.HomePage)} cannot more than 200 characters");

            if ((request.Body.Neighbourhood).Length > 10)
                return ValidationResult.ValidationError($"{nameof(request.Body.Neighbourhood)} cannot more than 10 characters");

            if ((request.Body.Hamlet).Length > 10)
                return ValidationResult.ValidationError($"{nameof(request.Body.Hamlet)} cannot more than 10 characters");

            if ((request.Body.UrbanVillage).Length > 100)
                return ValidationResult.ValidationError($"{nameof(request.Body.UrbanVillage)} cannot more than 100 characters");

            if ((request.Body.SubDistrict).Length > 100)
                return ValidationResult.ValidationError($"{nameof(request.Body.SubDistrict)} cannot more than 100 characters");

            if ((request.Body.CityCode).Length > 50)
                return ValidationResult.ValidationError($"{nameof(request.Body.CityCode)} cannot more than 50 characters");

            if ((request.Body.PostCode).Length > 10)
                return ValidationResult.ValidationError($"{nameof(request.Body.PostCode)} cannot more than 10 characters");

            if ((request.Body.TaxAddressId).Length > 50)
                return ValidationResult.ValidationError($"{nameof(request.Body.TaxAddressId)} cannot more than 50 characters");

            return ValidationResult.Ok();
        }
    }
}
