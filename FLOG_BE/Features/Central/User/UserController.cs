using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using FLOG_BE.Features.Central.User.GetUser;

namespace FLOG_BE.Features.Central.User
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("User")]
        public async Task<IActionResult> GetUser([CommaSeparated] List<string> UserFullName
          , [CommaSeparated] List<string> UserGroupCode
          , [CommaSeparated] List<string> EmailAddress
          , [CommaSeparated]  bool? InActive
          , [CommaSeparated] List<string> CreatedBy
          , [CommaSeparated] List<string> CreatedByName
          , [CommaSeparated] List<DateTime?> CreatedDateStart
          , [CommaSeparated] List<DateTime?> CreatedDateEnd
          , [CommaSeparated] List<DateTime?> ModifiedDateStart
          , [CommaSeparated] List<DateTime?> ModifiedDateEnd
          , [CommaSeparated] List<string> ModifiedBy
          , [CommaSeparated] List<DateTime?> ModifiedDate
          , [CommaSeparated] List<string> sort
          , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {

        var req = new GetUser.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetUser.RequestFilter
                {
                    UserFullName = UserFullName,
                    UserGroupCode = UserGroupCode,
                    EmailAddress = EmailAddress,
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

        [HttpDelete]
        [Route("User")]
        public async Task<IActionResult> DeleteUser(DeleteUser.RequestFilter body)
        {
            var req = new DeleteUser.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
        [HttpPost]
        [Route("User")]
        public async Task<IActionResult> CreateUser([FromBody] PostUser.RequestBodyUser body)
        {
            var req = new PostUser.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("User")]
        public async Task<IActionResult> UpdateUser([FromBody] PutUser.RequestPutBodyUser body)
        {
            var req = new PutUser.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

    }
}
