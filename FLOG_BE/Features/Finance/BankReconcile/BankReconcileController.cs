using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Finance.BankReconcile
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class BankReconcileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BankReconcileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("BankReconcile")]
        public async Task<IActionResult> CreateBankReconcile([FromBody] PostBankReconcile.RequestReconcileBody body)
        {
            var req = new PostBankReconcile.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("BankReconcile")]
        public async Task<IActionResult> UpdateBankReconcile([FromBody] PutBankReconcile.RequestReconcileBody body)
        {
            var req = new PutBankReconcile.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }        

        [HttpDelete]
        [Route("BankReconcile")]
        public async Task<IActionResult> DeleteBankReconcile([FromBody] DeleteBankReconcile.RequestDeleteBody body)
        {
            var req = new DeleteBankReconcile.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("PutStatusBankReconcile")]
        public async Task<IActionResult> PutStatusBankReconcile([FromBody] PutStatusBankReconcile.RequestPutStatus body)
        {
            var req = new PutStatusBankReconcile.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

       
        [HttpGet]
        [Route("GetProgressBankReconcile")]
        public async Task<IActionResult> GetProgressBankReconcile(
            [CommaSeparated] List<string> DocumentNo
           , [CommaSeparated] List<DateTime?> TransactionDateStart
           , [CommaSeparated] List<DateTime?> TransactionDateEnd
           , [CommaSeparated] List<string> CheckbookCode
           , [CommaSeparated] List<string> CurrencyCode
           , [CommaSeparated] List<DateTime?> BankCutoffEndStart
           , [CommaSeparated] List<DateTime?> BankCutoffEndEnd
           , [CommaSeparated] List<string> Description
           , [CommaSeparated] List<decimal?> BankEndingOrgBalanceMin
           , [CommaSeparated] List<decimal?> BankEndingOrgBalanceMax
           , [CommaSeparated] List<decimal?> CheckbookEndingOrgBalanceMin
           , [CommaSeparated] List<decimal?> CheckbookEndingOrgBalanceMax
           , [CommaSeparated] List<decimal?> BalanceDifferenceMin
           , [CommaSeparated] List<decimal?> BalanceDifferenceMax
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
            var req = new GetProgressBankReconcile.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetProgressBankReconcile.RequestFilter
                {
                    DocumentNo = DocumentNo,
                    TransactionDateStart = TransactionDateStart,
                    TransactionDateEnd = TransactionDateEnd,
                    CheckbookCode = CheckbookCode,
                    CurrencyCode = CurrencyCode,
                    BankCutoffEndStart = BankCutoffEndStart,
                    BankCutoffEndEnd = BankCutoffEndEnd,
                    Description = Description,
                    BankEndingOrgBalanceMin = BankEndingOrgBalanceMin,
                    BankEndingOrgBalanceMax = BankEndingOrgBalanceMax,
                    CheckbookEndingOrgBalanceMin = CheckbookEndingOrgBalanceMin,
                    CheckbookEndingOrgBalanceMax = CheckbookEndingOrgBalanceMax,
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
        [Route("GetHistoryBankReconcile")]
        public async Task<IActionResult> GetHistoryBankReconcile(
          [CommaSeparated] List<string> DocumentNo
           , [CommaSeparated] List<DateTime?> TransactionDateStart
           , [CommaSeparated] List<DateTime?> TransactionDateEnd
           , [CommaSeparated] List<string> CheckbookCode
           , [CommaSeparated] List<string> CurrencyCode
           , [CommaSeparated] List<DateTime?> BankCutoffEndStart
           , [CommaSeparated] List<DateTime?> BankCutoffEndEnd
           , [CommaSeparated] List<string> Description
           , [CommaSeparated] List<decimal?> BankEndingOrgBalanceMin
           , [CommaSeparated] List<decimal?> BankEndingOrgBalanceMax
           , [CommaSeparated] List<decimal?> CheckbookEndingOrgBalanceMin
           , [CommaSeparated] List<decimal?> CheckbookEndingOrgBalanceMax
           , [CommaSeparated] List<decimal?> BalanceDifferenceMin
           , [CommaSeparated] List<decimal?> BalanceDifferenceMax
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
            var req = new GetHistoryBankReconcile.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetHistoryBankReconcile.RequestFilter
                {
                    DocumentNo = DocumentNo,
                    TransactionDateStart = TransactionDateStart,
                    TransactionDateEnd = TransactionDateEnd,
                    CheckbookCode = CheckbookCode,
                    CurrencyCode = CurrencyCode,
                    BankCutoffEndStart = BankCutoffEndStart,
                    BankCutoffEndEnd = BankCutoffEndEnd,
                    Description = Description,
                    BankEndingOrgBalanceMin = BankEndingOrgBalanceMin,
                    BankEndingOrgBalanceMax = BankEndingOrgBalanceMax,
                    CheckbookEndingOrgBalanceMin = CheckbookEndingOrgBalanceMin,
                    CheckbookEndingOrgBalanceMax = CheckbookEndingOrgBalanceMax,
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
        [Route("GetPrevBankReconcile")]
        public async Task<IActionResult> GetPrevBankReconcile(
            [CommaSeparated] string CheckbookCode
          , [CommaSeparated] DateTime? BankCutoffStart
          , [CommaSeparated] List<string> sort
          , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetPrevBankReconcile.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetPrevBankReconcile.RequestFilter
                {
                    CheckbookCode = CheckbookCode,
                    BankCutoffStart = BankCutoffStart
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetActivitiesBankReconcile")]
        public async Task<IActionResult> GetActivitiesBankReconcile(
            [CommaSeparated] string CheckbookCode
          , [CommaSeparated] DateTime? BankCutoffEnd
          , [CommaSeparated] Guid? BankReconcileId
          , [CommaSeparated] List<string> sort
          , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetActivitiesBankReconcile.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetActivitiesBankReconcile.RequestFilter
                {
                    CheckbookCode = CheckbookCode,
                    BankCutoffEnd = BankCutoffEnd,
                    BankReconcileId = BankReconcileId
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
