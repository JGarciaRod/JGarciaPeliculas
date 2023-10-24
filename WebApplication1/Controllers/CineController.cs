using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SL_WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CineController : ControllerBase
    {
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            ML.Result result = BL.Cine.GetAll();
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
        [Route("GetById/{idCine?}")]
        public IActionResult GetById(int idCine)
        {
            ML.Result result = BL.Cine.GetById(idCine);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(400,result);
            }
        }

        [HttpPost]
        [Route("")]
        public IActionResult Add(ML.Cine cine)
        {
            ML.Result result = BL.Cine.Add(cine);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(400,result);
            }
        }

        [HttpPut]
        [Route("{idCine?}")]
        public IActionResult Update(int idCine,[FromBody] ML.Cine cine)
        {
            cine.IdCine = idCine;
            ML.Result result = BL.Cine.Update(cine);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(400,result);
            }
        }

        [HttpDelete]
        [Route("{idCine?}")]
        public IActionResult Dell(int idCine)
        {
            ML.Result result = BL.Cine.Dell(idCine);
            if(result.Correct)
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
