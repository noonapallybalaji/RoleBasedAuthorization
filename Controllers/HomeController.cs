using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoleBasedAuthorization.Helper;
using RoleBasedAuthorization.Models;
using RoleBasedAuthorization.ViewModel;
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


        public ActionResult Dashboard()
        {
            var cards = new List<CardModel>
            {
                new CardModel
                {
                    Title = "Kids",
                    Content = "This is page is only for kid whose age is below 10 years",
                    ButtonText = "Go to kids page",
                    ButtonLink = Url.Action("Kids", "Home")
                },
                new CardModel
                {
                    Title = "Teenagers",
                    Content = "This is page is only for Teenagers whose age is below 18 years.",
                    ButtonText = "Go to Teenagers Page",
                    ButtonLink = Url.Action("Teenagers", "Home")
                },
                new CardModel
                {
                    Title = "Adults",
                    Content = "This is page is only for Adults whose age is Above 18 years.",
                    ButtonText = "Go to Adults Page",
                    ButtonLink = Url.Action("Adults", "Home")
                },
                new CardModel
                {
                    Title = "Indians",
                    Content = "This is page is only for Indian citizens.",
                    ButtonText = "Go to Indians Page",
                    ButtonLink = Url.Action("Indians", "Home")
                },
                new CardModel
                {
                    Title = "US",
                    Content = "This is page is only for Us citizens.",
                    ButtonText = "Go to US Page",
                    ButtonLink = Url.Action("US", "Home")
                },
                new CardModel
                {
                    Title = "Cognine",
                    Content = "This is page is only for Cogniners.",
                    ButtonText = "Go to Cognine Page",
                    ButtonLink = Url.Action("Cognine","Home")
                },
            };

            return View(cards);
        }


        [Authorize(Policy = "Kids")]
        public IActionResult Kids()
        {
            return View();
        }
        [Authorize(Policy = "AtLeast13")]
        public IActionResult Teenagers()
        {
            return View();
        }
        [Authorize(Policy = "AtLeast18")]
        public IActionResult Adults()
        {
            return View();
        }
        public IActionResult Indians()
        {
            return View();
        }
        public IActionResult US()
        {
            return View();
        }
        [CustomAuthorize("Admin")]
        public IActionResult Cognine()
        {
            return View();
        }



    }
}
