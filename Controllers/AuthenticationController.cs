using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Security.Claims;
using ubereats.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ubereats.Controllers
{
    public class
    AuthenticationController : Controller
    {
        RestaurantContext db;
        public AuthenticationController(RestaurantContext context)
        {
            db = context;
        }
        [HttpPost]
        public IActionResult Login(User user) // авторизоваться
        {
            if (ModelState.IsValid)
            {
               return Redirect("/");// переадресация на главную страницу
            }
            return View("Index");
        }

        public IActionResult Index()
        {
            // claim - требование, что требуется для jwt-токена
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, "admin"), // пользователь
                new Claim(JwtRegisteredClaimNames.Email, "admin@mail.ru")
            };

            var token = new JwtSecurityToken(Constants.issuer,
                Constants.audience,
                claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(60), // токен действителен час с момента создания
                signingCredentials: new SigningCredentials(Constants.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256) // алгоритм шифрования
                );

            var value = new JwtSecurityTokenHandler().WriteToken(token);

            ViewBag.Token = value;
            return View("~/Views/Authentication/Authentication.cshtml");
        }
    }
}
