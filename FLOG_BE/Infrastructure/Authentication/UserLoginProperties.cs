using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Authentication
{
    public interface IUserLoginProperties
    {
        UserClaim UserClaim { get; set; }
        int? ID { get; set; }
    }
    public class UserLoginProperties : IUserLoginProperties
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public UserClaim UserClaim { get; set; }
        public int? ID { get; set; }

        public UserLoginProperties(IHttpContextAccessor httpContext, IConfiguration configuration)
        {
            this._httpContextAccessor = httpContext;
            this._configuration = configuration;
            Populate();
        }

        private void Populate()
        {
            var userTypeClaim = _httpContextAccessor.HttpContext.User.Claims.ToList().FirstOrDefault(x => x.Type.Equals(_configuration["Jwt:IssuerKey"], StringComparison.InvariantCultureIgnoreCase));
            if (userTypeClaim != null)
                UserClaim = EnumExtension.GetEnumFromDescription<UserClaim>(userTypeClaim.Value);
            var IdClaim = _httpContextAccessor.HttpContext.User.Claims.ToList().FirstOrDefault(x => x.Type.Equals("ID", StringComparison.InvariantCultureIgnoreCase));
            if (IdClaim != null)
            {
                if (int.TryParse(IdClaim.Value, out int id))
                    ID = id;
            }
        }
    }
}
