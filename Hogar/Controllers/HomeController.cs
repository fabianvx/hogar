using Hogar.Application.Services.Interfaces;
using Hogar.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Hogar.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ServiceEmail _ServiceEmail;
        public HomeController(ILogger<HomeController> logger,ServiceEmail serviceEmail)
        {
            _logger = logger;
            _ServiceEmail = serviceEmail;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contribucion()
        {
            return View();
        }


        public IActionResult Contacto()
        {
            return View();
        }
        public IActionResult Noticias()
        {
            return View();
        }
        public IActionResult Nosotros()
        {
            return View();
        }

        public IActionResult Voluntariado()
        {
            return View();
        }
        public IActionResult Asociados()
        {
            return View();
        }
        public IActionResult Ingreso()
        {
            return View();
        }
  
        public IActionResult Servicios()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpPost]
        public async Task<IActionResult> EnviarConsulta(string name, string email, string subject, string message)
        {
            var body = $"Nombre: {name}\nCorreo: {email}\nAsunto: {subject}\n\nMensaje: {message}";
            //Este es el correo al que llegaría la consulta
            await _ServiceEmail.SendEmailAsync("hogarjafethjimenez@gmail.com", subject, body);

            // Puedes redirigir a una página de agradecimiento o mostrar un mensaje
            TempData["MensajeExito"] = "Tu consulta ha sido enviada exitosamente.";
            return RedirectToAction("Contacto");
        }
    }
}
