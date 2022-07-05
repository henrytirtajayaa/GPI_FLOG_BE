using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Finance.Sales.SalesOrder
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class SalesOrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SalesOrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("GetSalesOrderAll")]
        public async Task<IActionResult> GetSalesOrderAll(

           [CommaSeparated] List<string> DocumentNo
           , [CommaSeparated] List<string> BranchCode
           , [CommaSeparated] List<string> TransactionType
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
           , [CommaSeparated] List<string> MasterNo
           , [CommaSeparated] List<string> AgreementNo
           , [CommaSeparated] int? Status
           , [CommaSeparated] List<string> StatusComment
           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetSalesOrderAll.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetSalesOrderAll.RequestFilter
                {
                    DocumentNo = DocumentNo,
                    BranchCode = BranchCode,
                    TransactionType = TransactionType,
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
                    MasterNo = MasterNo,
                    AgreementNo = AgreementNo,
                    Status = Status,
                    StatusComment = StatusComment
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetSalesOrderProgress")]
        public async Task<IActionResult> GetSalesOrderProgress(
          
           [CommaSeparated] List<string> DocumentNo
           , [CommaSeparated] List<string> BranchCode
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
           , [CommaSeparated] List<string> MasterNo
           , [CommaSeparated] List<string> AgreementNo
           , [CommaSeparated] int? Status
           , [CommaSeparated] List<string> StatusComment
           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetSalesOrderProgress.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetSalesOrderProgress.RequestFilter
                {
                    DocumentNo = DocumentNo,
                    BranchCode = BranchCode,
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
                    MasterNo = MasterNo,
                    AgreementNo = AgreementNo,
                    Status = Status,
                    StatusComment = StatusComment
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetSalesOrderHistory")]
        public async Task<IActionResult> GetSalesOrderHistory(
          
           [CommaSeparated] List<string> DocumentNo
           , [CommaSeparated] List<string> BranchCode
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
           , [CommaSeparated] List<string> MasterNo
           , [CommaSeparated] List<string> AgreementNo
           , [CommaSeparated] int? Status
           , [CommaSeparated] List<string> StatusComment
           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetSalesOrderHistory.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetSalesOrderHistory.RequestFilter
                {
                    DocumentNo = DocumentNo,
                    BranchCode = BranchCode,
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
                    MasterNo = MasterNo,
                    AgreementNo = AgreementNo,
                    Status = Status,
                    StatusComment = StatusComment
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }



        [HttpPost]
        [Route("PostSalesOrder")]
        public async Task<IActionResult> PostSalesQuotation([FromBody] PostSalesOrder.RequestSalesOrderBody body)
        {
            var req = new PostSalesOrder.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
        [HttpPut]
        [Route("PutSalesOrder")]
        public async Task<IActionResult> PutSalesOrder([FromBody] PutSalesOrder.RequestSalesOrderBody body)
        {
            var req = new PutSalesOrder.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
        [HttpPut]
        [Route("PutStatusSalesOrder")]
        public async Task<IActionResult> PutStatusSalesOrder([FromBody] PutStatusSalesOrder.RequestSalesOrder body)
        {
            var req = new PutStatusSalesOrder.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
        [HttpPut]
        [Route("SubmitSalesOrder")]
        public async Task<IActionResult> SubmitSalesOrder([FromBody] SubmitSalesOrder.SubmitSalesOrderBody body)
        {
            var req = new SubmitSalesOrder.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
       
       
        [HttpDelete]
        [Route("DeleteSalesOrder")]
        public async Task<IActionResult> DeleteSalesSalesOrder([FromBody] Features.Sales.SalesOrder.DeleteSalesOrder.RequestBodySalesOrder body)
        {
            var req = new Features.Sales.SalesOrder.DeleteSalesOrder.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
