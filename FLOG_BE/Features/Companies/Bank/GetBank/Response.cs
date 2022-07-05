using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.Bank.GetBank
{
    public class Response
    {
        public List<ResponseItem> Banks { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public string BankId { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string Address { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string Province { get; set; }
        public string CountryName { get; set; }
        public bool InActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
  
}
