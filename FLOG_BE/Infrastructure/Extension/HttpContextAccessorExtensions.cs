using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Infrastructure.Mediator;

public static class HttpContextAccessorExtensions
{
    public static UserLogin GetInitiator(this IHttpContextAccessor httpContextAccessor)
    {
        try
        {
            return new UserLogin()
            {
                UserId = httpContextAccessor.HttpContext.User.Identity.Name ?? "",
                SecurityId = httpContextAccessor.HttpContext.User.Claims.Any(x => x.Type == ClaimTypes.Role) ? httpContextAccessor.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Role).Value : "",
                CompanyId = httpContextAccessor.HttpContext.User.Claims.Any(x => x.Type.Equals("CompanyId", StringComparison.OrdinalIgnoreCase)) ? httpContextAccessor.HttpContext.User.Claims.First(x => x.Type.Equals("CompanyId", StringComparison.OrdinalIgnoreCase)).Value : "",
            };

        }
        catch
        {
            return null;
        }
    }
}

