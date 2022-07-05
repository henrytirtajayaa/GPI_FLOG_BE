using System;
using System.Threading.Tasks;

namespace Infrastructure.Mediator
{
    public interface IRequestValidator<in TRequest>
    {
        Task<ValidationResult> InternalValidate(TRequest request);
        int Order { get; }
    }
}
