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
using FLOG.Core.DocumentNo;

namespace FLOG_BE.Features.Sales.NegotiationSheet.GetNegotiationSheetApprovalComment
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

            var list = await PaginatedList<Entities.TrxDocumentApprovalComment, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());
                       
            return ApiResult<Response>.Ok(new Response()
            {
                ApprovalComments = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.TrxDocumentApprovalComment> GetTransactions(string personId, RequestFilter filter)
        {
             List<Person> ListUser =  _Flogcontext.Persons.ToList();

            var query = (from c in _context.TrxDocumentApprovalComments
                         join nsa in _context.TrxDocumentApprovals on c.TrxDocumentApprovalId equals nsa.TrxDocumentApprovalId
                         where nsa.TransactionId == filter.NegotiationSheetId && nsa.TrxModule == TRX_MODULE.TRX_SALES && nsa.ModeStatus == DOCSTATUS.NEW && nsa.DocFeatureId == DOCNO_FEATURE.TRXTYPE_SALES_NEGOSHEET
                         orderby c.CommentDate descending
                         select new Entities.TrxDocumentApprovalComment
                         {
                             TrxDocumentApprovalCommentId = c.TrxDocumentApprovalCommentId,
                             TrxDocumentApprovalId = c.TrxDocumentApprovalId,
                             TransactionId = filter.NegotiationSheetId,
                             CommentDate = c.CommentDate,
                             PersonId = c.PersonId,
                             Comments = c.Comments,
                             Status = c.Status,
                             UserFullName = ListUser.Where(p => Guid.Parse(p.PersonId) == c.PersonId).Select(p => p.PersonFullName).FirstOrDefault()                             
                         }).AsEnumerable().ToList().AsQueryable();

            return query;
        }

        private IQueryable<Entities.TrxDocumentApprovalComment> GetTransactionSorted(IQueryable<Entities.TrxDocumentApprovalComment> input, List<string> sort)
        {
            var query = input.OrderByDescending(x => x.CommentDate);

            return query;
        }
    }
}
