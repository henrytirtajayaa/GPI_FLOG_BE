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

namespace FLOG_BE.Features.Companies.NumberFormatDetail.GetNumberFormatDetail
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

            var query = getNumberFormatDetail(request.Initiator.UserId, request.Filter);
            query = getNumberFormatDetailSorted(query, request.Sort);

            var list = await PaginatedList<Entities.NumberFormatDetail, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            return ApiResult<Response>.Ok(new Response()
            {
                NumberFormatDetails = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.NumberFormatDetail> getNumberFormatDetail(string personId, RequestFilter filter)
        {
            var query = _context.NumberFormatDetails
                     .AsQueryable();

            var filterFormatHeaderId = filter.FormatHeaderId?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterFormatHeaderId.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NumberFormatDetail>(true);
                foreach (var filterItem in filterFormatHeaderId)
                {
                    predicate = predicate.Or(x => x.FormatHeaderId.ToString().Contains(filterItem));
                }
                query = query.Where(predicate);
            }

            var filterMaskFormat = filter.MaskFormat?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterMaskFormat.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NumberFormatDetail>(true);
                foreach (var filterItem in filterMaskFormat)
                {
                    predicate = predicate.Or(x => x.MaskFormat.Contains(filterItem));
                }
                query = query.Where(predicate);
            }
            var filterSegmentNoMin = filter.SegmentNoMin?.Where(x => x.HasValue).ToList();
            if (filterSegmentNoMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NumberFormatDetail>(true);
                foreach (var filterItem in filterSegmentNoMin)
                {
                    predicate = predicate.Or(x => x.SegmentNo >= filterItem);
                }
                query = query.Where(predicate);
            }
            var filterSegmentNoMax = filter.SegmentNoMax?.Where(x => x.HasValue).ToList();
            if (filterSegmentNoMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NumberFormatDetail>(true);
                foreach (var filterItem in filterSegmentNoMax)
                {
                    predicate = predicate.Or(x => x.SegmentNo <= filterItem);
                }
                query = query.Where(predicate);
            }
            var filterSegmentTypeMin = filter.SegmentTypeMin?.Where(x => x.HasValue).ToList();
            if (filterSegmentTypeMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NumberFormatDetail>(true);
                foreach (var filterItem in filterSegmentTypeMin)
                {
                    predicate = predicate.Or(x => x.SegmentType >= filterItem);
                }
                query = query.Where(predicate);
            }
           
            var filterSegmentTypeMax = filter.SegmentTypeMax?.Where(x => x.HasValue).ToList();
            if (filterSegmentTypeMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NumberFormatDetail>(true);
                foreach (var filterItem in filterSegmentTypeMax)
                {
                    predicate = predicate.Or(x => x.SegmentType <= filterItem);
                }
                query = query.Where(predicate);
            }
            
            var filterSegmentLengthMin = filter.SegmentLengthMin?.Where(x => x.HasValue).ToList();
            if (filterSegmentLengthMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NumberFormatDetail>(true);
                foreach (var filterItem in filterSegmentLengthMin)
                {
                    predicate = predicate.Or(x => x.SegmentLength >= filterItem);
                }
                query = query.Where(predicate);
            }
            var filterSegmentLengthMax = filter.SegmentLengthMax?.Where(x => x.HasValue).ToList();
            if (filterSegmentLengthMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NumberFormatDetail>(true);
                foreach (var filterItem in filterSegmentLengthMax)
                {
                    predicate = predicate.Or(x => x.SegmentLength <= filterItem);
                }
                query = query.Where(predicate);
            }
            
            var filterStartingValueMin = filter.StartingValueMin?.Where(x => x.HasValue).ToList();
            if (filterStartingValueMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NumberFormatDetail>(true);
                foreach (var filterItem in filterStartingValueMin)
                {
                    predicate = predicate.Or(x => x.StartingValue >= filterItem);
                }
                query = query.Where(predicate);
            }
            var filterStartingValueMax = filter.StartingValueMax?.Where(x => x.HasValue).ToList();
            if (filterStartingValueMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NumberFormatDetail>(true);
                foreach (var filterItem in filterStartingValueMax)
                {
                    predicate = predicate.Or(x => x.StartingValue <= filterItem);
                }
                query = query.Where(predicate);
            }
            
            var filterEndingValueMin = filter.EndingValueMin?.Where(x => x.HasValue).ToList();
            if (filterEndingValueMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NumberFormatDetail>(true);
                foreach (var filterItem in filterEndingValueMin)
                {
                    predicate = predicate.Or(x => x.EndingValue >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterEndingValueMax = filter.EndingValueMax?.Where(x => x.HasValue).ToList();
            if (filterEndingValueMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.NumberFormatDetail>(true);
                foreach (var filterItem in filterEndingValueMax)
                {
                    predicate = predicate.Or(x => x.EndingValue <= filterItem);
                }
                query = query.Where(predicate);
            }
            
            if (filter.Increase.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.NumberFormatDetail>(true);
                    predicate = predicate.Or(x => x.Increase == filter.Increase);
                query = query.Where(predicate);
            }


            return query;
        }

        private IQueryable<Entities.NumberFormatDetail> getNumberFormatDetailSorted(IQueryable<Entities.NumberFormatDetail> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("FormatHeaderId", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.FormatHeaderId) : query.ThenBy(x => x.FormatHeaderId);
                }

                if (item.Contains("MaskFormat", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.MaskFormat) : query.ThenBy(x => x.MaskFormat);
                }
                if (item.Contains("SegmentNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.SegmentNo) : query.ThenBy(x => x.SegmentNo);
                }
                if (item.Contains("SegmentType", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.SegmentType) : query.ThenBy(x => x.SegmentType);
                }
                if (item.Contains("SegmentLength", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.SegmentLength) : query.ThenBy(x => x.SegmentLength);
                }
                if (item.Contains("StartingValue", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.StartingValue) : query.ThenBy(x => x.StartingValue);
                }
                if (item.Contains("EndingValue", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.EndingValue) : query.ThenBy(x => x.EndingValue);
                }
                if (item.Contains("Increase", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Increase) : query.ThenBy(x => x.Increase);
                }
               



            }

            if (!sortingList.Any(x => x.Contains("SegmentNo", StringComparison.InvariantCultureIgnoreCase)))
            {
                query = query.ThenBy(x => x.SegmentNo);
            }

            return query;
        }
    }
}
