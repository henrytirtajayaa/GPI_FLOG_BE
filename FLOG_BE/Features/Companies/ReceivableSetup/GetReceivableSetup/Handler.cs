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

namespace FLOG_BE.Features.Companies.ReceivableSetup.GetReceivableSetup
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
            _context = context;
            _login = login;
            _linkCollection = linkCollection;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            if (string.IsNullOrEmpty(request.Initiator.UserId))
                return ApiResult<Response>.Unauthorized();

            var query = getReceivableSetup(request.Initiator.UserId, request.Filter);
            query = getReceivableSetupSorted(query, request.Sort);

            var list = await PaginatedList<Entities.ReceivableSetup, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            return ApiResult<Response>.Ok(new Response()
            {
                ReceivableSetups = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.ReceivableSetup> getReceivableSetup(string personId, RequestFilter filter)
        {
            var query = _context.ReceivableSetups.Where(x=>x.Status != FLOG.Core.DOCSTATUS.DELETE).AsQueryable();
                       
            return query;
        }

        private IQueryable<Entities.ReceivableSetup> getReceivableSetupSorted(IQueryable<Entities.ReceivableSetup> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());

            return query;
        }
    }
}
