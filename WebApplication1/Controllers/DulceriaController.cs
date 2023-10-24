using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SL_WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DulceriaController : ControllerBase
    {
        [HttpGet]
        [Route("Dulcerias")]
        public IActionResult GetAll()
        {
            ML.Result result = BL.Dulceria.GetAll();
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(400, result);
            }
        }

        [HttpGet]
        [Route("GetById/{idDulce?}")]
        public IActionResult GetById(int idDulce)
        {
            ML.Result result = BL.Dulceria.GetById(idDulce);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(400, result);
            }
        }
    }
}
