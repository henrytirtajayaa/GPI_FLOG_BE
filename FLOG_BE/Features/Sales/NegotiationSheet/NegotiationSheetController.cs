using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Sales.NegotiationSheet
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class NegotiationSheetController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NegotiationSheetController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut]
        [Route("CreateInvoiceNegotiationSheet")]
        public async Task<IActionResult> CreateInvoiceNegotiationSheet([FromBody] CreateInvoiceNegotiationSheet.RequestPutStatus body)
        {
            var req = new CreateInvoiceNegotiationSheet.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("PutStatusNegotiationSheet")]
        public async Task<IActionResult> PutStatusNegotiationSheet([FromBody] PutStatusNegotiationSheet.RequestPutStatus body)
        {
            var req = new PutStatusNegotiationSheet.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("PutNegotiationSheet")]
        public async Task<IActionResult> PutNegotiationSheet([FromBody] PutNegotiationSheet.RequestNegotiationSheetBody body)
        {
            var req = new PutNegotiationSheet.Request
            {
                Body = body
            };

            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetNegotiationSheetApprovalComments")]
        public async Task<IActionResult> GetNSApprovalComment([CommaSeparated] Guid NegotiationSheetId
            , [CommaSeparated] Guid PersonId
            , [CommaSeparated] Int32 Index
            , [CommaSeparated] List<string> sort
            , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetNegotiationSheetApprovalComment.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetNegotiationSheetApprovalComment.RequestFilter
                {
                    NegotiationSheetId = NegotiationSheetId,
                    PersonId = PersonId,
                    Index = Index
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetNegotiationSheetProgress")]
        public async Task<IActionResult> GetNegotiationSheetProgress(          
           [CommaSeparated] List<string> DocumentNo
           , [CommaSeparated] List<string> BranchCode
           , [CommaSeparated] List<DateTime?> TransactionDateStart
           , [CommaSeparated] List<DateTime?> TransactionDateEnd
           , [CommaSeparated] List<string> TransactionType
           , [CommaSeparated] List<string> MasterNo
           , [CommaSeparated] List<string> AgreementNo
           , [CommaSeparated] List<string> SoDocumentNo
           , [CommaSeparated] List<string> CustomerCode
           , [CommaSeparated] List<string> CustomerName
           , [CommaSeparated] List<string> ShippingLineName
           , [CommaSeparated] List<string> FinalDestination
           , [CommaSeparated] List<string> PortOfLoading
           , [CommaSeparated] List<string> PortOfDischarge
           , [CommaSeparated] List<string> TermOfShipment
           , [CommaSeparated] List<string> Commodity
           , [CommaSeparated] List<string> CreatedBy
           , [CommaSeparated] List<string> CreatedByName
           , [CommaSeparated] List<DateTime?> CreatedDateStart
           , [CommaSeparated] List<DateTime?> CreatedDateEnd
           , [CommaSeparated] List<string> ModifiedBy
           , [CommaSeparated] List<string> ModifiedByName
           , [CommaSeparated] List<DateTime?> ModifiedDateStart
           , [CommaSeparated] List<DateTime?> ModifiedDateEnd
           , [CommaSeparated] List<string> SalesOrderMasterNo
           , [CommaSeparated] List<string> SalesOrderAgreementNo
           , [CommaSeparated] int? Status
           , [CommaSeparated] List<string> StatusComment
           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetNegotiationSheetProgress.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetNegotiationSheetProgress.RequestFilter
                {
                    DocumentNo = DocumentNo,
                    BranchCode = BranchCode,
                    TransactionDateStart = TransactionDateStart,
                    TransactionDateEnd = TransactionDateEnd,
                    TransactionType = TransactionType,
                    CustomerCode = CustomerCode,
                    CustomerName = CustomerName,
                    SoDocumentNo = SoDocumentNo,
                    ShippingLineName = ShippingLineName,
                    FinalDestination = FinalDestination,
                    TermOfShipment = TermOfShipment,
                    PortOfLoading = PortOfLoading,
                    PortOfDischarge = PortOfDischarge,
                    Commodity = Commodity,
                    MasterNo = MasterNo, 
                    AgreementNo = AgreementNo,
                    CreatedBy = CreatedBy,
                    CreatedByName = CreatedByName,
                    CreatedDateStart = CreatedDateStart,
                    CreatedDateEnd = CreatedDateEnd,
                    ModifiedBy = ModifiedBy,
                    ModifiedByName = ModifiedByName,
                    ModifiedDateStart = ModifiedDateStart,
                    ModifiedDateEnd = ModifiedDateEnd,
                    SalesOrderMasterNo = SalesOrderMasterNo,
                    SalesOrderAgreementNo = SalesOrderAgreementNo,
                    Status = Status,
                    StatusComment = StatusComment
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetNegotiationSheetHistory")]
        public async Task<IActionResult> GetNegotiationSheetHistory(
           [CommaSeparated] List<string> DocumentNo
           , [CommaSeparated] List<string> BranchCode
           , [CommaSeparated] List<string> TransactionType            
           , [CommaSeparated] List<DateTime?> TransactionDateStart
           , [CommaSeparated] List<DateTime?> TransactionDateEnd
           , [CommaSeparated] List<string> SoDocumentNo
           , [CommaSeparated] List<string> CustomerCode
           , [CommaSeparated] List<string> CustomerName
           , [CommaSeparated] List<string> MasterNo
           , [CommaSeparated] List<string> AgreementNo
           , [CommaSeparated] List<string> ShippingLineName
           , [CommaSeparated] List<string> FinalDestination
           , [CommaSeparated] List<string> PortOfLoading
           , [CommaSeparated] List<string> PortOfDischarge
           , [CommaSeparated] List<string> TermOfShipment
           , [CommaSeparated] List<string> Commodity
           , [CommaSeparated] List<string> CreatedBy
           , [CommaSeparated] List<string> CreatedByName
           , [CommaSeparated] List<DateTime?> CreatedDateStart
           , [CommaSeparated] List<DateTime?> CreatedDateEnd
           , [CommaSeparated] List<string> ModifiedBy
           , [CommaSeparated] List<string> ModifiedByName
           , [CommaSeparated] List<DateTime?> ModifiedDateStart
           , [CommaSeparated] List<DateTime?> ModifiedDateEnd
           , [CommaSeparated] List<string> SalesOrderMasterNo
           , [CommaSeparated] List<string> SalesOrderAgreementNo
           , [CommaSeparated] int? Status
           , [CommaSeparated] List<string> StatusComment
           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetNegotiationSheetHistory.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetNegotiationSheetHistory.RequestFilter
                {
                    DocumentNo = DocumentNo,
                    BranchCode = BranchCode,
                    TransactionDateStart = TransactionDateStart,
                    TransactionDateEnd = TransactionDateEnd,
                    TransactionType = TransactionType,
                    CustomerCode = CustomerCode,
                    CustomerName = CustomerName,
                    SoDocumentNo = SoDocumentNo,
                    ShippingLineName = ShippingLineName,
                    FinalDestination = FinalDestination,
                    TermOfShipment = TermOfShipment,
                    PortOfLoading = PortOfLoading,
                    PortOfDischarge = PortOfDischarge,
                    Commodity = Commodity,
                    MasterNo = MasterNo,
                    AgreementNo = AgreementNo,
                    CreatedBy = CreatedBy,
                    CreatedByName = CreatedByName,
                    CreatedDateStart = CreatedDateStart,
                    CreatedDateEnd = CreatedDateEnd,
                    ModifiedBy = ModifiedBy,
                    ModifiedByName = ModifiedByName,
                    ModifiedDateStart = ModifiedDateStart,
                    ModifiedDateEnd = ModifiedDateEnd,
                    SalesOrderMasterNo = SalesOrderMasterNo,
                    SalesOrderAgreementNo = SalesOrderAgreementNo,
                    Status = Status,
                    StatusComment = StatusComment
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetNegotiationSheetApproved")]
        public async Task<IActionResult> GetNegotiationSheetApproved(

           [CommaSeparated] List<string> DocumentNo
           , [CommaSeparated] List<string> BranchCode
           , [CommaSeparated] List<string> TransactionType
           , [CommaSeparated] List<string> CustomerCode
           , [CommaSeparated] List<string> CustomerName
           , [CommaSeparated] List<DateTime?> TransactionDateStart
           , [CommaSeparated] List<DateTime?> TransactionDateEnd
           , [CommaSeparated] List<string> SoDocumentNo
           , [CommaSeparated] List<string> MasterNo
           , [CommaSeparated] List<string> AgreementNo
           , [CommaSeparated] List<string> ShippingLineName
           , [CommaSeparated] List<string> FinalDestination
           , [CommaSeparated] List<string> PortOfLoading
           , [CommaSeparated] List<string> PortOfDischarge
           , [CommaSeparated] List<string> TermOfShipment
           , [CommaSeparated] List<string> Commodity
           , [CommaSeparated] List<string> CreatedBy
           , [CommaSeparated] List<string> CreatedByName
           , [CommaSeparated] List<DateTime?> CreatedDateStart
           , [CommaSeparated] List<DateTime?> CreatedDateEnd
           , [CommaSeparated] List<string> ModifiedBy
           , [CommaSeparated] List<string> ModifiedByName
           , [CommaSeparated] List<DateTime?> ModifiedDateStart
           , [CommaSeparated] List<DateTime?> ModifiedDateEnd
           , [CommaSeparated] int? Status
           , [CommaSeparated] bool? IsInvoicePending
           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetNegotiationSheetApproved.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetNegotiationSheetApproved.RequestFilter
                {
                    DocumentNo = DocumentNo,
                    BranchCode = BranchCode,
                    TransactionDateStart = TransactionDateStart,
                    TransactionDateEnd = TransactionDateEnd,
                    TransactionType = TransactionType,
                    CustomerCode = CustomerCode,
                    CustomerName = CustomerName,
                    MasterNo = MasterNo,
                    AgreementNo = AgreementNo,
                    SoDocumentNo = SoDocumentNo,
                    ShippingLineName = ShippingLineName,
                    FinalDestination = FinalDestination,
                    TermOfShipment = TermOfShipment,
                    PortOfLoading = PortOfLoading,
                    PortOfDischarge = PortOfDischarge,
                    Commodity = Commodity,
                    CreatedBy = CreatedBy,
                    CreatedByName = CreatedByName,
                    CreatedDateStart = CreatedDateStart,
                    CreatedDateEnd = CreatedDateEnd,
                    ModifiedBy = ModifiedBy,
                    ModifiedByName = ModifiedByName,
                    ModifiedDateStart = ModifiedDateStart,
                    ModifiedDateEnd = ModifiedDateEnd,
                    Status = Status,
                    IsInvoicePending = IsInvoicePending,
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetNegotiationSheetAll")]
        public async Task<IActionResult> GetNegotiationSheetAll(
           [CommaSeparated] List<string> DocumentNo
           , [CommaSeparated] List<string> BranchCode
           , [CommaSeparated] List<DateTime?> TransactionDateStart
           , [CommaSeparated] List<DateTime?> TransactionDateEnd
           , [CommaSeparated] List<string> TransactionType
           , [CommaSeparated] List<string> MasterNo
           , [CommaSeparated] List<string> AgreementNo
           , [CommaSeparated] List<string> SoDocumentNo
           , [CommaSeparated] List<string> CustomerCode
           , [CommaSeparated] List<string> CustomerName
           , [CommaSeparated] List<string> ShippingLineName
           , [CommaSeparated] List<string> FinalDestination
           , [CommaSeparated] List<string> PortOfLoading
           , [CommaSeparated] List<string> PortOfDischarge
           , [CommaSeparated] List<string> TermOfShipment
           , [CommaSeparated] List<string> Commodity
           , [CommaSeparated] List<string> CreatedBy
           , [CommaSeparated] List<string> CreatedByName
           , [CommaSeparated] List<DateTime?> CreatedDateStart
           , [CommaSeparated] List<DateTime?> CreatedDateEnd
           , [CommaSeparated] List<string> ModifiedBy
           , [CommaSeparated] List<string> ModifiedByName
           , [CommaSeparated] List<DateTime?> ModifiedDateStart
           , [CommaSeparated] List<DateTime?> ModifiedDateEnd
           , [CommaSeparated] List<string> SalesOrderMasterNo
           , [CommaSeparated] List<string> SalesOrderAgreementNo
           , [CommaSeparated] int? Status
           , [CommaSeparated] List<string> StatusComment
           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetNegotiationSheetAll.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetNegotiationSheetAll.RequestFilter
                {
                    DocumentNo = DocumentNo,
                    BranchCode = BranchCode,
                    TransactionDateStart = TransactionDateStart,
                    TransactionDateEnd = TransactionDateEnd,
                    TransactionType = TransactionType,
                    CustomerCode = CustomerCode,
                    CustomerName = CustomerName,
                    SoDocumentNo = SoDocumentNo,
                    ShippingLineName = ShippingLineName,
                    FinalDestination = FinalDestination,
                    TermOfShipment = TermOfShipment,
                    PortOfLoading = PortOfLoading,
                    PortOfDischarge = PortOfDischarge,
                    Commodity = Commodity,
                    MasterNo = MasterNo,
                    AgreementNo = AgreementNo,
                    CreatedBy = CreatedBy,
                    CreatedByName = CreatedByName,
                    CreatedDateStart = CreatedDateStart,
                    CreatedDateEnd = CreatedDateEnd,
                    ModifiedBy = ModifiedBy,
                    ModifiedByName = ModifiedByName,
                    ModifiedDateStart = ModifiedDateStart,
                    ModifiedDateEnd = ModifiedDateEnd,
                    SalesOrderMasterNo = SalesOrderMasterNo,
                    SalesOrderAgreementNo = SalesOrderAgreementNo,
                    Status = Status,
                    StatusComment = StatusComment
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("DeleteNegotiationSheet")]
        public async Task<IActionResult> DeleteSalesNegotiationSheet([FromBody] DeleteNegotiationSheet.RequestBodyNegotiationSheet body)
        {
            var req = new DeleteNegotiationSheet.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
