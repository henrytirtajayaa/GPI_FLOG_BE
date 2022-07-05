using System;
using System.Threading.Tasks;
using Infrastructure.Authentication;

namespace Infrastructure.Mediator
{
    public interface IRequest : IRequest<Unit> { }

    public interface IRequest<out TResponse> : IInitiator
    {
    }

    public interface IRequestHandler<in TRequest> where TRequest : IRequest
    {
        ApiResult Handle(TRequest request);
    }

    public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        ApiResult<TResponse> Handle(TRequest request);
    }

    public interface IAsyncRequestHandler<in TRequest> where TRequest : IRequest
    {
        Task<ApiResult> Handle(TRequest request);
    }

    public interface IAsyncRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        Task<ApiResult<TResponse>> Handle(TRequest request);
    }
}
