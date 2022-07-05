using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.Payable.GetProgress
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public string SecuritySetting { get; set; }
        public RequestFilter Filter { get; set; }
        public Int32 Offset { get; set; }
        public Int32 Limit { get; set; }
        public List<string> Sort { get; set; }
    }

    public class RequestFilter
    {
        public List<string> DocumentType { get; set; }
        public List<string> DocumentNo { get; set; }
        public List<string> BranchCode { get; set; }
        public List<DateTime?> TransactionDateStart { get; set; }
        public List<DateTime?> TransactionDateEnd { get; set; }
        public List<string> TransactionType { get; set; }
        public List<string> CurrencyCode { get; set; }
        public List<decimal?> ExchangeRateMin { get; set; }
        public List<decimal?> ExchangeRateMax { get; set; }
        public Guid VendorId { get; set; }
        public List<string> VendorCode { get; set; }
        public List<string> VendorName { get; set; }
        public List<string> PaymentTermCode { get; set; }
        public List<string> VendorAddressCode { get; set; }
        public List<string> VendorDocumentNo { get; set; }
        public List<string> NsDocumentNo { get; set; }
        public List<string> MasterNo { get; set; }
        public List<string> AgreementNo { get; set; }
        public List<string> Description { get; set; }
        public List<decimal?> SubtotalAmountMin { get; set; }
        public List<decimal?> SubtotalAmountMax { get; set; }
        public List<decimal?> DiscountAmountMin { get; set; }
        public List<decimal?> DiscountAmountMax { get; set; }
        public List<decimal?> TaxAmountMin { get; set; }
        public List<decimal?> TaxAmountMax { get; set; }
        public List<string> VoidBy { get; set; }
        public List<DateTime?> VoidDateStart { get; set; }
        public List<DateTime?> VoidDateEnd { get; set; }
        public int? Status { get; set; }
        public List<string> StatusComment { get; set; }
        public List<string> CreatedBy { get; set; }
        public List<string> CreatedByName { get; set; }
        public List<DateTime?> CreatedDateStart { get; set; }
        public List<DateTime?> CreatedDateEnd { get; set; }
        public List<string> ModifiedBy { get; set; }
        public List<string> ModifiedByName { get; set; }
        public List<DateTime?> ModifiedDateStart { get; set; }
        public List<DateTime?> ModifiedDateEnd { get; set; }
        public List<string> BillToAddressCode { get; set; }
        public List<string> ShipToAddressCode { get; set; }
        public List<decimal?> OriginatingExtendedAmountMin { get; set; }
        public List<decimal?> OriginatingExtendedAmountMax { get; set; }
    }
}
