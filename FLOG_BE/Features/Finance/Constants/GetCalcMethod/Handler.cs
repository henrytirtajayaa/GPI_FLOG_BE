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
using LinqKit;
using FLOG.Core.Finance;

namespace FLOG_BE.Features.Finance.Constants.GetCalcMethod
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
            if (string.IsNullOrEmpty(request.Initiator.UserId))
                return ApiResult<Response>.Unauthorized();

            List<ResponseItem> list = new List<ResponseItem>();
            list.Add(new ResponseItem { CalcMethod = CALC_METHOD.MULTIPLY, Caption = CALC_METHOD.Caption(CALC_METHOD.MULTIPLY) });
            list.Add(new ResponseItem { CalcMethod = CALC_METHOD.DIVISION, Caption = CALC_METHOD.Caption(CALC_METHOD.DIVISION) });

            return ApiResult<Response>.Ok(new Response()
            {
                CalcMethods = list,
                ListInfo = null
            });
        }
    }
}
