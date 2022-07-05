using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Finance.Receivable.GetHistoryReceivable
{
    public class Response
    {
        public List<ReponseItem> ReceiveTransaction { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ReponseItem
    {
        public Guid ReceiveTransactionId { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public string BranchCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string CurrencyCode { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool IsMultiply { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public decimal OriginatingExtendedAmount { get; set; }
        public string PaymentTermCode { get; set; }
        public string CustomerAddressCode { get; set; }
        public string SoDocumentNo { get; set; }
        public string NsDocumentNo { get; set; }
        public string MasterNo { get; set; }
        public string AgreementNo { get; set; }
        public string Description { get; set; }
        public decimal SubtotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string ModifiedByName { get; set; }
        public string VoidBy { get; set; }
        public string VoidByName { get; set; }
        public DateTime? VoidDate { get; set; }
        public int Status { get; set; }
        public string StatusComment { get; set; }
        public string BillToAddressCode { get; set; }
        public string ShipToAddressCode { get; set; }
        public List<ReceivableTransactionDetail> ReceivableTransactionDetails { get; set; }
        public List<RequestReceivableTax> ReceivableTransactionTaxes { get; set; }

        public int DecimalPlaces { get; set; }
    }
    public class ReceivableTransactionDetail
    {
        public Guid TransactionDetailId { get; set; }
        public Guid ReceiveTransactionId { get; set; }
        public Guid ChargesId { get; set; }
        public string ChargesCode { get; set; }
        public string ChargesName { get; set; }
        public string ChargesDescription { get; set; }
        public decimal OriginatingAmount { get; set; }
        public decimal OriginatingTax { get; set; }
        public decimal OriginatingDiscount { get; set; }
        public decimal OriginatingExtendedAmount { get; set; }
        public decimal FunctionalTax { get; set; }
        public decimal FunctionalDiscount { get; set; }
        public decimal FunctionalExtendedAmount { get; set; }
        public Guid TaxScheduleId { get; set; }
        public bool IsTaxAfterDiscount { get; set; }
        public decimal PercentDiscount { get; set; }
        public string TaxScheduleCode { get; set; }
        public decimal TaxablePercentTax { get; set; }
        public int Status { get; set; }
    }

    public class RequestReceivableTax
    {
        public Guid ReceiveTransactionId { get; set; }
        public Guid TaxScheduleId { get; set; }
        public bool IsTaxAfterDiscount { get; set; }
        public string TaxScheduleCode { get; set; }
        public decimal TaxBasePercent { get; set; }
        public decimal TaxBaseOriginatingAmount { get; set; }
        public decimal TaxablePercent { get; set; }
        public decimal OriginatingTaxAmount { get; set; }
        public int Status { get; set; }
    }
}
