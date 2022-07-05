using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model.Central;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using AutoMapper;
using FLOG_BE.Helper;
using FLOG_BE.Features.Authentication.DoLogin;
using Infrastructure.Utils;
using LinqKit;
using FLOG_BE.Model.Central.Entities;

namespace FLOG_BE.Features.Authentication.GetSession
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FlogContext _context;
        private readonly ILogin _login;
        private readonly IMapper _mapper;

        public Handler(IHttpContextAccessor httpContextAccessor, FlogContext context, ILogin login, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _login = login;
            _mapper = mapper;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            string Id = null;
            Model.Central.Entities.SessionState SessionState = await _context.SessionStates.Where(x => x.PersonId == request.PersonId).FirstOrDefaultAsync();
            if (SessionState != null)
            {

                return ApiResult<Response>.Ok(new Response()
                {
                    Id = SessionState.Id.ToString(),
                    PersonId = SessionState.PersonId,
                    CompanySecurityId = SessionState.CompanySecurityId,
                    CompanyId = SessionState.CompanyId,
                    CreatedDate = SessionState.CreatedDate,
                });
            }else {
                return ApiResult<Response>.Ok(new Response()
                {
                    Id = null
                });
               
            }



        }

    }
}
