using FLOG_BE.Model.Central;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Central.Smartview.PutrSmartview
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
            var response = new Response()
            {
                SmartViewId = request.Body.SmartViewId
            };

            var smart = await _context.Smartviews.FirstOrDefaultAsync(x => x.SmartviewId == request.Body.SmartViewId);
            if (smart != null)
            {
                smart.GroupName = request.Body.GroupName;
                smart.SmartTitle = request.Body.SmartTitle;
                smart.SqlViewName = request.Body.SqlViewName;

                await _context.SaveChangesAsync();

                response.SmartViewId = smart.SmartviewId;
            }

            return ApiResult<Response>.Ok(response);
        }
    }
}
