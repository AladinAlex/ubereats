using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ubereats.DAL.Context;
using ubereats.DAL.Interfaces;
using ubereats.DAL.Repository;
using ubereats.Models;

namespace ubereats.Controllers
{
    public class HomeController : Controller
    {
        private readonly RestaurantContext db;
        private readonly ILogger<HomeController> logger;
        private readonly RestaurantRepository restaurantRepository;
        //private readonly HttpContext httpContext;


        public HomeController(RestaurantContext _context, ILogger<HomeController> _logger)
        {
            logger = _logger;
            db = _context;
            restaurantRepository = new RestaurantRepository(db);
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "restaurants";
            ViewBag.Search = null;
            var restaurants = await restaurantRepository.Select();
            return View(restaurants);
        }

        //[HttpGet]
        /// <summary>
        /// Поиск ресторана по имени и кухне
        /// </summary>
        /// <param name="restName"></param>
        /// <returns></returns>
        public async Task<IActionResult> SearchRestaurant(string restName)
        {
            ViewBag.Title = "restaurants";
            ViewBag.Search = restName;
            var restaurants = await restaurantRepository.GetForSearch(restName);
            return View("~/Views/Home/Index.cshtml", restaurants);
        }

        [HttpGet]
        public async Task<ActionResult> GetImage(int id)
        {
            byte[] imageData = await restaurantRepository.GetImageByteById(id);
            return File(imageData, "image/jpg");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}