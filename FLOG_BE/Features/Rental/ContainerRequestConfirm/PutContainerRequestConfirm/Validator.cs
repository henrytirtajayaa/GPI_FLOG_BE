using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using System.Text.RegularExpressions;

namespace FLOG_BE.Features.Rental.ContainerRequestConfirm.PutContainerRequestConfirm
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
            if (request.Body.ExpiredDate <= request.Body.IssueDate)
                return ValidationResult.ValidationError($"{nameof(request.Body.ExpiredDate)} must greater than {nameof(request.Body.IssueDate)}");

            return ValidationResult.Ok();
        }
    }
}
