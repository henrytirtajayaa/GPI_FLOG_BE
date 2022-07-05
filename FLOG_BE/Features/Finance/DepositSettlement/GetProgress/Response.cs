using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Finance.DepositSettlement.GetProgress
{
    public class Response
    {
        public List<ResponseItem> DepositSettlement { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public Guid SettlementHeaderId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public Guid ReceiveTransactionId { get; set; }
        public string DepositNo { get; set; }
        public string CurrencyCode { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool IsMultiply { get; set; }
        public string CheckbookCode { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Description { get; set; }
        public decimal OriginatingTotalPaid { get; set; }
        public decimal FunctionalTotalPaid { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string VoidBy { get; set; }
        public string VoidByName { get; set; }
        public DateTime? VoidDate { get; set; }
        public int Status { get; set; }
        public string StatusComment { get; set; }

        public List<DepositSettlementDetail> DepositSettlementDetails { get; set; }
        public decimal AppliedTotalPaid { get; set; }
    }

    public class DepositSettlementDetail
    {
        public Guid SettlementDetailId { get; set; }
        public Guid SettlementHeaderId { get; set; }
        public Guid ReceiveTransactionId { get; set; }
        public string Description { get; set; }
        public decimal OriginatingBalance { get; set; }
        public decimal FunctionalBalance { get; set; }
        public decimal OriginatingPaid { get; set; }
        public decimal FunctionalPaid { get; set; }
        public int Status { get; set; }
        public string DocumentNo { get; set; }
        public string CustomerName { get; set; }
        public string NsDocumentNo { get; set; }
        public decimal OrgDocAmount { get; set; }
    }
}
