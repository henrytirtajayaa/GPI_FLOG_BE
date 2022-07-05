using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Infrastructure.Attributes.CommaSeparated;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace FLOG_BE.Features.Companies.MSTransactionType
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class TransactionTypeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("MSTransactionType")]
        public async Task<IActionResult> GetTransactionType(
        [CommaSeparated] List<string> TransactionType
        , [CommaSeparated] List<string> TransactionName
        , [CommaSeparated] List<int?> PaymentCondition
        , [CommaSeparated] List<int?> RequiredSubject
        , [CommaSeparated] bool? InActive
        , [CommaSeparated] List<string> sort
        , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetTransactionType.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetTransactionType.RequestFilter
                {
                    TransactionType = TransactionType,
                    TransactionName = TransactionName,
                    PaymentCondition = PaymentCondition,
                    RequiredSubject = RequiredSubject,
                    InActive = InActive
                }
            };

            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPost]
        [Route("MSTransactionType")]
        public async Task<IActionResult> CreateTransactionType([FromBody] PostTransactionType.RequestPostTransactionType body)
        {
            var req = new PostTransactionType.Request
            {
                Body = body
            };

            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("MSTransactionType")]
        public async Task<IActionResult> UpdateTransactionType([FromBody] PutTransactionType.RequestUpdateTransactionType body)
        {
            var req = new PutTransactionType.Request
            {
                Body = body
            };

            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("MSTransactionType")]
        public async Task<IActionResult> DeleteTransactionType([FromBody] DeleteTransactionType.RequestDeleteTransactionType body)
        {
            var req = new DeleteTransactionType.Request
            {
                Body = body
            };

            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("MSTransactionTypeByDocSetup")]
        public async Task<IActionResult> GetTransactionTypeByDocSetup(
            int TrxModule,
            [CommaSeparated] List<string> TransactionType
            , [CommaSeparated] List<string> TransactionName
            , [CommaSeparated] List<int?> PaymentCondition
            , [CommaSeparated] List<int?> RequiredSubject
            , [CommaSeparated] bool? InActive
            , [CommaSeparated] List<string> sort
            , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetTransactionTypeByDocSetup.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetTransactionTypeByDocSetup.RequestFilter
                {
                    TrxModule = TrxModule,
                    TransactionType = TransactionType,
                    TransactionName = TransactionName,
                    PaymentCondition = PaymentCondition,
                    RequiredSubject = RequiredSubject,
                    InActive = InActive
                }
            };

            return ToActionResult(await _mediator.Send(req));
        }
    }
}
