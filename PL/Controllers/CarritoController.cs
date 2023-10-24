using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class CarritoController : Controller
    {
        public IActionResult Index()
        {
            ML.Dulceria dulceria = new ML.Dulceria();
            return View();
        }

        public IActionResult AddCarrito(int idDulce)
        {
            bool existe = false;
            ML.Venta carrito = new ML.Venta();
            carrito.Carrito = new List<object>();

            ML.Result result = BL.Dulceria.GetById(idDulce);
            if (HttpContext.Session.GetString("carrito") == null)
            {
                if (result.Correct)
                {
                    ML.Dulceria dulceria = (ML.Dulceria)result.Object;
                    dulceria.Cantidad = 1;
                    carrito.Carrito.Add(dulceria);
                    //serializa carrito
                    HttpContext.Session.SetString("Carrito",Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carrito));
                }
            }
            else
            {
                ML.Dulceria dulceria = (ML.Dulceria)result.Object;
                GetCarrito(carrito); //recuperamos el carrito
                foreach (ML.Dulceria dulceria1 in carrito.Carrito)
                {
                    if(dulceria.IdDulce == dulceria1.IdDulce)
                    {
                        dulceria1.Cantidad += 1;
                        existe = true;
                        break;
                    }
                    else
                    {
                        existe = false;
                    }
                }
                if (existe)
                {
                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carrito));
                }
                else
                {
                    dulceria.Cantidad = 1;
                    carrito.Carrito.Add (carrito);
                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carrito));
                }
            }
            return RedirectToAction("Catalogo");
        }

        public ML.Venta GetCarrito(ML.Venta carrito)
        {
            var ventaSession = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(HttpContext.Session.GetString("Carrito"));

            foreach (var obj in ventaSession)
            {
                ML.Dulceria objMateria = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Dulceria>(obj.ToString());
                carrito.Carrito.Add(objMateria);
            }
            return carrito;
        }
    }
}
