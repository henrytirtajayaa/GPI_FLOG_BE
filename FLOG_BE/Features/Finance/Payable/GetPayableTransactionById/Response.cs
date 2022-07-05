using FLOG_BE.Model.Central.Entities;
using FLOG_BE.Model.Companies.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Finance.Payable.GetPayableTransactionById
{
    public class Response
    {
        public ReponsePayable PayableTransaction { get; set; }
        public ResponseCompanySetup CompanySetup { get; set; }
        public ResponseCompanyAddress CompanyAddress { get; set; }
        public List<PayableTransactionDetail> PayableTransactionDetails { get; set; }
        public ResponseCurrency Currency { get; set; }
    }

    public class ReponsePayable
    {
        public Guid PayableTransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string DocumentNo { get; set; }
        public string BranchCode { get; set; }
        public string CurrencyCode { get; set; }
        public Guid VendorId { get; set; }
        public string VendorName { get; set; }
        public string Description { get; set; }
        public string NsDocumentNo { get; set; }
        public decimal OriginatingExtendedAmount { get; set; }
        public decimal SubtotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public int Status { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public DateTime? ModifiedDate { get; set; }
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
    public class PayableTransactionDetail
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
