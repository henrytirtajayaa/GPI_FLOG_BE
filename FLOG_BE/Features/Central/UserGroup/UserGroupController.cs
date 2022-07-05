using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Central.UserGroup
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class UserGroupController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserGroupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("UserGroup")]
        public async Task<IActionResult> getUserGroup(
            [CommaSeparated] List<string> UserGroupCode
            , [CommaSeparated] List<string> UserGroupName
            , [CommaSeparated] List<string> CreatedBy
            , [CommaSeparated] List<DateTime?> CreatedDateStart
            , [CommaSeparated] List<DateTime?> CreatedDateEnd
            , [CommaSeparated] List<string> UpdatedBy
            , [CommaSeparated] List<DateTime?> UpdatedDateStart
            , [CommaSeparated] List<DateTime?> UpdatedDateEnd
            , [CommaSeparated] List<string> sort
            , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetUserGroup.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetUserGroup.RequestFilter
                {
                    UserGroupCode = UserGroupCode,
                    UserGroupName = UserGroupName,
                    CreatedBy = CreatedBy,
                    CreatedDateStart = CreatedDateStart,
                    CreatedDateEnd = CreatedDateEnd,
                    UpdatedBy = UpdatedBy,
                    UpdatedDateStart = UpdatedDateStart,
                    UpdatedDateEnd = UpdatedDateEnd
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPost]
        [Route("UserGroup")]
        public async Task<IActionResult> CreateUserGroup([FromBody] PostUserGroup.RequestBodyPostUserGroup body)
        {
            var req = new PostUserGroup.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("UserGroup")]
        public async Task<IActionResult> UpdateUserGroup([FromBody] PutUserGroup.RequestBodyUpdateUserGroup body)
        {
            var req = new PutUserGroup.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("UserGroup")]
        public async Task<IActionResult> DeleteUserGroup(DeleteUserGroup.RequestBodyDeleteUserGroup body)
        {
            var req = new DeleteUserGroup.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

    }
}
