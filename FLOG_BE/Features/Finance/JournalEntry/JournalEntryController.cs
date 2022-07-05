using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Finance.JournalEntry
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class JournalEntryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JournalEntryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("JournalEntry")]
        public async Task<IActionResult> CreateJournalEntry([FromBody] PostJournalEntry.RequestEntryHeader body)
        {
            var req = new PostJournalEntry.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("JournalEntry")]
        public async Task<IActionResult> PutJournalEntry([FromBody] PutJournalEntry.RequestEntryHeader body)
        {
            var req = new PutJournalEntry.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("PutStatusJournalEntry")]
        public async Task<IActionResult> PutStatusJournalEntry([FromBody] PutJournalEntry.RequestEntryHeader body)
        {
            var req = new PutStatusJournalEntry.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("JournalEntry")]
        public async Task<IActionResult> DeleteJournalEntry([FromBody] DeleteJournalEntry.RequestDeleteJournalEntry body)
        {
            var req = new DeleteJournalEntry.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetJournalEntryHistory")]
        public async Task<IActionResult> GetHistory(
             [CommaSeparated] List<string> DocumentNo
             , [CommaSeparated] List<DateTime?> TransactionDateStart
             , [CommaSeparated] List<DateTime?> TransactionDateEnd
             , [CommaSeparated] List<string> BranchCode
             , [CommaSeparated] List<string> CurrencyCode
             , [CommaSeparated] List<decimal?> ExchangeRateMin
             , [CommaSeparated] List<decimal?> ExchangeRateMax
             , [CommaSeparated] List<string> SourceDocument
             , [CommaSeparated] List<string> Description
             , [CommaSeparated] List<decimal?> OriginatingTotalMin
             , [CommaSeparated] List<decimal?> OriginatingTotalMax
             , [CommaSeparated] List<decimal?> FunctionalTotalMin
             , [CommaSeparated] List<decimal?> FunctionalTotalMax
             , [CommaSeparated] List<string> CreatedBy
             , [CommaSeparated] List<string> CreatedByName
            , [CommaSeparated] List<DateTime?> CreatedDateStart
             , [CommaSeparated] List<DateTime?> CreatedDateEnd
             , [CommaSeparated] List<string> ModifiedBy
             , [CommaSeparated] List<string> ModifiedByName
             , [CommaSeparated] List<DateTime?> ModifiedDateStart
             , [CommaSeparated] List<DateTime?> ModifiedDateEnd
             , [CommaSeparated] List<DateTime?> ModifiedDate
             , [CommaSeparated] int? Status
             , [CommaSeparated] List<string> sort
             , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetHistoryJournalEntry.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetHistoryJournalEntry.RequestFilter
                {
                    DocumentNo = DocumentNo,
                    TransactionDateStart = TransactionDateStart,
                    TransactionDateEnd = TransactionDateEnd,
                    BranchCode = BranchCode,
                    CurrencyCode = CurrencyCode,
                    ExchangeRateMin = ExchangeRateMin,
                    ExchangeRateMax = ExchangeRateMax,
                    SourceDocument = SourceDocument,
                    Description = Description,
                    OriginatingTotalMin = OriginatingTotalMin,
                    OriginatingTotalMax = OriginatingTotalMax,
                    FunctionalTotalMin = FunctionalTotalMin,
                    FunctionalTotalMax = FunctionalTotalMax,
                    CreatedBy = CreatedBy,
                    CreatedDateStart = CreatedDateStart,
                    CreatedDateEnd = CreatedDateEnd,
                    ModifiedBy = ModifiedBy,
                    ModifiedByName = ModifiedByName,
                    ModifiedDateStart = ModifiedDateStart,
                    ModifiedDateEnd = ModifiedDateEnd,
                    Status = Status,                    
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetJournalEntryProgress")]
        public async Task<IActionResult> GetProgress(
            [CommaSeparated] List<string> DocumentNo
             , [CommaSeparated] List<DateTime?> TransactionDateStart
             , [CommaSeparated] List<DateTime?> TransactionDateEnd
             , [CommaSeparated] List<string> BranchCode
             , [CommaSeparated] List<string> CurrencyCode
             , [CommaSeparated] List<decimal?> ExchangeRateMin
             , [CommaSeparated] List<decimal?> ExchangeRateMax
             , [CommaSeparated] List<string> SourceDocument
             , [CommaSeparated] List<string> Description
             , [CommaSeparated] List<decimal?> OriginatingTotalMin
             , [CommaSeparated] List<decimal?> OriginatingTotalMax
             , [CommaSeparated] List<decimal?> FunctionalTotalMin
             , [CommaSeparated] List<decimal?> FunctionalTotalMax
             , [CommaSeparated] List<string> CreatedBy
             , [CommaSeparated] List<string> CreatedByName
            , [CommaSeparated] List<DateTime?> CreatedDateStart
             , [CommaSeparated] List<DateTime?> CreatedDateEnd
             , [CommaSeparated] List<string> ModifiedBy
             , [CommaSeparated] List<string> ModifiedByName
             , [CommaSeparated] List<DateTime?> ModifiedDateStart
             , [CommaSeparated] List<DateTime?> ModifiedDateEnd
             , [CommaSeparated] List<DateTime?> ModifiedDate
             , [CommaSeparated] int? Status
             , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetProgressJournalEntry.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetProgressJournalEntry.RequestFilter
                {
DocumentNo = DocumentNo,
                    TransactionDateStart = TransactionDateStart,
                    TransactionDateEnd = TransactionDateEnd,
                    BranchCode = BranchCode,
                    CurrencyCode = CurrencyCode,
                    ExchangeRateMin = ExchangeRateMin,
                    ExchangeRateMax = ExchangeRateMax,
                    SourceDocument = SourceDocument,
                    Description = Description,
                    OriginatingTotalMin = OriginatingTotalMin,
                    OriginatingTotalMax = OriginatingTotalMax,
                    FunctionalTotalMin = FunctionalTotalMin,
                    FunctionalTotalMax = FunctionalTotalMax,
                    CreatedBy = CreatedBy,
                    CreatedDateStart = CreatedDateStart,
                    CreatedDateEnd = CreatedDateEnd,
                    ModifiedBy = ModifiedBy,
                    ModifiedByName = ModifiedByName,
                    ModifiedDateStart = ModifiedDateStart,
                    ModifiedDateEnd = ModifiedDateEnd,
                    Status = Status,
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetDetailJournalEntry")]
        public async Task<IActionResult> GetDetailJournalEntry(
           Guid JournalEntryHeaderId
           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetDetailJournalEntry.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetDetailJournalEntry.RequestFilter
                {
                    JournalEntryHeaderId = JournalEntryHeaderId                    
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
