using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace Infrastructure.Utils.Extentions
{
    public static class ActionLinkExtensions
    {
        public static string FirstCapitalCase(this string input)
        {
            var converted = input;
            if (!string.IsNullOrEmpty(input))
            {
                converted = char.ToUpper(input[0]) + input.Substring(1);
            }
            return converted;
        }

        public static string GetBaseLink(this IHttpContextAccessor httpContextAccessor)
        {
            var baseUrl = $"{GetForwardedHostHeader(httpContextAccessor).RemoveTrailingSlash()}";
            var protocol = GetForwardedProtoHeader(httpContextAccessor);
            return $"{protocol}://{baseUrl.RemoveTrailingSlash()}";
        }

        public static string GetSelfLink(this IHttpContextAccessor httpContextAccessor)
        {
            var urlPath = GetForwardedUriHeader(httpContextAccessor);
            return $"{GetBaseLink(httpContextAccessor)}{urlPath}{httpContextAccessor.HttpContext.Request.QueryString}";
        }

        public static string GetForwardedHostHeader(this IHttpContextAccessor httpContextAccessor)
        {
            StringValues stringValues;
            return httpContextAccessor.HttpContext.Request.Headers.TryGetValue("X-Forwarded-Host", out stringValues) ? stringValues.FirstOrDefault() : httpContextAccessor.HttpContext.Request.Host.ToString();
        }

        public static string GetForwardedProtoHeader(this IHttpContextAccessor httpContextAccessor)
        {
            StringValues stringValues;
            return httpContextAccessor.HttpContext.Request.Headers.TryGetValue("X-Forwarded-Proto", out stringValues) ? stringValues.FirstOrDefault() : httpContextAccessor.HttpContext.Request.Scheme.ToString();
        }

        public static string GetForwardedUriHeader(this IHttpContextAccessor httpContextAccessor)
        {
            StringValues stringValues;
            return httpContextAccessor.HttpContext.Request.Headers.TryGetValue("X-Forwarded-Uri", out stringValues) ? stringValues.FirstOrDefault() : httpContextAccessor.HttpContext.Request.Path.ToString();
        }
    }
}
