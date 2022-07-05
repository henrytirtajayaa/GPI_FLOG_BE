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

namespace FLOG_BE.Features.Finance.Checkbook.GetApprovalComment
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly FlogContext _Flogcontext;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, FlogContext Flogcontext, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _Flogcontext = Flogcontext;
            _login = login;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            if (string.IsNullOrEmpty(request.Initiator.UserId))
                return ApiResult<Response>.Unauthorized();

            var query = GetTransactions(request.Initiator.UserId, request.Filter);
            query = GetTransactionSorted(query, request.Sort);

            var list = await PaginatedList<Entities.CheckbookApprovalComment, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            List<ResponseItem> response;
            List<Person> ListUser = await _Flogcontext.Persons.ToListAsync();
            List<Entities.CheckbookTransactionApproval> ListApproval = await _context.CheckbookTransactionApprovals.ToListAsync();
            response = new List<ResponseItem>(list.Select(x => new ResponseItem
            {
                CheckbookTransactionApprovalId = x.CheckbookTransactionApprovalId,
                ApprovalCommentId = x.ApprovalCommentId,
                CommentDate = x.CommentDate,
                PersonId = x.PersonId,
                Comments = x.Comments,
                Status = x.Status,
                CheckbookTransactionId = ListApproval.Where(c => c.CheckbookTransactionApprovalId == x.CheckbookTransactionApprovalId).Select(c => c.CheckbookTransactionId).FirstOrDefault(),
                UserFullName = ListUser.Where(p => Guid.Parse(p.PersonId) == x.PersonId).Select(p => p.PersonFullName).FirstOrDefault()
            }));
            
            response = response.Where(x => x.CheckbookTransactionId == request.Filter.CheckbookTransactionId).ToList();

            return ApiResult<Response>.Ok(new Response()
            {
                ApprovalComments = response,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.CheckbookApprovalComment> GetTransactions(string personId, RequestFilter filter)
        {
            var query = (from comm in _context.CheckbookApprovalComments
                         join det in _context.CheckbookTransactionApprovals on comm.CheckbookTransactionApprovalId equals det.CheckbookTransactionApprovalId
                         where det.CheckbookTransactionId == filter.CheckbookTransactionId
                         select new Entities.CheckbookApprovalComment()
                         {
                             ApprovalCommentId = comm.ApprovalCommentId,
                             CheckbookTransactionApprovalId = comm.CheckbookTransactionApprovalId,
                             CommentDate = comm.CommentDate,
                            Comments = comm.Comments,
                            PersonId = comm.PersonId,
                            Status = comm.Status
                         }
                        ).AsEnumerable().ToList().AsQueryable(); ;

            return query;
        }

        private IQueryable<Entities.CheckbookApprovalComment> GetTransactionSorted(IQueryable<Entities.CheckbookApprovalComment> input, List<string> sort)
        {
            var query = input.OrderByDescending(x => x.CommentDate);

            return query;
        }
    }
}
