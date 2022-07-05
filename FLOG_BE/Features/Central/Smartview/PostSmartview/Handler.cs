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

namespace FLOG_BE.Features.Central.Smartview.PostSmartview
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
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            

            Guid obj = Guid.NewGuid();
            var smartView = new SmartView()
            {
                SmartviewId = obj,
                SmartTitle = request.Body.SmartTitle,
                SqlViewName = request.Body.SqlViewName,
                GroupName = request.Body.GroupName,
                isFunction = false
            };

            _context.Smartviews.Add(smartView);

            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response()
            {
                SmartViewId = smartView.SmartviewId,
                SmartTitle = smartView.SmartTitle
            });
        }
    }
}
