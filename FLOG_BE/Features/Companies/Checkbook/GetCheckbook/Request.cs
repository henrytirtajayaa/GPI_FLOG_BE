using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.Checkbook.GetCheckbook
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public string SecuritySetting { get; set; }
        public RequestFilter Filter { get; set; }
        public Int32 Offset { get; set; }
        public Int32 Limit { get; set; }
        public List<string> Sort { get; set; }
    }

    public class RequestFilter
    {
        public List<string> CheckbookCode { get; set; }
        public List<string> CheckbookName { get; set; }
        public List<string> CheckbookAccountNo { get; set; }
        public List<string> CurrencyCode { get; set; }
        public List<string> BankCode { get; set; }
        public List<string> BankAccountCode { get; set; }
        public bool? HasCheckoutApproval { get; set; }
        public List<string> ApprovalCode { get; set; }
        public List<string> CheckbookInDocNo { get; set; }
        public List<string> CheckbookOutDocNo { get; set; }
        public List<string> ReceiptDocNo { get; set; }
        public List<string> PaymentDocNo { get; set; }
        public List<string> ReconcileDocNo { get; set; }
        public bool? IsCash { get; set; }
        public bool? InActive { get; set; }
        public List<string> CreatedBy { get; set; }
        public List<DateTime?> CreatedDateStart { get; set; }
        public List<DateTime?> CreatedDateEnd { get; set; }
        public List<string> ModifiedBy { get; set; }
        public List<DateTime?> ModifiedDateStart { get; set; }
        public List<DateTime?> ModifiedDateEnd { get; set; }
    }
}
