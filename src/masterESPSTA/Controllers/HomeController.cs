using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using masterESPSTA.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace masterESPSTA.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHostingEnvironment _env;

        public HomeController(ILogger<HomeController> logger, IHostingEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("acerca-de-nosotros")]
        public IActionResult About()
        {
            return View();
        }
        [Route("funcionarios")]
        public IActionResult Employes()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public FileResult FunctionManual()
        {
            return File("/files/manual-de-funciones.pdf", "application/pdf", "manual-de-funciones-empresa-de-servicios-publicos-de-santa-fe-de-antioquia.pdf");

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
