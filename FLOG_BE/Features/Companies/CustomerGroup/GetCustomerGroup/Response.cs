using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System.Collections.Generic;
using System;

namespace FLOG_BE.Features.Companies.CustomerGroup.GetCustomerGroup
{
    public class Response
    {
        public List<ResponseItem> CustomerGroups { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public Guid CustomerGroupId { get; set; }
        public string CustomerGroupCode { get; set; }
        public string CustomerGroupName { get; set; }
        public string PaymentTermCode { get; set; }
        public string ReceivableAccountNo { get; set; }
        public string AccruedReceivableAccountNo { get; set; }
        public bool Inactive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
