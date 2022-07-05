using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Companies.PaymentTerm
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class PaymentTermController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentTermController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("PaymentTerm")]
        public async Task<IActionResult> GetPaymentTerm(
            [CommaSeparated] List<string> PaymentTermCode
            , [CommaSeparated] List<string> PaymentTermDesc
            , [CommaSeparated] List<int?> DueMin
            , [CommaSeparated] List<int?> DueMax
            , [CommaSeparated] List<int?> Unit
            , [CommaSeparated] List<string> CreatedBy
            , [CommaSeparated] List<DateTime?> CreatedDateStart
            , [CommaSeparated] List<DateTime?> CreatedDateEnd
            , [CommaSeparated] List<string> ModifiedBy
            , [CommaSeparated] List<DateTime?> ModifiedDateStart
            , [CommaSeparated] List<DateTime?> ModifiedDateEnd
            , [CommaSeparated] List<string> sort
            , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetPaymentTerm.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetPaymentTerm.RequestFilter
                {
                    PaymentTermCode = PaymentTermCode,
                    PaymentTermDesc = PaymentTermDesc,
                    DueMin = DueMin,
                    DueMax = DueMax,
                    Unit = Unit,
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
        [Route("PaymentTerm")]
        public async Task<IActionResult> CreatePaymentTerm([FromBody] PostPaymentTerm.RequestPaymentTermBody body)
        {
            var req = new PostPaymentTerm.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("PaymentTerm")]
        public async Task<IActionResult> UpdatePaymentTerm([FromBody] PutPaymentTerm.RequestUpdatePaymentTermBody body)
        {
            var req = new PutPaymentTerm.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }


        [HttpDelete]
        [Route("PaymentTerm")]
        public async Task<IActionResult> DeletePaymentTerm(DeletePaymentTerm.RequestBodyDeletePaymentTerm body)
        {
            var req = new DeletePaymentTerm.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
