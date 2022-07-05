using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Mediator
{
    public class IdentityDecorator<TRequest, TResponse> : IRequestHandlerDecorator<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IdentityDecorator(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResult<TResponse>> Handle(TRequest request, RequestHandlerDelegate<TResponse> next)
        {
            request.Initiator = _httpContextAccessor.GetInitiator();
            return await next();
        }
    }
}
