using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class CineController : Controller
    {
        [HttpGet]
        public IActionResult GetAllCines()
        {
            ML.Cine cine = new ML.Cine();
            cine.Cines = new List<object>();

            ML.Result resultSubCstegoria = new ML.Result();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5083/api/Cine/");

                var responsTask = client.GetAsync("GetAll");
                responsTask.Wait();

                var result = responsTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ML.Result>();
                    readTask.Wait();

                    foreach (var resultItem in readTask.Result.Objects)
                    {
                        ML.Cine resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Cine>(resultItem.ToString());
                        cine.Cines.Add(resultItemList);
                    }
                }

            }
            return View(cine);
        }


        [HttpPost]
        public IActionResult GetAll(ML.Cine cine)
        {
           
            ML.Result resultSubCstegoria = new ML.Result();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5083/api/Cine/");

                var responsTask = client.GetAsync("$GetAll");
                responsTask.Wait();

                var result = responsTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ML.Result>();
                    readTask.Wait();

                    foreach (var resultItem in readTask.Result.Objects)
                    {
                        ML.Cine resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Cine>(resultItem.ToString());
                        cine.Cines.Add(resultItemList);
                    }
                }
            }
            return View(cine);
        }


        [HttpGet]
        public IActionResult Add(int? IdCine)
        {
            ML.Cine cine = new ML.Cine();
            cine.IdCine = IdCine.Value;

            cine.Cines = new List<object>();
            cine.Zona = new ML.Zona();

            //ML.Result resultZona = BL.Zona.GetAll();

            if (IdCine != 0 ) //update
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5083/api/Cine/");
                    var responsTask = client.GetAsync("GetById/" + IdCine);
                    responsTask.Wait();


                    var result = responsTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        
                        var readTask = result.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();

                        ML.Cine resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Cine>(readTask.Result.Object.ToString());
                        cine = resultItemList;

                        //cine.Zona.Zonas = resultZona.Objects;
                    }
                    else
                    {
                        //cine.Zona.Zonas = resultZona.Objects;
                    }
                }
            }
            return View(cine);
        }


        [HttpPost]
        public IActionResult Add(ML.Cine cine)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5083/api/Cine/");

                if(cine.IdCine == 0) //add
                {
                    var responsTask = client.PostAsJsonAsync<ML.Cine>("",cine);
                    responsTask.Wait();

                    var result = responsTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        ViewBag.Mensaje = "Se ha insertado correctamente";
                    }
                    else
                    {
                        ViewBag.Error = result.ToString();
                    }
                }
                else //update
                {
                    var responsTask = client.PutAsJsonAsync<ML.Cine>("" + cine.IdCine , cine);
                    responsTask.Wait();

                    var result = responsTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        ViewBag.Mensaje = "Se ha actualizado correctamente";
                    }
                    else
                    {
                        ViewBag.Error = result.ToString();
                    }
                }
            }
            return PartialView("Modal");
        }

        [HttpDelete]
        public IActionResult Dell(int IdCine)
        {
            ML.Cine cine = new ML.Cine();
            cine.IdCine = IdCine;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5083/api/Cine/");

                var responsTask = client.DeleteAsync("" +cine.IdCine);
                responsTask.Wait();

                var result = responsTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    ViewBag.Mensaje = "Se ha eliminado correctamente";
                }
                else
                {
                    ViewBag.Error = result.ToString();
                }
            }
            return PartialView("Modal");
        }   
    }
}
