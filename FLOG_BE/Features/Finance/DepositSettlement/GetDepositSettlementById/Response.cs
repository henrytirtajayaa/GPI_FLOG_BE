using FLOG_BE.Model.Central.Entities;
using FLOG_BE.Model.Companies.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Finance.DepositSettlement.GetDepositSettlementById
{
    public class Response
    {
        public ResponseDeposit DepositSettlement { get; set; }
        public ResponseCompanySetup CompanySetup { get; set; }
        public ResponseCompanyAddress CompanyAddress { get; set; }
        public List<DepositSettlementDetail> DepositSettlementDetails { get; set; }
    }
    public class ResponseDeposit
    {
        public Guid SettlementHeaderId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public string DepositNo { get; set; }
        public string CurrencyCode { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Description { get; set; }
        public decimal OriginatingTotalPaid { get; set; }
        public decimal FunctionalTotalPaid { get; set; }
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
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Fax { get; set; }
    }

    public class DepositSettlementDetail
    {
        public Guid SettlementDetailId { get; set; }
        public Guid ReceiveTransactionId { get; set; }
        public string DocumentNo { get; set; }
        public decimal OriginatingPaid { get; set; }
    }
}
