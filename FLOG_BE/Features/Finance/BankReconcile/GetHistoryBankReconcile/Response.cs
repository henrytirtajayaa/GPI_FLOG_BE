using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Finance.BankReconcile.GetHistoryBankReconcile
{
    public class Response
    {
        public List<ResponseItem> Reconciles { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public Guid BankReconcileId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string DocumentNo { get; set; }
        public string CheckbookCode { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime BankCutoffStart { get; set; }
        public DateTime BankCutoffEnd { get; set; }
        public string Description { get; set; }
        public decimal BankEndingOrgBalance { get; set; }
        public decimal CheckbookEndingOrgBalance { get; set; }
        public decimal BalanceDifference { get; set; }
        public decimal PrevCheckbookBalance { get; set; }
        public string PrevReconcileDocNo { get; set; }
        public string PrevBankReconcileId { get; set; }
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
        public bool AllowVoid { get; set; }

        public List<ResponseReconcileDetail> ReconcileDetails { get; set; }
        public List<ResponseReconcileAdjustment> ReconcileAdjustments { get; set; }
    }

    public class ResponseReconcileDetail
    {
        public Guid TransactionId { get; set; }
        public string Modul { get; set; }
        public string DocumentNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public string DocumentType { get; set; }
        public string TransactionType { get; set; }
        public string PaidSubject { get; set; }
        public decimal OriginatingAmount { get; set; }
        public bool IsChecked { get; set; }
    }

    public class ResponseReconcileAdjustment
    {
        public Guid BankReconcileAdjustmentId { get; set; }
        public Guid CheckbookTransactionId { get; set; }
        public Guid TransactionDetailId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string DocumentType { get; set; }
        public string TransactionType { get; set; }
        public Guid ChargesId { get; set; }
        public string CurrencyCode { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool IsMultiply { get; set; }
        public string PaidSubject { get; set; }
        public string Description { get; set; }
        public decimal OriginatingAmount { get; set; }
        public bool IsDeleted { get; set; }
        public string ChargesCode { get; set; }
        public string ChargesDescription { get; set; }
    }
}
