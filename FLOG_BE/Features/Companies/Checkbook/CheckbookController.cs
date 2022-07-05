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

namespace FLOG_BE.Features.Companies.Checkbook
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

        [HttpGet]
        [Route("Checkbook")]
        public async Task<IActionResult> GetCheckbook(
            [CommaSeparated] List<string> CheckbookCode
            , [CommaSeparated] List<string> CheckbookName
            , [CommaSeparated] List<string> BankAccountCode
            , [CommaSeparated] List<string> CurrencyCode
            , [CommaSeparated] List<string> BankCode
            , [CommaSeparated] List<string> CheckbookAccountNo
            , [CommaSeparated] bool? HasCheckoutApproval
            , [CommaSeparated] List<string> ApprovalCode
            , [CommaSeparated] List<string> CheckbookInDocNo
            , [CommaSeparated] List<string> CheckbookOutDocNo
            , [CommaSeparated] List<string> ReceiptDocNo
            , [CommaSeparated] List<string> PaymentDocNo
            , [CommaSeparated] List<string> ReconcileDocNo
            , [CommaSeparated] bool? IsCash
            , [CommaSeparated] bool? InActive
            , [CommaSeparated] List<string> CreatedBy
            , [CommaSeparated] List<DateTime?> CreatedDateStart
            , [CommaSeparated] List<DateTime?> CreatedDateEnd
            , [CommaSeparated] List<string> ModifiedBy
            , [CommaSeparated] List<DateTime?> ModifiedDateStart
            , [CommaSeparated] List<DateTime?> ModifiedDateEnd
            , [CommaSeparated] List<string> sort
            , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetCheckbook.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetCheckbook.RequestFilter
                {
                    CheckbookCode = CheckbookCode,
                    CheckbookName = CheckbookName,
                    BankAccountCode = BankAccountCode,
                    CurrencyCode = CurrencyCode,
                    BankCode = BankCode,
                    CheckbookAccountNo = CheckbookAccountNo,
                    HasCheckoutApproval = HasCheckoutApproval,
                    ApprovalCode = ApprovalCode,
                    CheckbookInDocNo = CheckbookInDocNo,
                    CheckbookOutDocNo = CheckbookOutDocNo,
                    ReceiptDocNo = ReceiptDocNo,
                    PaymentDocNo = PaymentDocNo,
                    ReconcileDocNo = ReconcileDocNo,
                    IsCash = IsCash,
                    InActive = InActive,
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
        [Route("Checkbook")]
        public async Task<IActionResult> CreateCheckbook([FromBody] PostCheckbook.RequestBodyPostCheckbook body)
        {
            var req = new PostCheckbook.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("Checkbook")]
        public async Task<IActionResult> UpdateCheckbook([FromBody] PutCheckbook.RequestBodyUpdateCheckbook body)
        {
            var req = new PutCheckbook.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("Checkbook")]
        public async Task<IActionResult> DeleteCheckbook([FromBody] DeleteCheckbook.RequestBodyDeleteCheckbook body)
        {
            var req = new DeleteCheckbook.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
