using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Finance.Checkbook
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class CheckbookController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CheckbookController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("CheckbookTransaction")]
        public async Task<IActionResult> CreateCheckbook([FromBody] PostTransaction.RequestCheckbookBody body)
        {
            var req = new PostTransaction.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("CheckbookTransaction")]
        public async Task<IActionResult> UpdateCheckbook([FromBody] PutTransaction.RequestCheckbookUpdateBody body)
        {
            var req = new PutTransaction.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("PutTrxDelete")]
        public async Task<IActionResult> PutTrxCheckbook([FromBody] PutTrxDelete.RequestCheckbookTrxDeleteBody body)
        {
            var req = new PutTrxDelete.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetProgressCheckbook")]
        public async Task<IActionResult> GetProggress([CommaSeparated] List<string> DocumentType
             , [CommaSeparated] List<string> DocumentNo
             , [CommaSeparated] List<string> BranchCode
             , [CommaSeparated] List<DateTime?> TransactionDateStart
             , [CommaSeparated] List<DateTime?> TransactionDateEnd
             , [CommaSeparated] List<string> TransactionType
             , [CommaSeparated] List<string> CurrencyCode
             , [CommaSeparated] List<decimal?> ExchangeRateMin
             , [CommaSeparated] List<decimal?> ExchangeRateMax
             , [CommaSeparated] List<string> CheckbookCode
             , [CommaSeparated] bool? IsVoid
             , [CommaSeparated] List<string> VoidDocumentNo
             , [CommaSeparated] List<string> PaidSubject
             , [CommaSeparated] List<string> SubjectCode
             , [CommaSeparated] List<string> Description
             , [CommaSeparated] List<decimal?> OriginatingTotalAmountMax
             , [CommaSeparated] List<decimal?> OriginatingTotalAmountMin
             , [CommaSeparated] List<decimal?> FunctionalTotalAmountMax
             , [CommaSeparated] List<decimal?> FunctionalTotalAmountMin
             , [CommaSeparated] List<string> VoidBy
             , [CommaSeparated] List<DateTime?> VoidDateStart
             , [CommaSeparated] List<DateTime?> VoidDateEnd
             , [CommaSeparated] int? Status
             , [CommaSeparated] List<string> StatusComment
             , [CommaSeparated] List<string> CreatedBy
             , [CommaSeparated] List<string> CreatedName
             , [CommaSeparated] List<DateTime?> CreatedDateStart
             , [CommaSeparated] List<DateTime?> CreatedDateEnd
             , [CommaSeparated] List<DateTime?> ModifiedDateStart
             , [CommaSeparated] List<DateTime?> ModifiedDateEnd
             , [CommaSeparated] List<string> ModifiedBy
             , [CommaSeparated] List<string> ModifiedByName
             , [CommaSeparated] List<DateTime?> ModifiedDate
             , [CommaSeparated] List<string> sort
             , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetProgress.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetProgress.RequestFilter
                {
                    DocumentType = DocumentType,
                    DocumentNo = DocumentNo,
                    BranchCode = BranchCode,
                    TransactionDateStart = TransactionDateStart,
                    TransactionDateEnd = TransactionDateEnd,
                    TransactionType = TransactionType,
                    CurrencyCode = CurrencyCode,
                    ExchangeRateMin = ExchangeRateMin,
                    ExchangeRateMax = ExchangeRateMax,
                    CheckbookCode = CheckbookCode,
                    IsVoid = IsVoid,
                    VoidDocumentNo = VoidDocumentNo,
                    PaidSubject = PaidSubject,
                    SubjectCode = SubjectCode,
                    Description = Description,
                    OriginatingTotalAmountMin = OriginatingTotalAmountMin,
                    OriginatingTotalAmountMax = OriginatingTotalAmountMax,
                    FunctionalTotalAmountMax = FunctionalTotalAmountMax,
                    FunctionalTotalAmountMin = FunctionalTotalAmountMin,
                    VoidBy = VoidBy,
                    VoidDateStart = VoidDateStart,
                    VoidDateEnd = VoidDateEnd,
                    Status = Status,
                    StatusComment = StatusComment,
                    CreatedBy = CreatedBy,
                    CreatedName = CreatedName,
                    CreatedDateStart = CreatedDateStart,
                    CreatedDateEnd = CreatedDateEnd,
                    ModifiedBy = ModifiedBy,
                    ModifiedByName = ModifiedByName,
                    ModifiedDateStart = ModifiedDateStart,
                    ModifiedDateEnd = ModifiedDateEnd

                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetHistoryCheckbook")]
        public async Task<IActionResult> GetHistory([CommaSeparated] List<string> DocumentType
             , [CommaSeparated] List<string> DocumentNo
             , [CommaSeparated] List<string> BranchCode
             , [CommaSeparated] List<DateTime?> TransactionDateStart
             , [CommaSeparated] List<DateTime?> TransactionDateEnd
             , [CommaSeparated] List<string> TransactionType
             , [CommaSeparated] List<string> CurrencyCode
             , [CommaSeparated] List<decimal?> ExchangeRateMin
             , [CommaSeparated] List<decimal?> ExchangeRateMax
             , [CommaSeparated] List<string> CheckbookCode
             , [CommaSeparated] bool? IsVoid
             , [CommaSeparated] List<string> VoidDocumentNo
             , [CommaSeparated] List<string> PaidSubject
             , [CommaSeparated] List<string> SubjectCode
             , [CommaSeparated] List<string> Description
             , [CommaSeparated] List<decimal?> OriginatingTotalAmountMax
             , [CommaSeparated] List<decimal?> OriginatingTotalAmountMin
             , [CommaSeparated] List<decimal?> FunctionalTotalAmountMax
             , [CommaSeparated] List<decimal?> FunctionalTotalAmountMin
             , [CommaSeparated] List<string> VoidBy
             , [CommaSeparated] List<DateTime?> VoidDateStart
             , [CommaSeparated] List<DateTime?> VoidDateEnd
             , [CommaSeparated] int? Status
             , [CommaSeparated] List<string> StatusComment
             , [CommaSeparated] List<string> CreatedBy
             , [CommaSeparated] List<string> CreatedName
             , [CommaSeparated] List<DateTime?> CreatedDateStart
             , [CommaSeparated] List<DateTime?> CreatedDateEnd
             , [CommaSeparated] List<DateTime?> ModifiedDateStart
             , [CommaSeparated] List<DateTime?> ModifiedDateEnd
             , [CommaSeparated] List<string> ModifiedBy
             , [CommaSeparated] List<string> ModifiedByName
             , [CommaSeparated] List<DateTime?> ModifiedDate
             , [CommaSeparated] List<string> sort
             , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetHistory.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetHistory.RequestFilter
                {
                    DocumentType = DocumentType,
                    DocumentNo = DocumentNo,
                    BranchCode = BranchCode,
                    TransactionDateStart = TransactionDateStart,
                    TransactionDateEnd = TransactionDateEnd,
                    TransactionType = TransactionType,
                    CurrencyCode = CurrencyCode,
                    ExchangeRateMin = ExchangeRateMin,
                    ExchangeRateMax = ExchangeRateMax,
                    CheckbookCode = CheckbookCode,
                    IsVoid = IsVoid,
                    VoidDocumentNo = VoidDocumentNo,
                    PaidSubject = PaidSubject,
                    SubjectCode = SubjectCode,
                    Description = Description,
                    OriginatingTotalAmountMin = OriginatingTotalAmountMin,
                    OriginatingTotalAmountMax = OriginatingTotalAmountMax,
                    FunctionalTotalAmountMax = FunctionalTotalAmountMax,
                    FunctionalTotalAmountMin = FunctionalTotalAmountMin,
                    VoidBy = VoidBy,
                    VoidDateStart = VoidDateStart,
                    VoidDateEnd = VoidDateEnd,
                    Status = Status,
                    StatusComment = StatusComment,
                    CreatedBy = CreatedBy,
                    CreatedName = CreatedName,
                    CreatedDateStart = CreatedDateStart,
                    CreatedDateEnd = CreatedDateEnd,
                    ModifiedBy = ModifiedBy,
                    ModifiedByName = ModifiedByName,
                    ModifiedDateStart = ModifiedDateStart,
                    ModifiedDateEnd = ModifiedDateEnd

                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetDetailCheckbookTransaction")]
        public async Task<IActionResult> GetDetailCheckbookTransaction(
           Guid CheckbookTransactionId
           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetDetail.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetDetail.RequestFilter
                {
                    CheckbookTransactionId = CheckbookTransactionId
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("PutStatusCheckbookTransaction")]
        public async Task<IActionResult> PutStatus([FromBody] PutStatus.RequestPutStatusCheckbook body)
        {
            var req = new PutStatus.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPost]
        [Route("CheckbookTransactionApproval")]
        public async Task<IActionResult> ApprovalCheckbook([FromBody] PostApprovalDetail.RequestCheckbookApproval body)
        {
            var req = new PostApprovalDetail.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetUserApproval")]
        public async Task<IActionResult> GetuserApproval([CommaSeparated] Guid CheckbookTransactionId
             , [CommaSeparated] Guid PersonId
             , [CommaSeparated] List<string> sort
             , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetUserApproval.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetUserApproval.RequestFilter
                {
                    CheckbookTransactionId = CheckbookTransactionId,
                    PersonId = PersonId 
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("PutStatusCheckbookApproval")]
        public async Task<IActionResult> PutStatusApproval([FromBody] PutStatusApproval.RequestPutStatusCheckbookApproval body)
        {
            var req = new PutStatusApproval.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetApprovalComment")]
        public async Task<IActionResult> GetApprovalComment([CommaSeparated] Guid CheckbookTransactionId
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
                    CheckbookTransactionId = CheckbookTransactionId
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("DeleteCheckbookTransaction")]
        public async Task<IActionResult> DeleteCheckbookTransaction(DeleteTransaction.RequestBodyCheckbookTransactionDelete body)
        {
            var req = new DeleteTransaction.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetCheckbookTransactionById")]
        public async Task<IActionResult> GetReceiveById(
            [CommaSeparated] Guid CheckbookTransactionId)
        {
            var req = new GetCheckbookTransactionById.Request
            {
                CheckbookTransactionId = CheckbookTransactionId
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
