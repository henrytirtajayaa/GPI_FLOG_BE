using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Companies.Container
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class ContainerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContainerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("Container")]
        public async Task<IActionResult> GetContainer([CommaSeparated] List<string> ContainerCode
            , [CommaSeparated] List<string> ContainerName
            , [CommaSeparated] List<int?> ContainerSizeMin
            , [CommaSeparated] List<int?> ContainerSizeMax
            , [CommaSeparated] List<string> ContainerType
            , [CommaSeparated] List<int?> ContainerTeusMin
            , [CommaSeparated] List<int?> ContainerTeusMax
            , [CommaSeparated] bool? IsReefer
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
            var req = new GetContainer.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetContainer.RequestFilter
                {
                    ContainerCode = ContainerCode,
                    ContainerName = ContainerName,
                    ContainerSizeMin = ContainerSizeMin,
                    ContainerSizeMax = ContainerSizeMax,
                    ContainerType = ContainerType,
                    ContainerTeusMin = ContainerTeusMin,
                    ContainerTeusMax = ContainerTeusMax,
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
        [Route("Container")]
        public async Task<IActionResult> CreateCompany([FromBody] PostContainer.RequestBodyContainer body)
        {
            var req = new PostContainer.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("Container")]
        public async Task<IActionResult> UpdateCompany([FromBody] PutContainer.RequestBodyUpdateContainer body)
        {
            var req = new PutContainer.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("Container")]
        public async Task<IActionResult> DeleteCompany(DeleteContainer.RequestBodyDeleteContainer body)
        {
            var req = new DeleteContainer.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
