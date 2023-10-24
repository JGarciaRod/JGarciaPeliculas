using BL;
using ML;
using Microsoft.AspNetCore.Mvc;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.IO.Image;

namespace PL.Controllers
{
    public class DulceriaController : Controller
    {
        public IActionResult Index()
        {
            ML.Dulceria dulceria = new ML.Dulceria();
            dulceria.Dulces = new List<object>();

            ML.Result resultSubCategoria = new ML.Result();

            using (var client= new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5083/api/Dulceria/");

                var responsTask = client.GetAsync("Dulcerias");
                responsTask.Wait();

                var result = responsTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ML.Result>();
                    readTask.Wait();

                    foreach (var resultItem in readTask.Result.Objects)
                    {
                        ML.Dulceria resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Dulceria>(resultItem.ToString());
                        dulceria.Dulces.Add(resultItemList);
                    }
                }

            }

            return View(dulceria);
        }

        public IActionResult AddCarrito(int idDulce)
        {
            bool existe = false;
            ML.Venta carrito = new ML.Venta();
            carrito.Carrito = new List<object>();

            ML.Result result = BL.Dulceria.GetById(idDulce);
            if (HttpContext.Session.GetString("Carrito") == null)
            {
                if (result.Correct)
                {
                    ML.Dulceria dulceria = (ML.Dulceria)result.Object;
                    dulceria.Cantidad = 1;
                    carrito.Carrito.Add(dulceria);
                    //serializa carrito
                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carrito));
                }
            }
            else
            {
                ML.Dulceria dulceria = (ML.Dulceria)result.Object;
                GetCarrito(carrito); //recuperamos el carrito
                foreach (ML.Dulceria dulceria1 in carrito.Carrito)
                {
                    if (dulceria.IdDulce == dulceria1.IdDulce)
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
                if (existe == true)
                {
                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carrito));
                }
                else
                {
                    dulceria.Cantidad = 1;
                    carrito.Carrito.Add(dulceria);
                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carrito));
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult Agregar(int idDulce)
        {
            bool existe = false;
            ML.Venta carrito = new ML.Venta();
            carrito.Carrito = new List<object>();

            ML.Result result = BL.Dulceria.GetById(idDulce);
            ML.Dulceria dulceria = (ML.Dulceria)result.Object;
            GetCarrito(carrito); //recuperamos el carrito

            foreach (ML.Dulceria dulceria1 in carrito.Carrito)
            {
                if (dulceria.IdDulce == dulceria1.IdDulce)
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
            if (existe == true)
            {
                HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carrito));
            }
            else
            {
                dulceria.Cantidad = 1;
                carrito.Carrito.Add(dulceria);
                HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carrito));
            }
            return RedirectToAction("Carrito");
        }

        public IActionResult Quitar(int idDulce)
        {
            bool existe = false;
            ML.Venta carrito = new ML.Venta();
            carrito.Carrito = new List<object>();

            ML.Result result = BL.Dulceria.GetById(idDulce);
            ML.Dulceria dulceria = (ML.Dulceria)result.Object;
            GetCarrito(carrito); //recuperamos el carrito

            foreach (ML.Dulceria dulceria1 in carrito.Carrito)
            {
                if (dulceria.IdDulce == dulceria1.IdDulce)
                {
                    dulceria1.Cantidad -= 1;
                    existe = true;
                    if(dulceria1.Cantidad == 0)
                    {
                        QuitarElemento(idDulce);
                    }
                    break;
                }
                else
                {
                    existe = false;
                }
            }
            if (existe == true)
            {
                HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carrito));
            }
            else
            {
                dulceria.Cantidad = 1;
                carrito.Carrito.Add(dulceria);
                HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carrito));
            }
            return RedirectToAction("Carrito");
        }

        public IActionResult QuitarElemento(int idDulce)
        {
            ML.Venta carrito = new ML.Venta();
            carrito.Carrito = new List<object>();
            ML.Result result = BL.Dulceria.GetById(idDulce);
            if (HttpContext.Session.GetString("Carrito") != null)
            {
                ML.Dulceria dulceria = (ML.Dulceria)result.Object;
                GetCarrito(carrito);
                foreach (ML.Dulceria item in carrito.Carrito)
                {
                    if(dulceria.IdDulce  == item.IdDulce)
                    {
                        carrito.Carrito.Remove(item);
                        HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carrito));
                        break;
                    }
                }
            }
            return RedirectToAction("Carrito");
        }

        public IActionResult DeleteCarro()
        {
            if (HttpContext.Session.GetString("Carrito") != null)
            {
                HttpContext.Session.Remove("Carrito");
            }

                return RedirectToAction("Index");
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

        public IActionResult Carrito()
        {
            ML.Venta carrito = new ML.Venta();
            carrito.Carrito = new List<object>();
            if (HttpContext.Session.GetString("Carrito")==null)
            {
                return View(carrito);
            }
            else
            {
                GetCarrito(carrito);
                return View(carrito);
            }
        }

        public IActionResult CrearPdf()
        {
            ML.Venta venta = new ML.Venta();
            venta.Carrito = new List<object>();
            GetCarrito(venta);

            //creamo documento pdf con una ruta temporal
            string temporal = Path.GetTempFileName() + ".pdf";
			PdfDocument document = new PdfDocument(new PdfWriter(temporal));

            //crea un objeto layoutpara agregar contenido al documento
            iText.Layout.Document doc = new iText.Layout.Document(document);

            doc.Add(new Paragraph("Lista de productos registrados"));

            //creamos la tabla para crear la lista de objetos
            iText.Layout.Element.Table table = new iText.Layout.Element.Table(4); //5 columnas

            // añadir las celdas al encabezado de la tabla
            table.AddHeaderCell("IdDulce");
            table.AddHeaderCell("Producto");
            table.AddHeaderCell("Precio");
            table.AddHeaderCell("Cantidad");

            //añadimos filas a la tabla con los datos de nuestro carrito
            foreach (ML.Dulceria dulceria in venta.Carrito)
            {
                table.AddCell(dulceria.IdDulce.ToString());
                table.AddCell(dulceria.Nombre);
                table.AddCell(dulceria.Precio.ToString());
                table.AddCell(dulceria.Cantidad.ToString());
            }
            
            //doc.Add(new Paragraph("El total a pagar por la compra fue de: " + venta.Total));

            if (doc != null && table != null)
            {
                doc.Add(table);

                doc.Close();
                DeleteCarro();
            }

            //obtener el contenido del archivo como un arreglo de bytes
            byte[] filebytes = System.IO.File.ReadAllBytes(temporal);

            //eliminanr archivo temporal
            System.IO.File.Delete(temporal);

            //descargar archivo pdf
            return new FileStreamResult(new MemoryStream(filebytes), "application/pdf")
            {
                FileDownloadName = "Ticket.pdf"
			};
        }
    }
}
