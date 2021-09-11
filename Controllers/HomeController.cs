using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Inmobiliaria_Peluffo.Models;

namespace Inmobiliaria_Peluffo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if(TempData.ContainsKey("Mensaje")){
                ViewBag.Mensaje = TempData["Mensaje"];
            }
            if(TempData.ContainsKey("Error")){
                ViewBag.Mensaje = TempData["Error"];
            }
            if(TempData.ContainsKey("StackTrate")){
                ViewBag.StackTrate = TempData["StackTrate"];
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Restringido(){
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
