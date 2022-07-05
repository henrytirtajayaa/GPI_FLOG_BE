using Infrastructure.Utils.Extentions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Utils
{
    public class HATEOASLink
    {
        private string _rel = "";
        private string _href;

        public string Rel
        {
            get => _rel;
            set => _rel = value.FirstCapitalCase();
        }

        public string Href
        {
            get => _href;
            set => _href = value.ToLowerInvariant();
        }

        public string Type { get; set; }
    }

    public class HATEOASLinkCollection
    {
        private string baseUrl;
        private string selfLink;
        private string selfMethod;
        private List<HATEOASLink> _links = new List<HATEOASLink>();

        private string CleanUrl()
        {
            var url = selfLink.Replace(baseUrl, "");
            url = url.TrimEnd('&');
            return url;
        }

        public string RelativePath { get => CleanUrl(); private set { } }
        public IEnumerable<HATEOASLink> Links => _links;

        public HATEOASLinkCollection(IHttpContextAccessor httpContextAccessor)
        {
            baseUrl = httpContextAccessor.GetBaseLink().ToLowerInvariant();
            selfLink = httpContextAccessor.GetSelfLink().ToLowerInvariant();
            selfMethod = httpContextAccessor.HttpContext.Request.Method;
        }

        private HATEOASLinkCollection(string baseUrl, string selfLink, string selfMethod)
        {
            this.baseUrl = baseUrl.ToLowerInvariant();
            this.selfLink = selfLink.ToLowerInvariant();
            this.selfMethod = selfMethod;
        }

        //create new links
        public HATEOASLinkCollection CreateNewLinks()
        {
            return new HATEOASLinkCollection(baseUrl, selfLink, selfMethod);
        }

        public void AddSelfLink()
        {
            _links.Add(new HATEOASLink
            {
                Href = selfLink,
                Type = selfMethod,
                Rel = "Self"
            });
        }

        public void AddLink(HATEOASLink link)
        {
            if (!string.IsNullOrEmpty(baseUrl))
                link = AddBaseUrl(link);

            _links.Add(link);
        }

        private HATEOASLink AddBaseUrl(HATEOASLink link)
        {
            if (!string.IsNullOrEmpty(link.Href))
                link.Href = baseUrl + link.Href;

            return link;
        }
    }
}
