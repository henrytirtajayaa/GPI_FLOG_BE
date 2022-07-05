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
using FLOG_BE.Model.Central.Entities;
using AutoMapper;

namespace FLOG_BE.Features.Companies.ApprovalSetupDetail.GetApprovalSetupDetail
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FlogContext _contextCentral;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;
        private readonly IMapper _mapper;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, FlogContext contextCentral, ILogin login, HATEOASLinkCollection linkCollection, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _contextCentral = contextCentral;
            _login = login;
            _mapper = mapper;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            if (string.IsNullOrEmpty(request.Initiator.UserId))
                return ApiResult<Response>.Unauthorized();

            var query = getApprovalSetupDetail(request.Filter);
            query = getApprovalSetupDetailSorted(query, request.Sort);

            var items = query.ToList();

            var response = new Response();
            response.ApprovalSetupDetail = new List<ResponseItem>();

            foreach (var item in items)
            {
                response.ApprovalSetupDetail.Add(new ResponseItem
                {
                    ApprovalSetupDetailId = item.ApprovalSetupDetailId,
                    ApprovalSetupHeaderId = item.ApprovalSetupHeaderId,
                    PersonId = item.PersonId,
                    PersonName = item.PersonId == null ? item.UserGroupName : item.PersonFullName,
                    Id = (item.PersonId != null ? item.PersonEmail : item.UserGroupCode),
                    PersonCategoryId = item.PersonCategoryId,
                    Description = item.Description,
                    Level = item.Level,
                    HasLimit = item.HasLimit,
                    ApprovalLimit = item.ApprovalLimit,
                    Status = item.Status,
                });
            }

            return ApiResult<Response>.Ok(new Response()
            {
                ApprovalSetupDetail = response.ApprovalSetupDetail,
                ListInfo = null
            });
        }

        private IQueryable<Entities.ApprovalSetupDetail> getApprovalSetupDetail(RequestFilter filter)
        {
            List<Person> persons = _contextCentral.Persons.ToList();
            List<PersonCategory> groups = _contextCentral.PersonCategories.ToList();

            IQueryable<Entities.ApprovalSetupDetail> query = (from det in _context.ApprovalSetupDetails
                                                              where det.ApprovalSetupHeaderId == filter.ApprovalSetupHeaderId
                                                              select new Entities.ApprovalSetupDetail
                                                              {
                                                                  ApprovalSetupDetailId = det.ApprovalSetupDetailId,
                                                                  ApprovalSetupHeaderId = det.ApprovalSetupHeaderId,
                                                                  Description = det.Description,
                                                                  PersonId = det.PersonId,
                                                                  PersonCategoryId = det.PersonCategoryId,
                                                                  Level = det.Level,
                                                                  HasLimit = det.HasLimit,
                                                                  ApprovalLimit = det.ApprovalLimit,
                                                                  Status = det.Status,
                                                                  CreatedBy = det.CreatedBy,
                                                                  CreatedDate = det.CreatedDate,
                                                                  PersonEmail = (det.PersonId != null ? persons.Where(x=>x.PersonId.Contains(det.PersonId.ToString(), StringComparison.OrdinalIgnoreCase)).Select(s=>s.EmailAddress).FirstOrDefault() : ""),
                                                                  PersonFullName = (det.PersonId != null ? persons.Where(x => x.PersonId.Contains(det.PersonId.ToString(), StringComparison.OrdinalIgnoreCase)).Select(s => s.PersonFullName).FirstOrDefault() : ""),
                                                                  UserGroupCode = (det.PersonCategoryId != null ? groups.Where(x => x.PersonCategoryId.Contains(det.PersonCategoryId.ToString(), StringComparison.OrdinalIgnoreCase)).Select(s => s.PersonCategoryCode).FirstOrDefault() : ""),
                                                                  UserGroupName = (det.PersonCategoryId != null ? groups.Where(x => x.PersonCategoryId.Contains(det.PersonCategoryId.ToString(), StringComparison.OrdinalIgnoreCase)).Select(s => s.PersonCategoryName).FirstOrDefault() : "")
                                                              });
            var filterDescription = filter.Description?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterDescription.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ApprovalSetupDetail>(true);
                foreach (var filterItem in filterDescription)
                {
                    predicate = predicate.Or(x => x.Description.Contains(filterItem));
                }
                query = query.Where(predicate);
            }

            var filterStatus = filter.Status?.Where(x => x.HasValue).ToList();
            if (filterStatus.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ApprovalSetupDetail>(true);
                foreach (var filterItem in filterStatus)
                {
                    predicate = predicate.Or(x => x.Status == filterItem);
                }
                query = query.Where(predicate);
            }

            var filterApprovalLimit = filter.ApprovalLimit?.Where(x => x.HasValue).ToList();
            if (filterApprovalLimit.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ApprovalSetupDetail>(true);
                foreach (var filterItem in filterApprovalLimit)
                {
                    predicate = predicate.Or(x => x.ApprovalLimit == filterItem);
                }
                query = query.Where(predicate);
            }

            return query;
        }

        private IQueryable<Entities.ApprovalSetupDetail> getApprovalSetupDetailSorted(IQueryable<Entities.ApprovalSetupDetail> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            
            if (!sortingList.Any(x => x.Contains("Level", StringComparison.InvariantCultureIgnoreCase)))
            {
                query = query.ThenBy(x => x.Level);
            }

            return query;
        }


    }
}
