using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Finance.ApPayment
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class ApPaymentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApPaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
        [Route("PostVendorPayment")]
        public async Task<IActionResult> CreateVendorPayment([FromBody] PostVendorPayment.RequestPaymentBody body)
        {
            var req = new PostVendorPayment.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetProgressVendorPayment")]
        public async Task<IActionResult> GetProgressVendorPayment(
          
            [CommaSeparated] List<string> DocumentNo
           , [CommaSeparated] List<DateTime?> TransactionDateStart
           , [CommaSeparated] List<DateTime?> TransactionDateEnd
           , [CommaSeparated] List<string> TransactionType
           , [CommaSeparated] List<string> CheckbookCode
           , [CommaSeparated] List<string> CurrencyCode
           , [CommaSeparated] List<decimal?> ExchangeRateMin
           , [CommaSeparated] List<decimal?> ExchangeRateMax
           , [CommaSeparated] List<string> VendorName
           , [CommaSeparated] List<string> Description
           , [CommaSeparated] List<decimal?> OriginatingTotalPaidMin
           , [CommaSeparated] List<decimal?> OriginatingTotalPaidMax
           , [CommaSeparated] List<decimal?> FunctionalTotalPaidMin
           , [CommaSeparated] List<decimal?> FunctionalTotalPaidMax
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
            var req = new GetProgressVendorPayment.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetProgressVendorPayment.RequestFilter
                {
                    DocumentNo = DocumentNo,
                    TransactionDateStart = TransactionDateStart,
                    TransactionDateEnd = TransactionDateEnd,
                    TransactionType = TransactionType,
                    CheckbookCode = CheckbookCode,
                    CurrencyCode = CurrencyCode,
                    ExchangeRateMin = ExchangeRateMin,
                    ExchangeRateMax = ExchangeRateMax,
                    VendorName = VendorName,
                    Description = Description,
                    OriginatingTotalPaidMin = OriginatingTotalPaidMin,
                    OriginatingTotalPaidMax = OriginatingTotalPaidMax,
                    FunctionalTotalPaidMin = FunctionalTotalPaidMin,
                    FunctionalTotalPaidMax = FunctionalTotalPaidMax,
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
        [Route("GetPaymentDetail")]
        public async Task<IActionResult> GetPaymentDetail(
            [CommaSeparated] Guid PayableTransactionId
           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetPaymentDetail.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetPaymentDetail.RequestFilterDetail
                {
                    PayableTransactionId = PayableTransactionId,
                   
                }
            };
            return ToActionResult(await _mediator.Send(req));
        } 
        [HttpGet]
        [Route("GetHistoryVendorPayment")]
        public async Task<IActionResult> GetHistoryVendorPayment(
          
            [CommaSeparated] List<string> DocumentNo
           , [CommaSeparated] List<DateTime?> TransactionDateStart
           , [CommaSeparated] List<DateTime?> TransactionDateEnd
           , [CommaSeparated] List<string> TransactionType
           , [CommaSeparated] List<string> CheckbookCode
           , [CommaSeparated] List<string> CurrencyCode
           , [CommaSeparated] List<decimal?> ExchangeRateMin
           , [CommaSeparated] List<decimal?> ExchangeRateMax
           , [CommaSeparated] List<string> VendorName
           , [CommaSeparated] List<string> Description
           , [CommaSeparated] List<decimal?> OriginatingTotalPaidMin
           , [CommaSeparated] List<decimal?> OriginatingTotalPaidMax
           , [CommaSeparated] List<decimal?> FunctionalTotalPaidMin
           , [CommaSeparated] List<decimal?> FunctionalTotalPaidMax
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
            var req = new GetHistoryVendorPayment.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetHistoryVendorPayment.RequestFilter
                {
                    DocumentNo = DocumentNo,
                    TransactionDateStart = TransactionDateStart,
                    TransactionDateEnd = TransactionDateEnd,
                    TransactionType = TransactionType,
                    CheckbookCode = CheckbookCode,
                    CurrencyCode = CurrencyCode,
                    ExchangeRateMin = ExchangeRateMin,
                    ExchangeRateMax = ExchangeRateMax,
                    VendorName = VendorName,
                    Description = Description,
                    OriginatingTotalPaidMin = OriginatingTotalPaidMin,
                    OriginatingTotalPaidMax = OriginatingTotalPaidMax,
                    FunctionalTotalPaidMin = FunctionalTotalPaidMin,
                    FunctionalTotalPaidMax = FunctionalTotalPaidMax,
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

        [HttpPut]
        [Route("PutVendorPayment")]
        public async Task<IActionResult> UpdateVendorPayment([FromBody] PutVendorPayment.RequestPayment body)
        {
            var req = new PutVendorPayment.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
        [HttpPut]
        [Route("DeleteVendorPayment")]
        public async Task<IActionResult> DeleteVendorPayment([FromBody] DeleteVendorPayment.RequestPaymentDeleteBody body)
        {
            var req = new DeleteVendorPayment.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPost]
        [Route("PaymentTransactionApproval")]
        public async Task<IActionResult> ApprovalCheckbook([FromBody] PostSubmitApprovalDetail.RequestPaymentApproval body)
        {
            var req = new PostSubmitApprovalDetail.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetUserApprovalPayment")]
        public async Task<IActionResult> GetuserApprovalPayment([CommaSeparated] Guid PaymentHeaderId
            , [CommaSeparated] Guid PersonId
            , [CommaSeparated] List<string> sort
            , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetUserApprovalPayment.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetUserApprovalPayment.RequestFilter
                {
                    PaymentHeaderId = PaymentHeaderId,
                    PersonId = PersonId
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("PutStatusPaymentApproval")]
        public async Task<IActionResult> PutStatusApproval([FromBody] PutStatusApprovalPayment.RequestPutStatusPaymentApproval body)
        {
            var req = new PutStatusApprovalPayment.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
        [HttpGet]
        [Route("GetApprovalPaymnetComment")]
        public async Task<IActionResult> GetApprovalComment([CommaSeparated] Guid PaymentHeaderId
            , [CommaSeparated] Guid PersonId
            , [CommaSeparated] Int32 Index
            , [CommaSeparated] List<string> sort
            , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetApprovalComment.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetApprovalComment.RequestFilter
                {
                    PaymentHeaderId = PaymentHeaderId,
                    PersonId = PersonId,
                    Index = Index
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetVendorPaymentById")]
        public async Task<IActionResult> GetReceiveById(
            [CommaSeparated] Guid PaymentHeaderId)
        {
            var req = new GetVendorPaymentById.Request
            {
                PaymentHeaderId = PaymentHeaderId,
            };
            return ToActionResult(await _mediator.Send(req));
        }

    }
}
