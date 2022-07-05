using System;
using CryptSharp;
using Microsoft.Extensions.Configuration;
using FLOG_BE.Model.Central.Entities;
using System.Security.Claims;

namespace Infrastructure.Authentication
{
    public interface ILogin
    {
        string CreateToken(UserClaim userClaim, Person users);
        string CreateTokenWithCompany(UserClaim userClaim, Person users, CompanySecurity companyId);
        bool PasswordIsMatch(string password, string hash);
        string Encrypt(string password);
    }

    public class Login : ILogin
    {
        private readonly IConfiguration _config;
        public Login(IConfiguration config)
        {
            this._config = config;
        }

        public string CreateToken(UserClaim userClaim, Person users)
        {
            var tokenBuilder = new JwtTokenBuilder()
                                .AddSecurityKey(JwtSecurityKey.Create(_config["Jwt:Key"]))
                                .AddSubject(users.EmailAddress)
                                .AddIssuer(_config["Jwt:Issuer"])
                                .AddAudience(_config["Jwt:Issuer"])
                                .AddClaim("UserClaim", userClaim.GetEnumDescription())
                                .AddClaim(ClaimTypes.Name, users.PersonId)
                                .AddClaim(ClaimTypes.Email, users.EmailAddress)
                                .AddExpiry(10);

            return tokenBuilder.Build().Value;
        }

        public string CreateTokenWithCompany(UserClaim userClaim, Person users, CompanySecurity companySecurity)
        {
            var tokenBuilder = new JwtTokenBuilder()
                                .AddSecurityKey(JwtSecurityKey.Create(_config["Jwt:Key"]))
                                .AddSubject(users.EmailAddress)
                                .AddIssuer(_config["Jwt:Issuer"])
                                .AddAudience(_config["Jwt:Issuer"])
                                .AddClaim("UserClaim", userClaim.GetEnumDescription())
                                .AddClaim(ClaimTypes.Name, users.PersonId)
                                .AddClaim(ClaimTypes.Email, users.EmailAddress)
                                .AddClaim(ClaimTypes.Role, companySecurity.SecurityRoleId)
                                .AddClaim("CompanyId", companySecurity.CompanyId)
                                .AddExpiry(int.Parse(_config["Jwt:ExpiryInMinutes"]));

            return tokenBuilder.Build().Value;
        }

        public bool PasswordIsMatch(string password, string hash)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            return (Crypter.CheckPassword(password, hash));
        }

        public string Encrypt(string password)
        {
            var result = Crypter.Blowfish.Crypt(password);

            if (string.IsNullOrEmpty(password))
                return "Password cannot be null!";

            return result;
        }
    }
}