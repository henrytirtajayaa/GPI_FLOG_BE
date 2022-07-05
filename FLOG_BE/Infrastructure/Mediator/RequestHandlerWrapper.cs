using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Mediator
{
    public interface IRequestHandlerWrapper
    {
        Task<ApiResult> Handle(IRequest request, IServiceProvider services);
    }

    public interface IRequestHandlerWrapper<TResponse>
    {
        Task<ApiResult<TResponse>> Handle(IRequest<TResponse> request, IServiceProvider services);
    }

    public class RequestHandlerWrapper<TRequest> : IRequestHandlerWrapper where TRequest : IRequest
    {
        public async Task<ApiResult> Handle(IRequest request, IServiceProvider services)
        {
            var handler = GetRequestHandlerDelegate(request, services);

            foreach (var decorator in services.GetServices<IRequestHandlerDecorator<TRequest, Unit>>())
            {
                var previousHandler = handler;
                handler = () => decorator.Handle((TRequest)request, previousHandler);
            }

            return ApiResult.Translate(await handler());
        }

        private static RequestHandlerDelegate<Unit> GetRequestHandlerDelegate(IRequest request, IServiceProvider services)
        {
            if (services.GetService<IAsyncRequestHandler<TRequest>>() != null)
            {
                return async () =>
                {
                    var handler = services.GetService<IAsyncRequestHandler<TRequest>>();
                    var response = await handler.Handle((TRequest)request);
                    return ApiResult<Unit>.Translate(response, Unit.Value);
                };
            }

            if (services.GetService<IRequestHandler<TRequest>>() != null)
            {
                return async () =>
                {
                    var handler = services.GetService<IRequestHandler<TRequest>>();
                    var response = await Task.FromResult(handler.Handle((TRequest)request));
                    return ApiResult<Unit>.Translate(response, Unit.Value);
                };
            }

            throw new InvalidOperationException($"No handler was found for request of type {request.GetType()}");
        }
    }

    public class RequestHandlerWrapper<TRequest, TResponse> : IRequestHandlerWrapper<TResponse> where TRequest : IRequest<TResponse>
    {
        public async Task<ApiResult<TResponse>> Handle(IRequest<TResponse> request, IServiceProvider services)
        {
            var handler = GetRequestHandlerDelegate(request, services);

            foreach (var decorator in services.GetServices<IRequestHandlerDecorator<TRequest, TResponse>>())
            {
                var previousHandler = handler;
                handler = () => decorator.Handle((TRequest)request, previousHandler);
            }

            return await handler();
        }

        private static RequestHandlerDelegate<TResponse> GetRequestHandlerDelegate(IRequest<TResponse> request, IServiceProvider services)
        {
            if (services.GetService<IAsyncRequestHandler<TRequest, TResponse>>() != null)
            {
                return async () =>
                {
                    var handler = services.GetService<IAsyncRequestHandler<TRequest, TResponse>>();
                    return await handler.Handle((TRequest)request);
                };
            }

            if (services.GetService<IRequestHandler<TRequest, TResponse>>() != null)
            {
                return async () =>
                {
                    var handler = services.GetService<IRequestHandler<TRequest, TResponse>>();
                    return await Task.FromResult(handler.Handle((TRequest)request));
                };
            }

            throw new InvalidOperationException($"No handler was found for request of type {request.GetType()}");
        }
    }
}
