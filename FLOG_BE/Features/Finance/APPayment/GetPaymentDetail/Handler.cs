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

namespace FLOG_BE.Features.Finance.ApPayment.GetPaymentDetail
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FlogContext _contextCentral;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;

        public Handler(IHttpContextAccessor httpContextAccessor, FlogContext contextCentral, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _contextCentral = contextCentral;
            _login = login;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            if (string.IsNullOrEmpty(request.Initiator.UserId))
                return ApiResult<Response>.Unauthorized();

            var query = getPaymentProgressDetail(request.Initiator.UserId, request.Filter);
            
            var list = await PaginatedList<Entities.ApPaymentDetail, ResponseDetailItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());


            return ApiResult<Response>.Ok(new Response()
            {
                PaymentDetail = list,
                ListInfo = list.ListInfo
            });
        }
      
     
        private IQueryable<Entities.ApPaymentDetail> getPaymentProgressDetail(string personId, RequestFilterDetail filter)
        {
          
            var query =  _context.ApPaymentDetails
                .Where(x => x.PayableTransactionId == filter.PayableTransactionId)
                .OrderByDescending( x => x.RowId)
                .AsQueryable();

           
            return query;
        }

       
    }
}
