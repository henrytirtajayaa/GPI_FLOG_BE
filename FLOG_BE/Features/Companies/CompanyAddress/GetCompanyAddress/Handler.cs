using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model.Companies;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Infrastructure.Utils;
using Entities = FLOG_BE.Model.Companies.Entities;
using LinqKit;

namespace FLOG_BE.Features.Companies.CompanyAddress.GetCompanyAddress
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

            var query = getCompanyAddress(request.Initiator.UserId, request.Filter);
            query = getCompanyAddressSorted(query, request.Sort);

            List<Entities.City> ListCity = await _context.Cities.ToListAsync();
            List<Entities.Country> ListCountry = await _context.Countries.ToListAsync();


            var list = await PaginatedList<Entities.CompanyAddress, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());


            List<ResponseItem> response;

            response= new List<ResponseItem>(list.Select(x => new ResponseItem
            {
                CompanyAddressId = x.CompanyAddressId,
                AddressCode = x.AddressCode,
                AddressName = x.AddressName,
                ContactPerson = x.ContactPerson,
                Address = x.Address,
                Handphone = x.Handphone,
                EmailAddress = x.EmailAddress,
                Phone1 = x.Phone1,
                Extension1 = x.Extension1,
                Phone2 = x.Phone2,
                Extension2 = x.Extension2,
                Fax = x.Fax,
                HomePage = x.HomePage,
                Neighbourhood = x.Neighbourhood,
                Hamlet = x.Hamlet,
                UrbanVillage = x.UrbanVillage,
                SubDistrict = x.SubDistrict,
                CityCode = x.CityCode,
                CityName = ListCity.Where(p => p.CityCode == x.CityCode).Select(p => p.CityName).FirstOrDefault(),
                Province = ListCity.Where(p => p.CityCode == x.CityCode).Select(p => p.Province).FirstOrDefault(),
                CountryId = ListCity.Where(p => p.CityCode == x.CityCode).Select(p => p.CountryID).FirstOrDefault(),
                CountryName = "",
                PostCode = x.PostCode,
                IsSameAddress = x.IsSameAddress,
                TaxAddressId = x.TaxAddressId,
                TaxAddressName = list.Where(p => p.AddressCode == x.AddressCode).Select(p => p.AddressName).FirstOrDefault(),
                Default = x.Default,
                CreatedDate = x.CreatedDate,
                ModifiedBy = x.ModifiedBy,
                ModifiedDate = x.ModifiedDate,
            })); ;

            for (var i = 0; i < response.Count(); i++)
            {
                response[i].CountryName = ListCountry.Where(p => p.CountryId == response[i].CountryId).Select(p => p.CountryName).FirstOrDefault();
            }

            return ApiResult<Response>.Ok(new Response()
            {
                CompanyAddresses = response,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.CompanyAddress> getCompanyAddress(string personId, RequestFilter filter)
        {
            var query = _context.CompanyAddresses
                     .AsQueryable();

            var filterCode = filter.AddressCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanyAddress>(true);
                foreach (var filterItem in filterCode)
                {
                    predicate = predicate.Or(x => x.AddressCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterName = filter.AddressName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterName.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanyAddress>(true);
                foreach (var filterItem in filterName)
                {
                    predicate = predicate.Or(x => x.AddressName.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterContactPerson = filter.ContactPerson?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterContactPerson.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanyAddress>(true);
                foreach (var filterItem in filterContactPerson)
                {
                    predicate = predicate.Or(x => x.ContactPerson.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterAddress = filter.Address?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterAddress.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanyAddress>(true);
                foreach (var filterItem in filterAddress)
                {
                    predicate = predicate.Or(x => x.Address.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterHandphone = filter.Handphone?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterHandphone.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanyAddress>(true);
                foreach (var filterItem in filterHandphone)
                {
                    predicate = predicate.Or(x => x.Handphone.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterPhone1 = filter.Phone1?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterPhone1.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanyAddress>(true);
                foreach (var filterItem in filterPhone1)
                {
                    predicate = predicate.Or(x => x.Phone1.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterExtension1 = filter.Extension1?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterExtension1.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanyAddress>(true);
                foreach (var filterItem in filterExtension1)
                {
                    predicate = predicate.Or(x => x.Extension1.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterPhone2 = filter.Phone2?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterPhone2.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanyAddress>(true);
                foreach (var filterItem in filterPhone2)
                {
                    predicate = predicate.Or(x => x.Phone2.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterExtension2 = filter.Extension2?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterExtension2.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanyAddress>(true);
                foreach (var filterItem in filterExtension2)
                {
                    predicate = predicate.Or(x => x.Extension2.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterFax = filter.Fax?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterFax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanyAddress>(true);
                foreach (var filterItem in filterFax)
                {
                    predicate = predicate.Or(x => x.Fax.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterEmailAddress = filter.EmailAddress?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterEmailAddress.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanyAddress>(true);
                foreach (var filterItem in filterEmailAddress)
                {
                    predicate = predicate.Or(x => x.EmailAddress.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterHomePage = filter.HomePage?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterHomePage.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanyAddress>(true);
                foreach (var filterItem in filterHomePage)
                {
                    predicate = predicate.Or(x => x.HomePage.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterNeighbourhood = filter.Neighbourhood?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterNeighbourhood.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanyAddress>(true);
                foreach (var filterItem in filterNeighbourhood)
                {
                    predicate = predicate.Or(x => x.Neighbourhood.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterHamlet = filter.Hamlet?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterHamlet.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanyAddress>(true);
                foreach (var filterItem in filterHamlet)
                {
                    predicate = predicate.Or(x => x.Hamlet.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterUrbanVillage = filter.UrbanVillage?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterUrbanVillage.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanyAddress>(true);
                foreach (var filterItem in filterUrbanVillage)
                {
                    predicate = predicate.Or(x => x.UrbanVillage.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterSubDistrict = filter.SubDistrict?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterSubDistrict.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanyAddress>(true);
                foreach (var filterItem in filterSubDistrict)
                {
                    predicate = predicate.Or(x => x.SubDistrict.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterCityCode = filter.CityCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCityCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanyAddress>(true);
                foreach (var filterItem in filterCityCode)
                {
                    predicate = predicate.Or(x => x.CityCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterPostCode = filter.PostCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterPostCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanyAddress>(true);
                foreach (var filterItem in filterPostCode)
                {
                    predicate = predicate.Or(x => x.PostCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            if (filter.IsSameAddress.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.CompanyAddress>(true);
                predicate = predicate.Or(x => x.IsSameAddress == filter.IsSameAddress);
                query = query.Where(predicate);
            }

            var filterTaxAddressId = filter.TaxAddressId?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterTaxAddressId.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanyAddress>(true);
                foreach (var filterItem in filterTaxAddressId)
                {
                    predicate = predicate.Or(x => x.TaxAddressId.Contains(filterItem));
                }
                query = query.Where(predicate);
            }

            if (filter.Default.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.CompanyAddress>(true);
                predicate = predicate.Or(x => x.Default == filter.Default);
                query = query.Where(predicate);
            }

            var filterCreatedBy = filter.CreatedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCreatedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanyAddress>(true);
                foreach (var filterItem in filterCreatedBy)
                {
                    predicate = predicate.Or(x => x.CreatedBy.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterCreatedDateStart = filter.CreatedDateStart?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanyAddress>(true);
                foreach (var filterItem in filterCreatedDateStart)
                {
                    predicate = predicate.Or(x => x.CreatedDate.HasValue && ((DateTime)x.CreatedDate).Date >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterCreatedDateEnd = filter.CreatedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanyAddress>(true);
                foreach (var filterItem in filterCreatedDateEnd)
                {
                    predicate = predicate.Or(x => x.CreatedDate.HasValue && ((DateTime)x.CreatedDate).Date <= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterModifiedBy = filter.ModifiedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterModifiedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanyAddress>(true);
                foreach (var filterItem in filterModifiedBy)
                {
                    predicate = predicate.Or(x => x.ModifiedBy.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterModifiedDateStart = filter.ModifiedDateStart?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanyAddress>(true);
                foreach (var filterItem in filterModifiedDateStart)
                {
                    predicate = predicate.Or(x => x.ModifiedDate.HasValue && ((DateTime)x.ModifiedDate).Date >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterModifiedDateEnd = filter.ModifiedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanyAddress>(true);
                foreach (var filterItem in filterModifiedDateEnd)
                {
                    predicate = predicate.Or(x => x.ModifiedDate.HasValue && ((DateTime)x.ModifiedDate).Date <= filterItem);
                }
                query = query.Where(predicate);
            }

            return query;
        }

        private IQueryable<Entities.CompanyAddress> getCompanyAddressSorted(IQueryable<Entities.CompanyAddress> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("AddressCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.AddressCode) : query.ThenBy(x => x.AddressCode);
                }

                if (item.Contains("AddressName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.AddressName) : query.ThenBy(x => x.AddressName);
                }

                if (item.Contains("ContactPerson", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ContactPerson) : query.ThenBy(x => x.ContactPerson);
                }

                if (item.Contains("Address", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Address) : query.ThenBy(x => x.Address);
                }

                if (item.Contains("Handphone", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Handphone) : query.ThenBy(x => x.Handphone);
                }

                if (item.Contains("Phone1", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Phone1) : query.ThenBy(x => x.Phone1);
                }

                if (item.Contains("Extension1", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Extension1) : query.ThenBy(x => x.Extension1);
                }

                if (item.Contains("Phone2", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Phone2) : query.ThenBy(x => x.Phone2);
                }

                if (item.Contains("Extension2", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Extension2) : query.ThenBy(x => x.Extension2);
                }

                if (item.Contains("Fax", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Fax) : query.ThenBy(x => x.Fax);
                }

                if (item.Contains("EmailAddress", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.EmailAddress) : query.ThenBy(x => x.EmailAddress);
                }

                if (item.Contains("HomePage", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.HomePage) : query.ThenBy(x => x.HomePage);
                }

                if (item.Contains("Neighbourhood", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Neighbourhood) : query.ThenBy(x => x.Neighbourhood);
                }

                if (item.Contains("Hamlet", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Hamlet) : query.ThenBy(x => x.Hamlet);
                }

                if (item.Contains("UrbanVillage", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.UrbanVillage) : query.ThenBy(x => x.UrbanVillage);
                }

                if (item.Contains("SubDistrict", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.SubDistrict) : query.ThenBy(x => x.SubDistrict);
                }

                if (item.Contains("CityCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CityCode) : query.ThenBy(x => x.CityCode);
                }

                if (item.Contains("PostCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.PostCode) : query.ThenBy(x => x.PostCode);
                }

                if (item.Contains("IsSameAddress", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.IsSameAddress) : query.ThenBy(x => x.IsSameAddress);
                }

                if (item.Contains("TaxAddressId", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.TaxAddressId) : query.ThenBy(x => x.TaxAddressId);
                }

                if (item.Contains("Default", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Default) : query.ThenBy(x => x.Default);
                }

                if (item.Contains("CreatedBy", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CreatedBy) : query.ThenBy(x => x.CreatedBy);
                }
                if (item.Contains("CreatedDate", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CreatedDate) : query.ThenBy(x => x.CreatedDate);
                }
                if (item.Contains("ModifiedBy", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ModifiedBy) : query.ThenBy(x => x.ModifiedBy);
                }
                if (item.Contains("ModifiedDate", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ModifiedDate) : query.ThenBy(x => x.ModifiedDate);
                }
            }

            if (!sortingList.Any())
            {
                query = query.ThenBy(x => x.AddressCode);
            }

            return query;
        }
    }
}
