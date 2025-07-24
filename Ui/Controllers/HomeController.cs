using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Ui.Models;
using BL.Services;
using BL.Contracts;

namespace Ui.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICity _city;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ICity city, ILogger<HomeController> logger)
        {
            _city = city;
            _logger = logger;
           
        }

        public IActionResult Index()
        {
            _city.GetAll();
            return View();
        }

        public IActionResult Privacy()
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
