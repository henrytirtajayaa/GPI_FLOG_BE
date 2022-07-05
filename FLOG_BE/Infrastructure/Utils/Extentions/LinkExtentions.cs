using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Utils.Extentions
{
    public static class LinkExtentions
    {
        public static HATEOASLink GetLink(this IEnumerable<HATEOASLink> links, string rel)
            => links
                .FirstOrDefault(link => link.Rel.Equals(rel, StringComparison.InvariantCultureIgnoreCase));

        public static string GetUrl(this IEnumerable<HATEOASLink> links, string rel)
            => links
                .Where(link => link.Rel.Equals(rel, StringComparison.InvariantCultureIgnoreCase))
                .Select(link => link.Href)
                .FirstOrDefault();
    }
}
