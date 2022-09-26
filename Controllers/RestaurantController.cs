using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ubereats.DAL.Context;
using ubereats.DAL.Repository;
using ubereats.Models;
namespace ubereats.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly RestaurantContext db;
        private readonly ILogger<RestaurantController> _logger;
        private readonly RestaurantRepository restaurantRepository;
        public RestaurantController(RestaurantContext context, ILogger<RestaurantController> logger)
        {
            _logger = logger;
            db = context;
            restaurantRepository = new RestaurantRepository(db);
        }

        [HttpGet]
        public async Task<IActionResult> Restaurant(int ID)
        {
            // Пример логирования
            _logger.LogInformation(message: $"RestID = {ID}");

            var rest = await restaurantRepository.GetAsync(ID);
            ViewBag.Title = rest.RestName;
            ViewBag.Search = null;
            return View("~/Views/Rest/Rest.cshtml", rest);
        }

        [HttpGet]
        public async Task<ActionResult> GetImage(int id)
        {
            byte[] imageData = await restaurantRepository.GetImageByteById(id);
            return File(imageData, "image/jpg");
        }
    }
}
