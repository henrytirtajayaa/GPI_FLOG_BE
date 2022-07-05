using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLOG_BE.Features.Finance.BankReconcile.PutBankReconcile
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestReconcileBody Body { get; set; }
    }

    public class RequestReconcileBody
    {
        public Guid BankReconcileId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string CheckbookCode { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime BankCutoffStart { get; set; }
        public DateTime BankCutoffEnd { get; set; }
        public Guid PrevBankReconcileId { get; set; }
        public string Description { get; set; }
        public decimal BankEndingOrgBalance { get; set; }
        public decimal CheckbookEndingOrgBalance { get; set; }
        public List<RequestReconcileDetail> ReconcileDetails { get; set; }
        public List<RequestReconcileAdjustment> ReconcileAdjustments { get; set; }
    }

    public class RequestReconcileDetail
    {
        public DateTime TransactionDate { get; set; }
        public Guid TransactionId { get; set; }
        public string Modul { get; set; }
        
        public bool IsChecked { get; set; }
        public int Status { get; set; }
    }

    public class RequestReconcileAdjustment
    {
        public Guid CheckbookTransactionId { get; set; }
        public Guid TransactionDetailId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string DocumentType { get; set; }
        public Guid ChargesId { get; set; }
        public string CurrencyCode { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool IsMultiply { get; set; }
        public string PaidSubject { get; set; }
        public string Description { get; set; }
        public decimal OriginatingAmount { get; set; }
        public bool IsDeleted { get; set; }
    }
}
