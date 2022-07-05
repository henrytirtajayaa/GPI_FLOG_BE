using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Sales.NegotiationSheet.GetNegotiationSheetHistory
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
      
        public List<Guid> NegotiationSheetId { get; set; }
        public List<DateTime?> TransactionDateStart { get; set; }
        public List<DateTime?> TransactionDateEnd { get; set; }
        public List<string> TransactionType { get; set; }
        public List<string> BranchCode { get; set; }
        public List<string> DocumentNo { get; set; }
        public List<string> SoDocumentNo { get; set; }
        public List<string> MasterNo { get; set; }
        public List<string> AgreementNo { get; set; }
        public List<string> CustomerCode { get; set; }
        public List<string> CustomerName { get; set; }
        public List<string> ShippingLineName { get; set; }
        public List<string> TermOfShipment { get; set; }
        public List<string> FinalDestination { get; set; }
        public List<string> PortOfLoading { get; set; }
        public List<string> PortOfDischarge { get; set; }
        public List<string> Commodity { get; set; }
        public int? Status { get; set; }
        public List<string> StatusComment { get; set; }

        public List<string> CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public List<string> ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public List<string> CreatedByName { get; set; }
        public List<DateTime?> CreatedDateStart { get; set; }
        public List<DateTime?> CreatedDateEnd { get; set; }
        public List<string> ModifiedByName { get; set; }
        public List<DateTime?> ModifiedDateStart { get; set; }
        public List<DateTime?> ModifiedDateEnd { get; set; }
      
        public List<string> SalesOrderMasterNo { get; set; }
        public List<string> SalesOrderAgreementNo { get; set; }
    }
}
