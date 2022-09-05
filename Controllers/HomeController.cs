using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ubereats.Interfaces;
using ubereats.Models;

namespace ubereats.Controllers
{
    public class HomeController : Controller, IRestaurant
    {
        private readonly RestaurantContext db;
        //private readonly HttpContext httpContext;
        

        public HomeController(RestaurantContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "restaurants";
            ViewBag.Search = null;
            var list = db.Restaurants.Where(r => r.isDeleted == false).ToList();
            list.ForEach(rest => rest.Image = null);    // чтобы не нагружать, все равно массив байт Image там не используется
            return View(list);
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
            List<Restaurant> list;
            if (!String.IsNullOrEmpty(restName))
                list = db.Restaurants.Where(r => r.isDeleted == false && (r.RestName.ToLower().Contains(restName.ToLower()) || r.KitchenType.ToLower().Contains(restName.ToLower()))).ToList();
            else
                list = db.Restaurants.Where(r => r.isDeleted == false).ToList();
            list.ForEach(rest => rest.Image = null);    // чтобы не нагружать, все равно массив байт Image там не используется
            return View("~/Views/Home/Index.cshtml", list);
        }

        public ActionResult GetImage(int id)
        {
            byte[] imageData = db.Restaurants.First(r => r.ID == id).Image;
            return File(imageData, "image/jpg");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}