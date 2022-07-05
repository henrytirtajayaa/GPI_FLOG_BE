using FLOG_BE.Model.Central.Entities;
using FLOG_BE.Model.Companies.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Finance.Checkbook.GetCheckbookTransactionById
{
    public class Response
    {
        public ReponseCheckbook CheckbookTransaction { get; set; }
        public ResponseCompanySetup CompanySetup { get; set; }
        public ResponseCompanyAddress CompanyAddress { get; set; }
        public List<CheckbookDetail> CheckbookDetails { get; set; }
        public ResponseCurrency Currency { get; set; }
    }

    public class ReponseCheckbook
    {
        public Guid CheckbookTransactionId { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public string BranchCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string CurrencyCode { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool IsMultiply { get; set; }
        public string CheckbookCode { get; set; }
        public string CheckbookName { get; set; }
        public string ApprovalCode { get; set; }
        public bool IsVoid { get; set; }
        public string VoidDocumentNo { get; set; }
        public string PaidSubject { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public string BankAccountCode { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string Description { get; set; }
        public decimal OriginatingTotalAmount { get; set; }
        public decimal FunctionalTotalAmount { get; set; }
        public decimal AppliedTotalPaid { get; set; }
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
    public class CheckbookDetail
    {
        public Guid TransactionDetailId { get; set; }
        public string ChargesName { get; set; }
        public decimal OriginatingAmount { get; set; }
    }
    public class ResponseCurrency
    {
        public Guid CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyUnit { get; set; }
        public string CurrencySubUnit { get; set; }
    }
}
