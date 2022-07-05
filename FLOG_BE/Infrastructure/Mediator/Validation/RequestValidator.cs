using System;
using System.Threading.Tasks;
using FluentValidation;

namespace Infrastructure.Mediator
{
    public abstract class RequestValidator<TRequest> : AbstractValidator<TRequest>, IRequestValidator<TRequest>
    {
        public virtual int Order => 1;

        protected virtual ValidationResult HasClaim(TRequest request)
        {
            return null;
        }

        protected new virtual ValidationResult Validate(TRequest request)
        {
            return ValidationResult.Ok();
        }

        public Task<ValidationResult> InternalValidate(TRequest request)
        {
            var result = HasClaim(request) ??
                         FluentValidate(request) ??
                         Validate(request);
            return Task.FromResult(result);
        }

        private ValidationResult FluentValidate(TRequest request)
        {
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;
            ValidatorOptions.DisplayNameResolver = (type, memberInfo, expression) => memberInfo.Name;

            var results = base.Validate(request);
            if (results.IsValid) return null;

            foreach (var failure in results.Errors)
            {
                if (!(failure.CustomState is ValidationResult msg)) continue;

                if (string.IsNullOrWhiteSpace(msg.ErrorDescription)) msg.ErrorDescription = failure.ErrorMessage;
                return msg;
            }
            return null;
        }
    }
}
