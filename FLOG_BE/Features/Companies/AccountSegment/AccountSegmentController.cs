using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace FLOG_BE.Features.Companies.AccountSegment
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class AccountSegmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountSegmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("AccountSegment")]
        public async Task<IActionResult> GetAccountSegment([CommaSeparated] List<int> SegmentNo
            , [CommaSeparated] List<string> Description
            , [CommaSeparated] List<int> Length 
            , [CommaSeparated] bool? IsMainAccount
            , [CommaSeparated] List<string> sort
            , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetAccountSegment.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetAccountSegment.RequestFilter
                {
                    SegmentNo = SegmentNo,
                    Description = Description,
                    IsMainAccount = IsMainAccount,
                    Length = Length
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPost]
        [Route("AccountSegment")]
        public async Task<IActionResult> CreateAccountSegment([FromBody] List<PostAccountSegment.RequestBodyAS> body)
        {
            var req = new PostAccountSegment.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("AccountSegment")]
        public async Task<IActionResult> DeleteAccount(List<DeleteAccountSegment.RequestDelete> body)
        {
            var req = new DeleteAccountSegment.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
