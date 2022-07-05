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
using Entities = FLOG_BE.Model.Companies.Entities;
using LinqKit;
using FLOG_BE.Model.Companies;
using FLOG.Core;
using FLOG_BE.Model.Central.Entities;
using FLOG_BE.Model.Companies.Entities;

namespace FLOG_BE.Features.Finance.Checkbook.GetDetail
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

            var query = GetTransactions(request.Initiator.UserId, request.Filter);
            query = GetTransactionSorted(query, request.Sort);

            var list = await PaginatedList<Entities.CheckbookTransactionDetail, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            List<ResponseItem> response;

            List<Charges> ListCharges = _context.Charges.ToList();

            response = new List<ResponseItem>(list.Select(x => new ResponseItem
            {
                CheckbookTransactionId = x.CheckbookTransactionId,
                ChargesDescription = x.ChargesDescription,
                ChargesId = x.ChargesId,
                FunctionalAmount = x.FunctionalAmount,
                OriginatingAmount = x.OriginatingAmount,
                TransactionDetailId = x.TransactionDetailId,
                Status = x.Status,
                ChargesCode = ListCharges.Where(c => c.ChargesId == x.ChargesId).Select(c => c.ChargesCode).FirstOrDefault(),
                ChargesName = ListCharges.Where(c => c.ChargesId == x.ChargesId).Select(c => c.ChargesName).FirstOrDefault()
            }));

            return ApiResult<Response>.Ok(new Response()
            {
                DetailEntries = response,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.CheckbookTransactionDetail> GetTransactions(string personId, RequestFilter filter)
        {

            var query = _context.CheckbookTransactionDetails
                         .Where(x => x.CheckbookTransactionId == filter.CheckbookTransactionId)
                         .OrderBy(x=>x.RowIndex)
                         .AsEnumerable().ToList().AsQueryable();

            return query;
        }

        private IQueryable<Entities.CheckbookTransactionDetail> GetTransactionSorted(IQueryable<Entities.CheckbookTransactionDetail> input, List<string> sort)
        {
            var query = input.OrderBy(x => x.RowIndex);

            return query;
        }
    }
}
