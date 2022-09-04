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
        [HttpPost("/Login")]
        public IActionResult Login(User _user) // авторизоваться
        {
            if (ModelState.IsValid) // если валидный объект User
            {
                //https://metanit.com/sharp/aspnet5/23.7.php
                User user = db.Users.FirstOrDefault(u => u.loginname == _user.loginname && u.password == _user.password);
                if (user != null)
                {
                    var jwt = JwtConfiguration.GetJwtSecurityToken(user.loginname, user.email);
                    var responce = new
                    {
                        access_token = jwt,
                        username = user.loginname
                    };
                    return Json(responce);
                    //return Redirect("/");
                }
            }
            return View("~/Views/Authentication/Authentication.cshtml");
        }

        //[HttpPost("/token")]
        public IActionResult Index()
        {
            ViewBag.Token = JwtConfiguration.GetJwtSecurityToken();
            return View("~/Views/Authentication/Authentication.cshtml");
        }
    }
}
