using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.Checkbook.PutTransaction
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestCheckbookUpdateBody Body { get; set; }
    }

    public class RequestCheckbookUpdateBody
    {
        public Guid CheckbookTransactionId { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public string BranchCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string CurrencyCode { get; set; }
        public decimal ExchangeRate { get; set; }
        public string CheckbookCode { get; set; }
        public bool IsVoid { get; set; }
        public string VoidDocumentNo { get; set; }
        public string PaidSubject { get; set; }
        public string SubjectCode { get; set; }
        public string Description { get; set; }
        public decimal OriginatingTotalAmount { get; set; }
        public decimal FunctionalTotalAmount { get; set; }
        public bool IsMultiply { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string VoidBy { get; set; }
        public DateTime? VoidDate { get; set; }
        public int Status { get; set; }
        public string StatusComment { get; set; }
        public List<RequestCheckbookDetail> RequestCheckbookDetails { get; set; }

    }

    public class RequestCheckbookDetail
    {
        public Guid ChargesId { get; set; }
        public string ChargesDescription { get; set; }
        public decimal OriginatingAmount { get; set; }
        public decimal FunctionalAmount { get; set; }
        public int Status { get; set; }
        public int RowIndex { get; set; }
    }
}
