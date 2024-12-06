using Microsoft.AspNetCore.Mvc;
using SupportBilling.APPLICATION.Dtos;
using SupportBilling.Web.Models;
using SupportBilling.Web.Services;
using System.Diagnostics;

namespace SupportBilling.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Reports()
        {
            // Aquí puedes obtener los datos de reportes desde la API y pasarlos al modelo
            return View();
        }
    }
}
