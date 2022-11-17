using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ubereats.Models.Context;
using ubereats.Models.Rest;

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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Restaurant(int ID)
        {
            if(!HttpContext.User.Identity.IsAuthenticated)
                return View("~/Views/Authentication/Authentication.cshtml");
            //string? token = HttpContext.Session.GetString("Token");

            //if (string.IsNullOrEmpty(token) || !_jwtConfiguration.IsTokenValid(token))
            //    return View("~/Views/Authentication/Authentication.cshtml");

            // Пример логирования
            //_logger.LogInformation(message: $"RestID = {ID} \n Token = {token}");

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
