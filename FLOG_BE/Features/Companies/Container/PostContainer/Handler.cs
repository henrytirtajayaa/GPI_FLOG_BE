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
using Entities = FLOG_BE.Model.Companies.Entities;

namespace FLOG_BE.Features.Companies.Container.PostContainer
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
            try
            {
                var container = new Entities.Container()
                {
                    ContainerId = Guid.NewGuid(),
                    ContainerCode = request.Body.ContainerCode,
                    ContainerName = request.Body.ContainerName,
                    ContainerSize = request.Body.ContainerSize,
                    ContainerTeus = request.Body.ContainerTeus,
                    ContainerType = request.Body.ContainerType,
                    Inactive = request.Body.Inactive,
                    CreatedBy = request.Initiator.UserId,
                    CreatedDate = DateTime.Now
                };

                _context.Containers.Add(container);

                await _context.SaveChangesAsync();

                return ApiResult<Response>.Ok(new Response()
                {
                    Containerid = container.ContainerId,
                    ContainerCode = container.ContainerCode,
                    ContainerName = container.ContainerName,
                    ContainerSize = container.ContainerSize,
                    ContainerTeus = container.ContainerTeus,
                    ContainerType = container.ContainerType,
                    Inactive = container.Inactive
                });
            }
            catch (Exception ex)
            {
                return ApiResult<Response>.ValidationError(ex.Message);
            }
        }
    }
}
