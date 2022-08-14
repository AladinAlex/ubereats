using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Drawing;
using System.Security.Claims;
using ubereats.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

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
        //[Authorize]
        public IActionResult Autorization()
        {
            // claim - требование, что требуется для jwt-токена
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, "admin"), // пользователь
                new Claim(JwtRegisteredClaimNames.Email, "admin@mail.ru")
            };

            byte[] secretBytes = Encoding.UTF8.GetBytes(Constants.secretKey); // секретная фраза для key
            var key = new SymmetricSecurityKey(secretBytes);

            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(Constants.issuer,
                Constants.audience,
                claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(60), // токен действителен час с момента создания
                signingCredentials: signingCredentials // алгоритм шифрования
                );

            var value = new JwtSecurityTokenHandler().WriteToken(token);

            ViewBag.Token = value;
            return View("~/Views/Autorization.cshtml");
        }

        [Authorize]
        public IActionResult Rest(int ID)
        {
            ViewBag.Search = null;
            Restaurant? rest = db.Restaurants.Where(r => r.ID == ID).FirstOrDefault();
            rest.Image = null;
            ViewBag.Title = rest.RestName;
            return View("~/Views/Home/Rest.cshtml", rest);
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