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

namespace FLOG_BE.Features.Finance.ApPayment.GetUserApprovalPayment
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

            var list = await PaginatedList<Entities.ApPaymentApproval, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            List<ResponseItem> response;

            
            response = new List<ResponseItem>(list.Select(x => new ResponseItem
            {
                PaymentApprovalId = x.PaymentApprovalId,
                PaymentHeaderId = x.PaymentHeaderId,
                Index = x.Index,
                PersonId = x.PersonId,
                PersonCategoryId = x.PersonCategoryId,
                Status = x.Status
            }));

            var responseData = new ResponseData();

            var CurrentApproval = GetCurrentUserApproval(response);
            if (CurrentApproval.CurrentIndex > 0)
            {
                responseData.MaxIndex = response.Max(x => x.Index);
                responseData.CurrentIndex = CurrentApproval.CurrentIndex;
              
            }
            responseData.StatusApproval = false;
            responseData.StatusPosting = false;

            var CekApprovalStatus = response.FirstOrDefault(x => x.PersonId == request.Filter.PersonId && x.Index == CurrentApproval.CurrentIndex);
            if (CekApprovalStatus != null)
            {
                if (CekApprovalStatus.Index == CurrentApproval.CurrentIndex)
                {
                    responseData.StatusApproval = true;
                }
                if (responseData.MaxIndex == responseData.CurrentIndex)
                {
                    responseData.StatusPosting = true;
                }
                responseData.PaymentApprovalId = CekApprovalStatus.PaymentApprovalId;
                responseData.PersonId = request.Filter.PersonId;

            }
            var DetailResponse = response.Where(x => x.Index == responseData.CurrentIndex).ToList();
            return ApiResult<Response>.Ok(new Response()
            {
                DetailEntries = DetailResponse,
                ListInfo = list.ListInfo,
                UserApproval = responseData
            });

        }

        private ResponseData GetCurrentUserApproval(List<ResponseItem> response)
        {
            var result = new ResponseData();
            foreach (var item in response)
            {
                if (item.Status == DOCSTATUS.NEW)
                {
                    result.CurrentIndex = item.Index;
                    return result;
                }
                else { }
            }

            return result;
        }

        private IQueryable<Entities.ApPaymentApproval> GetTransactions(string personId, RequestFilter filter)
        {

            var query = _context.ApPaymentApprovals
                         .Where(x => x.PaymentHeaderId == filter.PaymentHeaderId)
                         .AsEnumerable().ToList().AsQueryable();

            return query;
        }

        private IQueryable<Entities.ApPaymentApproval> GetTransactionSorted(IQueryable<Entities.ApPaymentApproval> input, List<string> sort)
        {
            var query = input.OrderBy(x => x.Index);

            return query;
        }
    }
}
