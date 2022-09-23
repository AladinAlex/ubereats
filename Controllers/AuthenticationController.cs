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
using ubereats.DAL.Context;
using ubereats.Models.DAL.Repository;

namespace ubereats.Controllers
{
    public class
    AuthenticationController : Controller
    {
        private readonly RestaurantContext db;
        private readonly UserRepository userRepository;
        //private readonly HttpContext httpContext;

        public AuthenticationController(RestaurantContext context)
        {
            db = context;
            userRepository = new UserRepository(db);
        }

        [HttpPost]
        public async Task<IActionResult> Login(User _user) // авторизоваться
        {
            if (ModelState.IsValid) // если валидный объект User
            {
                User user = await userRepository.GetAsync(_user);
                if (user != null)
                {
                    var jwt = await JwtConfiguration.GetJwtSecurityToken(user.loginname, user.email);
                    ViewBag.Token = jwt;
                    return Redirect("/");
                }
            }
            return View("~/Views/Authentication/Authentication.cshtml");
        }

        //[HttpPost("/token")]
        public async Task<IActionResult> Index()
        {
            //ViewBag.Token = JwtConfiguration.GetJwtSecurityToken();
            return View("~/Views/Authentication/Authentication.cshtml");
        }
    }
}
