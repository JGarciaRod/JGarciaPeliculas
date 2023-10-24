using Microsoft.AspNetCore.Mvc;

namespace SL_WebApi.Controllers
{
    [Route("api/Cine")]
    public class CineController : Controller
    {
        [Route("")]
        [HttpGet]
        public IActionResult GetAll()
        {

            return Ok();
        }
    }
}
