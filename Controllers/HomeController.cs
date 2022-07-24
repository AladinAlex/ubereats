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
            //RestaurantContext db = new RestaurantContext();
            //ViewBag.Resraurants = db.Restaurants.ToList().Where(r => r.isDeleted == false);
            return View(db.Restaurants.ToList().Where(r => r.isDeleted == false));
            //return View();
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
            //var imageData = db.Restaurants.Where(r => r.ID == id).Select(r => r.Image);
            byte[] imageData = db.Restaurants.First(r => r.ID == id).Image;
            return File(imageData, "image/jpg");
            //return new FileStreamResult(new System.IO.MemoryStream(imageData), "image/png");
            //return View();
        }
    }
}