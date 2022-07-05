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

namespace FLOG_BE.Features.Finance.ApPayment.GetApprovalComment
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

            var list = await PaginatedList<Entities.ApPaymentApprovalComment, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

           
            return ApiResult<Response>.Ok(new Response()
            {
                ApprovalComments = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.ApPaymentApprovalComment> GetTransactions(string personId, RequestFilter filter)
        {
             List<ApPaymentApproval> ListApp =  _context.ApPaymentApprovals.Where(x => x.PaymentHeaderId == filter.PaymentHeaderId ).ToList();
             List<Person> ListUser =  _Flogcontext.Persons.ToList();


            
            
            var query = (from x in _context.ApPaymentApprovalComments
                         select new Entities.ApPaymentApprovalComment
                         {

                             PaymentApprovalId = x.PaymentApprovalId,
                             CommentDate = x.CommentDate,
                             PersonId = x.PersonId,
                             Comments = x.Comments,
                             Status = x.Status,
                             UserFullName = ListUser.Where(p => Guid.Parse(p.PersonId) == x.PersonId).Select(p => p.PersonFullName).FirstOrDefault()


                         }).AsEnumerable().ToList().AsQueryable();


            //var query = _context.ApPaymentApprovalComments
            //            .AsEnumerable().ToList().AsQueryable(); ;

            if (ListApp.Count > 0)
            {

                var predicate = PredicateBuilder.New<Entities.ApPaymentApprovalComment>(true);
                foreach (var filterItem in ListApp)
                {
                    predicate = predicate.Or(x => x.PaymentApprovalId == filterItem.PaymentApprovalId);
                }
                query = query.Where(predicate);
            }else {
                var predicate = PredicateBuilder.New<Entities.ApPaymentApprovalComment>(true);
                    predicate = predicate.Or(x => x.PaymentApprovalId == null);
                query = query.Where(predicate);
            }

            return query;
        }

        private IQueryable<Entities.ApPaymentApprovalComment> GetTransactionSorted(IQueryable<Entities.ApPaymentApprovalComment> input, List<string> sort)
        {
            var query = input.OrderByDescending(x => x.CommentDate);

            return query;
        }
    }
}
