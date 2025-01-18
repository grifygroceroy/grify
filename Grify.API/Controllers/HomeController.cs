using Common.Model;
using Grify.API.Applogics.Common.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Grify.API.Controllers
{
    [Route(ApiRoutes.v1)]
    public class HomeController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult> GetItem([FromQuery] HomeQuery query)
        {
            return Ok(await Mediator.Send(query));
            //new text
        }
    }
}
