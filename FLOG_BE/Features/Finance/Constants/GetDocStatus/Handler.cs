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
using FLOG.Core;

namespace FLOG_BE.Features.Finance.Constants.GetDocStatus
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
            list.Add(new ResponseItem { DocStatus = DOCSTATUS.INACTIVE, Caption = DOCSTATUS.Caption(DOCSTATUS.INACTIVE) }); 
            list.Add(new ResponseItem { DocStatus = DOCSTATUS.NEW, Caption = DOCSTATUS.Caption(DOCSTATUS.NEW) });
            list.Add(new ResponseItem { DocStatus = DOCSTATUS.PROCESS, Caption = DOCSTATUS.Caption(DOCSTATUS.PROCESS) });
            list.Add(new ResponseItem { DocStatus = DOCSTATUS.DELETE, Caption = DOCSTATUS.Caption(DOCSTATUS.DELETE) });
            list.Add(new ResponseItem { DocStatus = DOCSTATUS.APPROVE, Caption = DOCSTATUS.Caption(DOCSTATUS.APPROVE) });
            list.Add(new ResponseItem { DocStatus = DOCSTATUS.DISAPPROVE, Caption = DOCSTATUS.Caption(DOCSTATUS.DISAPPROVE) });
            list.Add(new ResponseItem { DocStatus = DOCSTATUS.VOID, Caption = DOCSTATUS.Caption(DOCSTATUS.VOID) });
            list.Add(new ResponseItem { DocStatus = DOCSTATUS.CLOSE, Caption = DOCSTATUS.Caption(DOCSTATUS.CLOSE) });
            
            return ApiResult<Response>.Ok(new Response()
            {
                DocStatus = list,
                ListInfo = null
            });
        }
    }
}
