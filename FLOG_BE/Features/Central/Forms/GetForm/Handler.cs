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
using FLOG_BE.Model.Central.Entities;
using LinqKit;

namespace FLOG_BE.Features.Central.SecurityRoles.Forms.GetForm
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FlogContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;

        public Handler(IHttpContextAccessor httpContextAccessor, FlogContext context, ILogin login, HATEOASLinkCollection linkCollection)
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

            var query = getUserCompany(request.Filter);
           
            var list = await PaginatedList<Form, ReponseForms>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            return ApiResult<Response>.Ok(new Response()
            {
                Forms = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Form> getUserCompany(RequestFilterForm filter)
        {
            var query = _context.Forms.Where(x => x.FormLink != "#" ).AsQueryable();

           
            var filterDb = filter.Module?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterDb.Any())
            {

                var predicate = PredicateBuilder.New<Form>(true);
                foreach (var filterItem in filterDb)
                {
                    if (filterItem != "ALL Form Group")
                    //predicate = predicate.or(x => (x.Module.Contains(filterItem)));
                    predicate = predicate.Or(x => x.Module.Contains(filterItem));
                }


                query = query.Where(predicate);
            }
            var filterFormName = filter.FormName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterFormName.Any())
            {

                var predicate = PredicateBuilder.New<Form>(true);
                foreach (var filterItem in filterFormName)
                {
                    predicate = predicate.And(x => (x.FormName != filterItem));
                }


                query = query.Where(predicate);
            }

            return query;
        }

      
    }
}