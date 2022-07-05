using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace FLOG_BE.Features.Companies.Reference
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class ReferenceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReferenceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("Reference")]
        public async Task<IActionResult> GetReference([CommaSeparated] List<string> ReferenceCode,
            [CommaSeparated] List<string> ReferenceName
            , [CommaSeparated] List<string> sort
            , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetReference.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetReference.RequestFilter
                {
                    ReferenceCode = ReferenceCode,
                    ReferenceName = ReferenceName
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
