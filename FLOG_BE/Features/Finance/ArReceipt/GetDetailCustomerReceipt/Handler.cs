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

namespace FLOG_BE.Features.Finance.ArReceipt.GetDetailCustomerReceipt
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

            var query = getArReceipt(request.Initiator.UserId, request.Filter);
            query = getArReceiptSorted(query, request.Sort);

            var list = await PaginatedList<Entities.ArReceiptDetail, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            return ApiResult<Response>.Ok(new Response()
            {
                DetailReceipt = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.ArReceiptDetail> getArReceipt(string personId, RequestFilter filter)
        {
            var query = (from x in _context.ArReceiptDetails
                         join h in _context.ArReceiptHeaders on x.ReceiptHeaderId equals h.ReceiptHeaderId
                         select new Entities.ArReceiptDetail
                         {
                             ReceiptDetailId = x.ReceiptDetailId,
                             ReceiptHeaderId = x.ReceiptHeaderId,
                             ReceiveTransactionId = x.ReceiveTransactionId,
                             Description = x.Description,
                             OriginatingBalance = x.OriginatingBalance,
                             FunctionalBalance = x.FunctionalBalance,
                             OriginatingPaid = x.OriginatingPaid,
                             FunctionalPaid = x.FunctionalPaid,
                             Status = x.Status,
                         }).AsEnumerable().AsQueryable();


            var wherePredicates = PredicateBuilder.New<Entities.ArReceiptDetail>(true);
            if (filter.ReceiptHeaderId != null && filter.ReceiptHeaderId != Guid.Empty)
            {
                var predicate = PredicateBuilder.New<Entities.ArReceiptDetail>(false);
                predicate = predicate.Or(x => x.ReceiptHeaderId == filter.ReceiptHeaderId);
                wherePredicates.And(predicate);
            }
            if (filter.ReceiveTransactionId != null && filter.ReceiveTransactionId != Guid.Empty)
            {
                var predicate = PredicateBuilder.New<Entities.ArReceiptDetail>(false);
                predicate = predicate.Or(x => x.ReceiveTransactionId == filter.ReceiveTransactionId);
                wherePredicates.And(predicate);
            }

            query = query.Where(wherePredicates);



            return query;
        }

        private IQueryable<Entities.ArReceiptDetail> getArReceiptSorted(IQueryable<Entities.ArReceiptDetail> input, List<string> sort)
        {
            var query = input.OrderByDescending(x => x.OriginatingBalance).ThenByDescending(x => x.ReceiveTransactionId);

            return query;
        }
    }
}
