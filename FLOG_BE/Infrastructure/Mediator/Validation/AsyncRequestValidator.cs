using System;
using System.Threading.Tasks;
using FluentValidation;

namespace Infrastructure.Mediator
{
    public abstract class AsyncRequestValidator<TRequest> : AbstractValidator<TRequest>, IRequestValidator<TRequest>
    {
        public virtual int Order => 1;

        protected virtual Task<ValidationResult> HasClaim(TRequest request)
        {
            return null;
        }

        protected new virtual Task<ValidationResult> Validate(TRequest request)
        {
            return Task.FromResult(ValidationResult.Ok());
        }

        public Task<ValidationResult> InternalValidate(TRequest request)
        {
            var result = HasClaim(request);
            if (result?.Result != null) return result;
            //Validate request properties
            result = Task.FromResult(FluentValidate(request));
            if (result?.Result != null) return result;
            //Validate request content
            return Validate(request);
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
