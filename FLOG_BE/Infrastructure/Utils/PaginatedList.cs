using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Utils
{
    public class PaginatedList<T, U> : List<U> where T : class
    {
        private readonly HATEOASLinkCollection _linkCollection;
        private static IMapper _mapper;

        public int Offset { get; }
        public int Limit { get; }
        public int Total { get; }
        public ListInfo ListInfo { get; private set; }

        public PaginatedList(List<T> source, int total, int offset, int limit, HATEOASLinkCollection linkCollection)
        {
            //if(_mapper == null)
            //{
            //    _mapper = Mapper.Instance;
            //}

            Offset = offset; //Start position
            Limit = limit; //Amount of records to return
            Total = total; //Total records
            _linkCollection = linkCollection;

            //TotalPages = (int)Math.Ceiling(count / (double)limit);

            //Same type no mapping needed
            if (typeof(T) == typeof(U))
            {
                if (source != null)
                {
                    AddRange((IEnumerable<U>)source);
                    CreateActionLinks();
                    return;
                }
            }
            //Dest != source, Mapping (automapper) profile needs te exist
            var retList = new List<U>();
            foreach (var item in source)
            {
                if (_mapper != null)
                    retList.Add(_mapper.Map<U>(item));
                else
                    retList.Add(Mapper.Map<U>(item));
            }
            AddRange(retList);
            CreateActionLinks();
        }

        private void CreateActionLinks()
        {
            //CHECK IF HATEOAS MUST BE IMPLEMENTED HERE 
            ListInfo = new ListInfo(_linkCollection)
            {
                Count = Count,
                HasMore = Count < Total ? "Yes" : "No",
                TotalCount = Total
            };

            ListInfo.BuildLinks(Limit, Offset);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="source">When the query has inner joins, use GetCountFromInnerJoin first to retrieve the correct amount of results</param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="countFromInnerJoin"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        public static async Task<PaginatedList<T, U>> CreateAsync(HATEOASLinkCollection linkCollection, IQueryable<T> source, int offset, int limit, int? countFromInnerJoin, IMapper mapper = null)
        {
            _mapper = mapper;

            //Use defaults value according to generic specs 
            if (limit < 0) limit = 100;
            if (offset < 0) offset = 0;

            int count;
            //BEWARE: the results can be incorrect, the CountAsync query has a bug, not using the inner join (https://github.com/aspnet/EntityFrameworkCore/issues/8201)
            if (countFromInnerJoin.HasValue)
            {
                count = countFromInnerJoin.Value;
            }
            else count = await source.CountAsync();

            if (count == 0)
                return new PaginatedList<T, U>(new List<T>(), 0, 0, limit, linkCollection);

            List<T> items = new List<T>();
            if (limit > 0 && offset < count)
                items = await Task.FromResult(source.Skip(offset).Take(limit).ToList());
            return new PaginatedList<T, U>(items, count, offset, limit, linkCollection);
        }

        /// <summary>
        /// Put an already existing list in the paginated form
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="source"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static PaginatedList<T, U> UseList(HATEOASLinkCollection linkCollection, IList<T> source, int offset, int limit, int count, IMapper mapper = null)
        {
            _mapper = mapper;

            //Use defaults value according to generic specs 
            if (limit < 0) limit = 100;
            if (offset < 0) offset = 0;

            //no items, make empty list
            if (count == 0)
                return new PaginatedList<T, U>(new List<T>(), count, 0, 0, linkCollection);

            return new PaginatedList<T, U>(source.ToList(), count, offset, limit, linkCollection);
        }
    }
}
