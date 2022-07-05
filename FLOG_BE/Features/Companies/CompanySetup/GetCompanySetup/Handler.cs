using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model.Central;
using FLOG_BE.Model.Companies;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Infrastructure.Utils;
using Entities = FLOG_BE.Model.Companies.Entities;
using LinqKit;

namespace FLOG_BE.Features.Companies.CompanySetup.GetCompanySetup
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FlogContext _contextCentral;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, FlogContext contextCentral, ILogin login, HATEOASLinkCollection linkCollection)
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

            var query = getUserCompanySetup(request.Initiator.UserId, request.Filter);
            query = getUserCompanySetUpSorted(query, request.Sort);
                        
            var list = await PaginatedList<Entities.CompanySetup, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());
            return ApiResult<Response>.Ok(new Response()
            {
                CompanySetup = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.CompanySetup> getUserCompanySetup(string personId, RequestFilter filter)
        {
            var query = (from comp in _context.CompanySetups
                         select new Entities.CompanySetup
                         {
                             CompanyId = comp.CompanyId,
                             CompanyName = comp.CompanyName,
                             CompanySetupId = comp.CompanySetupId,
                             CompanyTaxName = comp.CompanyTaxName,
                             CompanyLogo = comp.CompanyLogo,
                             CompanyAddressId = comp.CompanyAddressId,
                             TaxRegistrationNo = comp.TaxRegistrationNo,
                             CreatedBy = comp.CreatedBy,
                             CreatedDate = comp.CreatedDate,
                             ModifiedBy = comp.ModifiedBy,
                             ModifiedDate = comp.ModifiedDate,
                             LogoImageData = comp.LogoImageData,
                             LogoImageTitle = comp.LogoImageTitle,
                             LogoImageType = comp.LogoImageType,
                             LogoImageUrl = (comp.LogoImageData != null && comp.LogoImageData.Length > 0 ? string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(comp.LogoImageData)) : ""),
                             CompanyAddress  = (from myaddr in _context.CompanyAddresses
                                     where myaddr.CompanyAddressId == comp.CompanyAddressId
                                     select myaddr).FirstOrDefault(),                             
                         }).AsEnumerable().ToList().AsQueryable();

            var filterName = filter.CompanyName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterName.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanySetup>(true);
                foreach (var filterItem in filterName)
                {
                    predicate = predicate.Or(x => x.CompanyName.Contains(filterItem));
                }
                query = query.Where(predicate);
            }

            var filterTaxRegistrationNo = filter.CompanyTaxName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterTaxRegistrationNo.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanySetup>(true);
                foreach (var filterItem in filterTaxRegistrationNo)
                {
                    predicate = predicate.Or(x => x.CompanyTaxName.Contains(filterItem));
                }
                query = query.Where(predicate);
            }
            var filterLogo = filter.CompanyLogo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterLogo.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanySetup>(true);
                foreach (var filterItem in filterLogo)
                {
                    predicate = predicate.Or(x => x.CompanyLogo.Contains(filterItem));
                }
                query = query.Where(predicate);
            }

            return query;
        }

        private IQueryable<Entities.CompanySetup> getUserCompanySetUpSorted(IQueryable<Entities.CompanySetup> input, List<string> sort)
        {
            //      public string CompanyName { get; set; }
            //public string CompanyAddressId { get; set; }
            //public string TaxRegistrationNo { get; set; }
            //public string CompanyTaxName { get; set; }
            //public string CompanyLogo { get; set; }


            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("CompanyName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CompanyName) : query.ThenBy(x => x.CompanyName);
                }

                if (item.Contains("CompanyTaxName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CompanyTaxName) : query.ThenBy(x => x.CompanyTaxName);
                }

                if (item.Contains("CompanyLogo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CompanyLogo) : query.ThenBy(x => x.CompanyLogo);
                }
            }


            return query;
        }
    }
}
