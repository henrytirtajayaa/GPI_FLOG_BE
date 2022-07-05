using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Finance.Sales.Quotation
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class SalesQuotationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SalesQuotationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("GetSalesQuotationAll")]
        public async Task<IActionResult> GetSalesQuotationAll(

           [CommaSeparated] List<string> DocumentNo
           , [CommaSeparated] List<string> SalesPerson
           , [CommaSeparated] List<string> CustomerName
           , [CommaSeparated] List<string> ShipperName
           , [CommaSeparated] List<string> ShippingLineOwner
           , [CommaSeparated] List<DateTime?> TransactionDateStart
           , [CommaSeparated] List<DateTime?> TransactionDateEnd
           , [CommaSeparated] List<string> CreatedBy
           , [CommaSeparated] List<string> CreatedByName
           , [CommaSeparated] List<DateTime?> CreatedDateStart
           , [CommaSeparated] List<DateTime?> CreatedDateEnd
           , [CommaSeparated] List<string> ModifiedBy
           , [CommaSeparated] List<string> ModifiedByName
           , [CommaSeparated] List<DateTime?> ModifiedDateStart
           , [CommaSeparated] List<DateTime?> ModifiedDateEnd
           , [CommaSeparated] int? Status
           , [CommaSeparated] List<string> StatusComment
           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetQuotationAll.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetQuotationAll.RequestFilter
                {
                    DocumentNo = DocumentNo,
                    TransactionDateStart = TransactionDateStart,
                    TransactionDateEnd = TransactionDateEnd,
                    SalesPerson = SalesPerson,
                    CustomerName = CustomerName,
                    ShipperName = ShipperName,
                    ShippingLineOwner = ShippingLineOwner,
                    CreatedBy = CreatedBy,
                    CreatedByName = CreatedByName,
                    CreatedDateStart = CreatedDateStart,
                    CreatedDateEnd = CreatedDateEnd,
                    ModifiedBy = ModifiedBy,
                    ModifiedByName = ModifiedByName,
                    ModifiedDateStart = ModifiedDateStart,
                    ModifiedDateEnd = ModifiedDateEnd,
                    Status = Status,
                    StatusComment = StatusComment
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetSalesQuotationProgress")]
        public async Task<IActionResult> GetSalesQuotationProgress(
          
           [CommaSeparated] List<string> DocumentNo
           , [CommaSeparated] List<string> SalesPerson
           , [CommaSeparated] List<string> CustomerName
           , [CommaSeparated] List<string> ShipperName
           , [CommaSeparated] List<string> ShippingLineOwner
           , [CommaSeparated] List<DateTime?> TransactionDateStart
           , [CommaSeparated] List<DateTime?> TransactionDateEnd
           , [CommaSeparated] List<string> CreatedBy
           , [CommaSeparated] List<string> CreatedByName
           , [CommaSeparated] List<DateTime?> CreatedDateStart
           , [CommaSeparated] List<DateTime?> CreatedDateEnd
           , [CommaSeparated] List<string> ModifiedBy
           , [CommaSeparated] List<string> ModifiedByName
           , [CommaSeparated] List<DateTime?> ModifiedDateStart
           , [CommaSeparated] List<DateTime?> ModifiedDateEnd
           , [CommaSeparated] int? Status
           , [CommaSeparated] List<string> StatusComment
           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetQuotationProgress.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetQuotationProgress.RequestFilter
                {
                    DocumentNo = DocumentNo,
                    TransactionDateStart = TransactionDateStart,
                    TransactionDateEnd = TransactionDateEnd,
                    SalesPerson = SalesPerson,
                    CustomerName = CustomerName,
                    ShipperName = ShipperName,
                    ShippingLineOwner = ShippingLineOwner,
                    CreatedBy = CreatedBy,
                    CreatedByName = CreatedByName,
                    CreatedDateStart = CreatedDateStart,
                    CreatedDateEnd = CreatedDateEnd,
                    ModifiedBy = ModifiedBy,
                    ModifiedByName = ModifiedByName,
                    ModifiedDateStart = ModifiedDateStart,
                    ModifiedDateEnd = ModifiedDateEnd,
                    Status = Status,
                    StatusComment = StatusComment
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetSalesQuotationHistory")]
        public async Task<IActionResult> GetSalesQuotationHistory(

            [CommaSeparated] List<string> TransactionType
            ,[CommaSeparated] List<string> DocumentNo
            , [CommaSeparated] List<string> SalesPerson
            , [CommaSeparated] List<string> CustomerName
            , [CommaSeparated] List<string> ShipperName
            , [CommaSeparated] List<string> ShippingLineOwner
           , [CommaSeparated] List<DateTime?> TransactionDateStart
           , [CommaSeparated] List<DateTime?> TransactionDateEnd
           , [CommaSeparated] List<string> CreatedBy
           , [CommaSeparated] List<string> CreatedByName
           , [CommaSeparated] List<DateTime?> CreatedDateStart
           , [CommaSeparated] List<DateTime?> CreatedDateEnd
           , [CommaSeparated] List<string> ModifiedBy
           , [CommaSeparated] List<string> ModifiedByName
           , [CommaSeparated] List<DateTime?> ModifiedDateStart
           , [CommaSeparated] List<DateTime?> ModifiedDateEnd
           , [CommaSeparated] int? Status
           , [CommaSeparated] List<string> StatusComment
           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetQuotationHistory.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetQuotationHistory.RequestFilter
                {
                    TransactionType = TransactionType,
                    DocumentNo = DocumentNo,
                    TransactionDateStart = TransactionDateStart,
                    TransactionDateEnd = TransactionDateEnd,
                    SalesPerson = SalesPerson,
                    CustomerName = CustomerName,
                    ShipperName = ShipperName,
                    ShippingLineOwner = ShippingLineOwner,
                    CreatedBy = CreatedBy,
                    CreatedByName = CreatedByName,
                    CreatedDateStart = CreatedDateStart,
                    CreatedDateEnd = CreatedDateEnd,
                    ModifiedBy = ModifiedBy,
                    ModifiedByName = ModifiedByName,
                    ModifiedDateStart = ModifiedDateStart,
                    ModifiedDateEnd = ModifiedDateEnd,
                    Status = Status,
                    StatusComment = StatusComment
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPost]
        [Route("PostSalesQuotation")]
        public async Task<IActionResult> PostSalesQuotation([FromBody] PostQuotation.RequestQuotationBody body)
        {
            var req = new PostQuotation.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
        [HttpPut]
        [Route("PutSalesQuotation")]
        public async Task<IActionResult> PutSalesQuotation([FromBody] PutQuotation.RequestQuotationBody body)
        {
            var req = new PutQuotation.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
        [HttpPut]
        [Route("PutCancelSalesQuotation")]
        public async Task<IActionResult> PutCancelSalesQuotation([FromBody] PutStatusQuotation.RequestSalesCancel body)
        {
            var req = new PutStatusQuotation.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
        [HttpPut]
        [Route("PutStatusQuotation")]
        public async Task<IActionResult> PutStatusQuotation([FromBody] PutStatusQuotation.RequestSalesCancel body)
        {
            var req = new PutStatusQuotation.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
        [HttpPut]
        [Route("SubmitQuotation")]
        public async Task<IActionResult> SubmitQuotation([FromBody] SubmitQuotation.SubmitQuotationBody body)
        {
            var req = new SubmitQuotation.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
        [HttpDelete]
        [Route("DeleteSalesQuotation")]
        public async Task<IActionResult> DeleteSalesQuotation([FromBody] Features.Sales.Quotation.DeleteQuotation.RequestBodySalesQuotation body)
        {
            var req = new Features.Sales.Quotation.DeleteQuotation.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
