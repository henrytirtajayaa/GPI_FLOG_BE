using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.Bank.PutBank
{
    public class Response
    {
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string Address { get; set; }
        public string CityCode { get; set; }
        public bool Inactive { get; set; }
    }


}
