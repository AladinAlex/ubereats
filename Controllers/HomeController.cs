using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Drawing;
using ubereats.Models;

namespace ubereats.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        RestaurantContext db;

        public HomeController(RestaurantContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            ViewBag.Title = "restaurants";
            ViewBag.Search = null;
            //RestaurantContext db = new RestaurantContext();
            //ViewBag.Resraurants = db.Restaurants.ToList().Where(r => r.isDeleted == false);
            var list = db.Restaurants.Where(r => r.isDeleted == false).ToList();
            list.ForEach(rest => rest.Image = null);    // чтобы не нагружать, все равно массив байт Image там не используется
            return View(list);
            //return View();
        }

        /// <summary>
        /// Поиск ресторана по имени и кухне
        /// </summary>
        /// <param name="restName"></param>
        /// <returns></returns>
        public IActionResult SearchRestaurant(string restName)
        {
            ViewBag.Title = "restaurants";
            ViewBag.Search = restName;
            var list = db.Restaurants.ToList();
            if (!String.IsNullOrEmpty(restName))
                list = list.Where(r => r.isDeleted == false && (r.RestName.ToLower().Contains(restName.ToLower()) || r.KitchenType.ToLower().Contains(restName.ToLower()))).ToList();
            else
                list = list.Where(r => r.isDeleted == false).ToList();
            list.ForEach(rest => rest.Image = null);    // чтобы не нагружать, все равно массив байт Image там не используется
            return View("~/Views/Home/Index.cshtml", list);
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

        public ActionResult GetImage(int id)
        {
            byte[] imageData = db.Restaurants.First(r => r.ID == id).Image;
            return File(imageData, "image/jpg");
        }
    }
}