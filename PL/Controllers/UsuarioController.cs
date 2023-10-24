using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Web;

namespace PL.Controllers
{
	public class UsuarioController : Controller
	{
		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Login(string email, string password)
		{
			ML.Usuario usuario = new ML.Usuario();
			usuario.Email = email;
			usuario.Password = BL.Usuario.Encrip256(password);
			ML.Result result = BL.Usuario.GetByEmail(email);

			if (result.Correct)
			{
				
					ViewBag.Login = true;
					return RedirectToAction("Index", "Home");
				
			}
			else
			{
				ViewBag.Login = false;
				ViewBag.Mensaje = "Hubo un error";
				return PartialView("modal");
			}
		}

		[HttpPost]
		public IActionResult Add(ML.Usuario usuario)
		{
			ML.Result result = BL.Usuario.Add(usuario);

			if (result.Correct)
			{
                ViewBag.Login = true;
                return RedirectToAction("Login", "Usuario");
            }
            else
			{
                ViewBag.Login = false;
                ViewBag.Mensaje = "Hubo un error";
                return PartialView("modal");
            }
		}

		[HttpGet]
		public IActionResult Olvide()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Olvide(string email)
		{
			//ML.Usuario usuario = new ML.Usuario();
			//usuario.Email = email;
			ML.Result result = BL.Usuario.GetByEmail(email);
			if (result.Correct)
			{
				ViewBag.Login = true;
				ViewBag.Menssaje = "Se envio un mail a su correo por favor verifique";
				CambiarPassword(email);
			}
			else
			{
				ViewBag.Login = false;
				ViewBag.Mensaje = "Hubo un error";
			}
			return PartialView("modal");
		}


		[HttpPost]
		public ActionResult CambiarPassword(string email)
		{
			//llamar al metodo
			string emailOrigen = "jgarciar271@gmail.com";

			string cuerpo = $"<h1>Bienvenido</h1>" + "<br />" + "<p>Si usted solicito una recuperacion de contraseña continue al enlace, de caso contrario ignore este correo</p>" +
				$"<center><a href=" + "http://localhost:5028/Usuario/NewPassword?email=" + email + ">Recuperar contraseña </a></center>";

			MailMessage mailMessage = new MailMessage(emailOrigen, email, "Recuperar Contraseña", cuerpo);
			mailMessage.IsBodyHtml = true;

			//string contenidoHtml = System.IO.File.ReadAllText(Path.Combine("Views", "Usuario", "Email.html"));

			//string contenidoHtml = System.IO.File.ReadAllText(Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", "Templates", "Email.html"));

			//mailMessage.Body = contenidoHtml;
			string url = "http://localhost:5028/Usuario/NewPassword/" + HttpUtility.UrlEncode(email);

			//string url = "http://192.168.0.104/Usuario/NewPassword/" + HttpUtility.UrlEncode(email);
			mailMessage.Body = mailMessage.Body.Replace("{Url}", url);

			SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
			smtpClient.EnableSsl = true;
			smtpClient.UseDefaultCredentials = false;
			smtpClient.Port = 587;
			smtpClient.Credentials = new System.Net.NetworkCredential(emailOrigen, "yubzutshntvzsnup");

			smtpClient.Send(mailMessage);
			smtpClient.Dispose();

			ViewBag.Login = false;
			ViewBag.Modal = "show";
			ViewBag.Mensaje = "Se ha enviado un correo de confirmación a tu correo electronico";
			return RedirectToAction("Olvide","Usuario");
		}

		[HttpGet]
		public IActionResult NewPassword(string email) { 
			ML.Usuario usuario = new ML.Usuario();
			usuario.Email = email;
			return View(usuario); 
		}

		[HttpPost]
		public IActionResult Actualizar(ML.Usuario usuario) 
		{
			ML.Result resultEmail = BL.Usuario.GetByEmail(usuario.Email);
			if (resultEmail.Correct)
			{
                ML.Result result = BL.Usuario.Actualizar(usuario.Email, usuario.Password);
                if (result.Correct)
                {
                    ViewBag.Login = true;
                    return RedirectToAction("Login", "Usuario");
                }
                else
                {
                    ViewBag.Login = false;
                    ViewBag.Mensaje = "Hubo un error";
                    return PartialView("modal");
                }
            }
			else
			{
				ViewBag.Login = false;
                ViewBag.Mensaje = "Hubo un error";
                return PartialView("modal");
            }

			
		}

	}
}
