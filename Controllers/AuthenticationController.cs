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
        readonly RestaurantContext db;
        public AuthenticationController(RestaurantContext context)
        {
            db = context;
        }
        [HttpPost]
        public IActionResult Login(User user) // авторизоваться
        {
            if(ModelState.IsValid) // если валидный объект User
            {
                return Redirect("/");
            }
            return View("~/Views/Authentication/Authentication.cshtml");
        }

        public IActionResult Index()
        {
            ViewBag.Token = JwtConfiguration.GetJwtSecurityToken();
            return View("~/Views/Authentication/Authentication.cshtml");
        }
    }
}
