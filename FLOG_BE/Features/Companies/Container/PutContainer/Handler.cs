using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model.Companies;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Infrastructure.Utils;
using FLOG_BE.Model.Companies.Entities;

namespace FLOG_BE.Features.Companies.Container.PutContainer
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {

            var response = new Response()
            {
                Containerid = request.Body.Containerid,
                ContainerName = request.Body.ContainerName,
                ContainerSize = request.Body.ContainerSize,
                ContainerTeus = request.Body.ContainerTeus,
                ContainerType = request.Body.ContainerType,
                IsReefer = request.Body.IsReefer,
                Inactive = request.Body.Inactive
            };
            var container = await _context.Containers.FirstOrDefaultAsync(x => x.ContainerId == request.Body.Containerid);
            if (container != null)
            {
                container.ContainerName = request.Body.ContainerName;
                container.ContainerSize = request.Body.ContainerSize;
                container.ContainerTeus = request.Body.ContainerTeus;
                container.ContainerType = request.Body.ContainerType;
                container.Inactive = request.Body.Inactive;
                container.ModifiedBy = request.Initiator.UserId;
                container.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                response.ContainerCode = container.ContainerCode;
            }

            return ApiResult<Response>.Ok(response);
        }
    }
}
