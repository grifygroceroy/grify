using Common.Model;
using Grify.API.Applogics.Common.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Grify.API.Controllers
{
    [Route(ApiRoutes.v1)]
    [AllowAnonymous]
    public class AuthController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult> RefreshToken([FromQuery] AuthQuery query)
        {
            return Ok(await Mediator.Send(query));
        }
    }
}
