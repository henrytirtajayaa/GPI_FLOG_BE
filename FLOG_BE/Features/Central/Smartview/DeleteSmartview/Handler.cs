using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model.Central;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Infrastructure.Utils;
using FLOG_BE.Model.Central.Entities;

namespace FLOG_BE.Features.Central.Smartview.DeleteSmartview
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FlogContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;


        public Handler(IHttpContextAccessor httpContextAccessor, FlogContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _login = login;
            _linkCollection = linkCollection;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            var record = _context.Smartviews.FirstOrDefault(x => x.SmartviewId == request.Body.SmartviewId);
            
            if (record == null)
            {
                return ApiResult<Response>.ValidationError("SmartView Not Found!");  
            } else {
                _context.Attach(record);
                _context.Remove(record);
                await _context.SaveChangesAsync();

                return ApiResult<Response>.Ok(new Response()
                {
                    SmartviewId = request.Body.SmartviewId
                });
            };
            
        }
    }
}
