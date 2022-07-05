﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;

namespace FLOG_BE.Features.Finance.ApPayment.GetHistoryVendorPayment
{
    public class Response
    {
        public List<ResponseItem> PaymentHeader { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public Guid PaymentHeaderId { get; set; }
        public Int64 RowId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string DocumentNo { get; set; }
        public string CurrencyCode { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool IsMultiply { get; set; }
        public string CheckbookCode { get; set; }
        public Guid VendorId { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
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
        public List<ApPaymentDetail> ApPaymentDetails { get; set; }

        public decimal OriginatingTotalInvoice { get; set; }
        public decimal AppliedTotalPaid { get; set; }

        public string VoidBy { get; set; }
        public string VoidByName { get; set; }
        public DateTime? VoidDate { get; set; }
        public string StatusComment { get; set; }

    }
    public class ApPaymentDetail
    {
        public Guid PaymentHeaderId { get; set; }
        public Guid PaymentTransaktionId { get; set; }
        public string DocumentNo { get; set; }
        public string VendorCode { get; set; }
        public string NsDocumentNo { get; set; }
        public string MasterNo { get; set; }
        public string AgreementNo { get; set; }
        public decimal OrgDocAmount { get; set; }
        public string Description { get; set; }
        public decimal OriginatingBalance { get; set; }
        public decimal FuctionalBalance { get; set; }
        public decimal OriginatingPaid { get; set; }
        public decimal FuctionalPaid { get; set; }
        public int Status { get; set; }
    }

}
