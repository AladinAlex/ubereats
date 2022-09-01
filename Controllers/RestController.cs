using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ubereats.Models;
namespace ubereats.Controllers
{
    public class RestaurantController : Controller
    {
        RestaurantContext db;
        public RestaurantController(RestaurantContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        //[Authorize]
        public IActionResult Restaurant(int ID)
        {
            ViewBag.Search = null;
            Restaurant? rest = db.Restaurants.Where(r => r.ID == ID).FirstOrDefault();
            rest.Image = null;
            ViewBag.Title = rest.RestName;
            return View("~/Views/Home/Rest.cshtml", rest);
        }

        public ActionResult GetImage(int id)
        {
            byte[] imageData = db.Restaurants.First(r => r.ID == id).Image;
            return File(imageData, "image/jpg");
        }
    }
}
