using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class EstadisticasController : Controller
    {
        public IActionResult GetAll()
        {
            ML.Cine cine = new ML.Cine();
            cine.Cines = new List<object>();

            ML.Result resultSubCstegoria = new ML.Result();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5083/api/Estadisticas/");

                var responsTask = client.GetAsync("Cine_Zonas");
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
    }
}
