using FLOG_BE.Model.Central.Entities;
using FLOG_BE.Model.Companies.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Finance.Receivable.GetReceivableTransactionById
{
    public class Response
    {
        public ReponseReceive ReceiveTransaction { get; set; }
        public ResponseCompanySetup CompanySetup { get; set; }
        public ResponseCompanyAddress CompanyAddress { get; set; }
        public List<ReceivableTransactionDetail> ReceiveTransactionDetail { get; set; }
        public ResponseCurrency Currency { get; set; }
    }

    public class ReponseReceive
    {
        public Guid ReceiveTransactionId { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public string BranchCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CurrencyCode { get; set; }
        public string SoDocumentNo { get; set; }
        public string NsDocumentNo { get; set; }
        public string AddressBillToAddress { get; set; }
        public string CityBillToAddress { get; set; }
        public string Description { get; set; }
        public decimal OriginatingExtendedAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal SubtotalAmount { get; set; }
    }
    public class ResponseCompanySetup
    {
        public Guid CompanySetupId { get; set; }
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public Guid CompanyAddressId { get; set; }
        public string LogoImageUrl { get; set; }
    }
    public class ResponseCompanyAddress
    {
        public Guid CompanyAddressId { get; set; }
        public string AddressCode { get; set; }
        public string AddressName { get; set; }
        public string Address { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Fax { get; set; }
    }
    public class ReceivableTransactionDetail
    {
        public Guid TransactionDetailId { get; set; }
        public Guid ChargesId { get; set; }
        public string ChargesName { get; set; }
        public decimal OriginatingExtendedAmount { get; set; }
    }
    public class ResponseCurrency
    { 
        public Guid CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyUnit { get; set; }
        public string CurrencySubUnit { get; set; }
    }
}
