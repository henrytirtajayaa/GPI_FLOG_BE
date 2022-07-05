using System;
using System.Threading.Tasks;
using Infrastructure.Authentication;

namespace Infrastructure.Mediator
{
    public interface IMediator
    {
        Task<ApiResult> Send(IRequest request);
        Task<ApiResult<TResponse>> Send<TResponse>(IRequest<TResponse> request);
    }

    public class Mediator : IMediator
    {
        private readonly IServiceProvider _services;

        public Mediator(IServiceProvider services)
        {
            _services = services;
        }

        public Task<ApiResult> Send(IRequest request)
        {
            var handlerType = typeof(RequestHandlerWrapper<>).MakeGenericType(request.GetType());
            var handlerWrapper = (IRequestHandlerWrapper)Activator.CreateInstance(handlerType);

            return handlerWrapper.Handle(request, _services);
        }

        public Task<ApiResult<TResponse>> Send<TResponse>(IRequest<TResponse> request)
        {
            var handlerType = typeof(RequestHandlerWrapper<,>).MakeGenericType(request.GetType(), typeof(TResponse));
            var handlerWrapper = (IRequestHandlerWrapper<TResponse>)Activator.CreateInstance(handlerType);

            return handlerWrapper.Handle(request, _services);
        }
    }
}
