using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;
namespace FLOG_BE.Features.Companies.City.PutCity
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
            if (string.IsNullOrEmpty(request.Body.CityId))
                return ValidationResult.ValidationError($"{nameof(request.Body.CityId)} cannot be null");
            if (string.IsNullOrEmpty(request.Body.CityCode))
                return ValidationResult.ValidationError($"{nameof(request.Body.CityCode)} cannot be null");
            if (string.IsNullOrEmpty(request.Body.CityName))
                return ValidationResult.ValidationError($"{nameof(request.Body.CityName)} cannot be null");
            if (string.IsNullOrEmpty(request.Body.Province))
                return ValidationResult.ValidationError($"{nameof(request.Body.Province)} cannot be null");
            if (string.IsNullOrEmpty(request.Body.CountryId.ToString()))
                return ValidationResult.ValidationError($"{nameof(request.Body.CountryId)} cannot be null");

            if (!Guid.TryParse(request.Body.CityId, out Guid guid))
            {
                return ValidationResult.ValidationError($"Incorrect format {nameof(request.Body.CityId)}");
            }
            
            if ((request.Body.CityCode).Length > 50)
                return ValidationResult.ValidationError($"{nameof(request.Body.CityCode)} cannot more than 50 characters");
            if ((request.Body.CityName).Length > 100)
                return ValidationResult.ValidationError($"{nameof(request.Body.CityName)} cannot more than 100 characters");
            if ((request.Body.Province).Length > 100)
                return ValidationResult.ValidationError($"{nameof(request.Body.Province)} cannot more than 100 characters");

            return ValidationResult.Ok();
        }
    }
}
