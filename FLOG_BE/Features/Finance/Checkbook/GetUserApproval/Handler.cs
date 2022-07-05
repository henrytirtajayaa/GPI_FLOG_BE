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

namespace FLOG_BE.Features.Finance.Checkbook.GetUserApproval
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly FlogContext _flogcontext;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, FlogContext flogcontext, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _flogcontext = flogcontext;
            _login = login;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            if (string.IsNullOrEmpty(request.Initiator.UserId))
                return ApiResult<Response>.Unauthorized();

            var query = GetTransactions(request.Initiator.UserId, request.Filter);
            query = GetTransactionSorted(query, request.Sort);

            var list = await PaginatedList<Entities.CheckbookTransactionApproval, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            List<ResponseItem> response;
                        
            response = new List<ResponseItem>(list.Select(x => new ResponseItem
            {
                CheckbookTransactionApprovalId = x.CheckbookTransactionApprovalId,
                CheckbookTransactionId = x.CheckbookTransactionId,
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

            var CekCurrentApproval = response.FirstOrDefault(x => x.Index == CurrentApproval.CurrentIndex);
            if (CekCurrentApproval != null)
            {
                if (CekCurrentApproval.PersonCategoryId == null || CekCurrentApproval.PersonCategoryId == Guid.Empty)
                {
                    var cekApproval = _flogcontext.Persons.FirstOrDefault(x => Guid.Parse(x.PersonId) == CekCurrentApproval.PersonId);
                    responseData.CurrentApproval = cekApproval.PersonFullName;
                }
                else
                {
                    var cekApproval = _flogcontext.PersonCategories.FirstOrDefault(x => Guid.Parse(x.PersonCategoryId) == CekCurrentApproval.PersonCategoryId);
                    responseData.CurrentApproval = cekApproval.PersonCategoryName;
                }
            }
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
            }

            return result;
        }

        private IQueryable<Entities.CheckbookTransactionApproval> GetTransactions(string personId, RequestFilter filter)
        {

            var query = _context.CheckbookTransactionApprovals
                         .Where(x => x.CheckbookTransactionId == filter.CheckbookTransactionId)
                         .AsEnumerable().ToList().AsQueryable();

            return query;
        }

        private IQueryable<Entities.CheckbookTransactionApproval> GetTransactionSorted(IQueryable<Entities.CheckbookTransactionApproval> input, List<string> sort)
        {
            var query = input.OrderBy(x => x.Index);

            return query;
        }
    }
}
