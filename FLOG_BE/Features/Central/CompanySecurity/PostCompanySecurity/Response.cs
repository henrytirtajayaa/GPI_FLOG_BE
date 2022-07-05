using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System.Collections.Generic;

namespace FLOG_BE.Features.Central.CompanySecurity.PostCompanySecurity
{
    public class Response
    {
        public string CompanySecurityId { get; set; }
        public string UserId { get; set; }
        public string CompanyId { get; set; }
        public string RoleId { get; set; }
    }


}
