using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace FLOG_BE.Features.Authentication
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<IActionResult> Login([FromQuery]string email, [FromQuery]string password)
        {
            return ToActionResult(await _mediator.Send(new DoLogin.Request
            {
                Password = password,
                Email = email
            }));
        }

        [HttpGet]
        [Route("CompanyLogin")]
        public async Task<IActionResult> CompanyLogin([FromQuery]string userid, [FromQuery]string roleid, [FromQuery]string CompanyId)
        {
            return ToActionResult(await _mediator.Send(new DoLoginCompany.Request
            {
                RoleId = roleid,
                UserId = userid,
                CompanyId = CompanyId
            }));
        }

        [HttpGet]
        [Route("token-detail")]
        public async Task<IActionResult> TokenDetail([FromQuery]string route)
        {
            return ToActionResult(await _mediator.Send(new GetTokenDetail.Request()
            {
                 Route = route
            }));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("SessionState")]
        public async Task<IActionResult> SessionState([FromQuery]string personid)
        {
            return ToActionResult(await _mediator.Send(new GetSession.Request()
            {
                PersonId = personid
            }));
        }

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePassword.RequestBodyChange body)
        {
            var req = new ChangePassword.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword.RequestData body)
        {
            var req = new ResetPassword.Request
            {
                Body = body

            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPost]
        [Route("CompanySignout")]
        [AllowAnonymous]
        public async Task<IActionResult> CompanySignout([FromBody] DoSignoutCompany.Request body)
        {
            var req = new DoSignoutCompany.Request
            {
                PersonId = body.PersonId,
                CompanySecurityId = body.CompanySecurityId,
                CompanyId = body.CompanyId
            };

            return ToActionResult(await _mediator.Send(req));
        }
    }
}
