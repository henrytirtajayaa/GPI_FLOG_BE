using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;

namespace FLOG_BE.Features.Companies.AccountSegment.PostAccountSegment
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
            var CountRequest = request.Body.Count();

            if (CountRequest > 10)
                return ValidationResult.ValidationError("Max 10 Records");

            foreach (var item in request.Body)
            {
                if (item.SegmentNo == null)
                    return ValidationResult.ValidationError($"{nameof(item.SegmentNo)} cannot be null");

                if (string.IsNullOrEmpty(item.Description))
                    return ValidationResult.ValidationError($"{nameof(item.Description)} cannot be null");

                if (item.Length == null)
                    return ValidationResult.ValidationError($"{nameof(item.Length)} cannot be null");

                if (item.Length < 1)
                    return ValidationResult.ValidationError($"{nameof(item.Length)} must larger than 1");
            }
            return ValidationResult.Ok();
        }
    }
}
