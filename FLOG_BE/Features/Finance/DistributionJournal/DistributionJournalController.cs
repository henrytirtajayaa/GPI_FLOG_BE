using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Finance.DistributionJournal
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class DistributionJournalController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DistributionJournalController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("GetDistributionJournalByDocNo")]
        public async Task<IActionResult> GetDistributionJournalByNo(
           string DocumentNo
           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetDistributionByDocNo.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetDistributionByDocNo.RequestFilter
                {
                    DocumentNo = DocumentNo,
                    TransactionId = Guid.Empty,
                    TrxModule = 0
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
