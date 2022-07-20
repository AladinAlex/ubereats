﻿using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ubereats.Models;

namespace ubereats.Controllers
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
            ViewBag.Title = "Uber Eats";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Autorization()
        {
            return View("~/Views/Autorization.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}