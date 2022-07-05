using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Mediator;


namespace FLOG_BE.Features.Finance.DepositSettlement.PostDepositSettlement
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestDepositSettlement Body { get; set; }
    }

    public class RequestDepositSettlement
    {
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
        [NotMapped]
        public string VendorName { get; set; }
        public string Description { get; set; }
        public decimal OriginatingTotalPaid { get; set; }
        public decimal FunctionalTotalPaid { get; set; }
        public string CreatedBy { get; set; }
        [NotMapped]
        public string CreatedByName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        [NotMapped]
        public string ModifiedByName { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string VoidBy { get; set; }
        [NotMapped]
        public string VoidByName { get; set; }
        public DateTime? VoidDate { get; set; }
        public int Status { get; set; }
        public string StatusComment { get; set; }
        [NotMapped]
        public List<DepositSettlementDetail> DepositSettlementDetails { get; set; }
    }

    public class DepositSettlementDetail
    {

        public Guid SettlementHeaderId { get; set; }
        public Guid ReceiveTransactionId { get; set; }
        public string Description { get; set; }
        public decimal OriginatingBalance { get; set; }
        public decimal FunctionalBalance { get; set; }
        public decimal OriginatingPaid { get; set; }
        public decimal FunctionalPaid { get; set; }
        public int Status { get; set; }
    }
}
