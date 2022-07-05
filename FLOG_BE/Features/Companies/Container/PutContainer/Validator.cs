using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Companies.Container.PutContainer
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
            if (string.IsNullOrEmpty(request.Body.Containerid.ToString()))
                return ValidationResult.ValidationError($"{nameof(request.Body.Containerid)} cannot be null");

            if (string.IsNullOrEmpty(request.Body.ContainerName))
                return ValidationResult.ValidationError($"{nameof(request.Body.ContainerName)} cannot be null");

            if (string.IsNullOrEmpty(request.Body.ContainerType))
                return ValidationResult.ValidationError($"{nameof(request.Body.ContainerType)} cannot be null");

            if (request.Body.ContainerSize <= 0)
                return ValidationResult.ValidationError($"{nameof(request.Body.ContainerSize)} value must > 0");

            if (request.Body.ContainerTeus <= 0)
                return ValidationResult.ValidationError($"{nameof(request.Body.ContainerTeus)} value must > 0");


            return ValidationResult.Ok();
        }
    }
}
