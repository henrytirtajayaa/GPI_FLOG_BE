using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Companies.Account
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("Account")]
        public async Task<IActionResult> GetAccount(
            [CommaSeparated] List<string> AccountId
            ,[CommaSeparated] List<string> Description
            , [CommaSeparated] List<string> Segment1
            , [CommaSeparated] List<string> Segment2
            , [CommaSeparated] List<string> Segment3
            , [CommaSeparated] List<string> Segment4
            , [CommaSeparated] List<string> Segment5
            , [CommaSeparated] List<string> Segment6
            , [CommaSeparated] List<string> Segment7
            , [CommaSeparated] List<string> Segment8
            , [CommaSeparated] List<string> Segment9
            , [CommaSeparated] List<string> Segment10
            , [CommaSeparated] bool? PostingType
            , [CommaSeparated] bool? NormalBalance
            , [CommaSeparated] bool? NoDirectEntry
            , [CommaSeparated] bool? Revaluation
            , [CommaSeparated] bool? Inactive
            , [CommaSeparated] List<string> CreatedBy
            , [CommaSeparated] List<DateTime?> CreatedDateStart
            , [CommaSeparated] List<DateTime?> CreatedDateEnd
            , [CommaSeparated] List<string> ModifiedBy
            , [CommaSeparated] List<DateTime?> ModifiedDateStart
            , [CommaSeparated] List<DateTime?> ModifiedDateEnd
            , [CommaSeparated] List<string> sort
            , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetAccount.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetAccount.RequestFilter
                {
                    AccountId = AccountId,
                    Description = Description,
                    Segment1 = Segment1,
                    Segment2 = Segment2,
                    Segment3 = Segment3,
                    Segment4 = Segment4,
                    Segment5 = Segment5,
                    Segment6 = Segment6,
                    Segment7 = Segment7,
                    Segment8 = Segment8,
                    Segment9 = Segment9,
                    Segment10 = Segment10,
                    PostingType = PostingType,
                    NormalBalance = NormalBalance,
                    NoDirectEntry = NoDirectEntry,
                    Revaluation = Revaluation,
                    Inactive = Inactive,
                    CreatedBy = CreatedBy,
                    CreatedDateStart = CreatedDateStart,
                    CreatedDateEnd = CreatedDateEnd,
                    ModifiedBy = ModifiedBy,
                    ModifiedDateStart = ModifiedDateStart,
                    ModifiedDateEnd = ModifiedDateEnd
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }
        [HttpPost]
        [Route("Account")]
        public async Task<IActionResult> CreateAccount([FromBody] PostAccount.RequestAccountBody body)
        {
            var req = new PostAccount.Request
            {
                Body = body
            };

            return ToActionResult(await _mediator.Send(req));
        }


        [HttpPut]
        [Route("Account")]
        public async Task<IActionResult> UpdateAccount([FromBody] PutAccount.RequestPutAccountBody body)
        {
            var req = new PutAccount.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("Account")]
        public async Task<IActionResult> DeleteAccount(DeleteAccount.RequestAccountDelete body)
        {
            var req = new DeleteAccount.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

    }
}
