using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Test.Data;
using Test.Models;

namespace Test.Controllers
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
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult MainPage()
        {
            TextParser textParser = new TextParser();
            Infrastructure infrastructure = textParser.ParseDocument();

            return View(infrastructure);
        }
        
        public IActionResult Choise() 
        { 
            return View();
        }

        public IActionResult MapRoute(string[] InfrastructuresNames)
        {
            

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}