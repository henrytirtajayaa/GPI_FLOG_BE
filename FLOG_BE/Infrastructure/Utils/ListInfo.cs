using Infrastructure.Utils.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Infrastructure.Utils
{
    public class ListInfo //: ActionLinks
    {
        private readonly HATEOASLinkCollection _linkCollection;

        public int Count { get; set; } = 0;
        public int TotalCount { get; set; } = 0;
        public string HasMore { get; set; } = "No";

        private IEnumerable<HATEOASLink> _links;
        public IEnumerable<HATEOASLink> Links
        {
            get
            {
                return _links == null ? _linkCollection?.Links : _links;
            }
            set
            {
                if (_linkCollection != null)
                    throw new Exception("pleas use HATEOASLinkCollection to build youre links");

                _links = value;
            }
        }

        /// <summary>
        /// watning olny used for deserialising
        /// </summary>
        public ListInfo()
        {
        }

        public ListInfo(HATEOASLinkCollection linkCollection)
        {
            _linkCollection = linkCollection.CreateNewLinks();
        }

        internal void BuildLinks(int limit, int offset)
        {
            if (limit == 0)
                return;

            var path = _linkCollection.RelativePath +
                       (!_linkCollection.RelativePath.Contains("?") && !_linkCollection.RelativePath.EndsWith("/") ? "/" : "") +
                       (_linkCollection.RelativePath.Contains("?") ? "" : "?");

            if (!path.Contains("offset="))
                path = $"{path}&offset=0";

            if (!path.Contains("limit="))
                path = $"{path}&limit={limit}";

            _linkCollection.AddLink(new HATEOASLink
            {
                Rel = "First",
                Href = ConstructUrl(path, 0, limit),
                Type = "GET"
            });

            if (TotalCount > limit && offset >= limit)
            {
                var previousOffset = offset - limit >= 0 ? offset - limit : 0;

                _linkCollection.AddLink(new HATEOASLink
                {
                    Rel = "Previous",
                    Href = ConstructUrl(path, previousOffset, limit),
                    Type = "GET"
                });
            }

            var nextOffset = offset + limit;
            if (TotalCount > nextOffset)
            {
                _linkCollection.AddLink(new HATEOASLink
                {
                    Rel = "Next",
                    Href = ConstructUrl(path, nextOffset, limit),
                    Type = "GET"
                });
            }

            var lastOffset = TotalCount == 0 ? 0 : TotalCount % limit > 0 ? TotalCount - (TotalCount % limit) : TotalCount - limit;
            _linkCollection.AddLink(new HATEOASLink
            {
                Rel = "Last",
                Href = ConstructUrl(path, lastOffset, limit),
                Type = "GET"
            });
        }

        private string ConstructUrl(string path, int offset, int limit)
        {
            path = Regex.Replace(path, "offset=?\\d*", $"offset={offset}");
            path = Regex.Replace(path, "limit=?\\d*", $"limit={limit}");
            return path;
        }

        internal HATEOASLink First()
            => Links.GetLink("First");

        internal HATEOASLink Next()
            => Links.GetLink("Next");

        internal HATEOASLink Previous()
            => Links.GetLink("Previous");

        internal HATEOASLink Last()
            => Links.GetLink("Last");
    }

    public class ActionLinks
    {
        public Dictionary<string, string> Links { get; set; } = new Dictionary<string, string>();
    }
    public class HATEOASLinkResponds
    {
        public IEnumerable<HATEOASLink> Links;
    }
}
