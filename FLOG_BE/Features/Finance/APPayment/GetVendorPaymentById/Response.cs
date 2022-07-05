using FLOG_BE.Model.Central.Entities;
using FLOG_BE.Model.Companies.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Finance.ApPayment.GetVendorPaymentById
{
    public class Response
    {
        public ReponsePayment VendorPayment { get; set; }
        public ResponseCompanySetup CompanySetup { get; set; }
        public ResponseCompanyAddress CompanyAddress { get; set; }
        public List<PaymentDetail> ApPaymentDetails { get; set; }
        public ResponseCurrency Currency { get; set; }
    }

    public class ReponsePayment
    {
        public Guid PaymentHeaderId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string DocumentNo { get; set; }
        public string CurrencyCode { get; set; }
        public Guid VendorId { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string Description { get; set; }
        public decimal OriginatingTotalPaid { get; set; }
        public decimal FunctionalTotalPaid { get; set; }
        public decimal AppliedTotalPaid { get; set; }
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
    public class PaymentDetail
    {
        public Guid PayableTransactionId { get; set; }
        public string DocumentNo { get; set; }
        public decimal OriginatingPaid { get; set; }
        public string NsDocumentNo { get; set; }
        public string MasterNo { get; set; }
        public string AgreementNo { get; set; }
    }
    public class ResponseCurrency
    {
        public Guid CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyUnit { get; set; }
        public string CurrencySubUnit { get; set; }
    }
}
