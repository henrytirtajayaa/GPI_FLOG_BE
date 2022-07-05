using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;

namespace FLOG_BE.Features.Finance.DistributionJournal.GetDistributionByDocNo
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
            if (string.IsNullOrEmpty(request.Filter.DocumentNo) && request.Filter.TransactionId == Guid.Empty)
                return ValidationResult.ValidationError($"Document No or Transaction Id cannot be null");

            return ValidationResult.Ok();
        }
    }
}
