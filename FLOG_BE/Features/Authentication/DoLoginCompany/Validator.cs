using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model;
using Infrastructure.Authentication;

namespace FLOG_BE.Features.Authentication.DoLoginCompany
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
            //check jwt token

            var claims = _httpContextAccessor.HttpContext.User.Claims.ToList(); ;
            if (claims.Any(x => x.Type == "ID" && x.Value.Equals(request.UserId, StringComparison.OrdinalIgnoreCase)))
            {
                return ValidationResult.Unauthorized();
            }

            return ValidationResult.Ok();
        }
    }
}
