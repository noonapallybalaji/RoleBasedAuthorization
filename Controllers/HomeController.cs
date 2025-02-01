using Microsoft.AspNetCore.Mvc;
using RoleBasedAuthorization.Models;
using System.Diagnostics;

namespace RoleBasedAuthorization.Controllers
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


    }
}
