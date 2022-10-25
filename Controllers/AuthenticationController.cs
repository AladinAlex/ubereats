using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ubereats.Models.Authentication.JWT;
using ubereats.Models.Authentication.User;
using ubereats.Models.Context;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http;

namespace ubereats.Controllers
{
    public class
    AuthenticationController : Controller
    {
        private readonly RestaurantContext db;
        private readonly IJwtConfiguration _jwtConfiguration;
        private readonly IUserRepository userRepository;
        //private readonly HttpContext httpContext;

        public AuthenticationController(RestaurantContext context, IJwtConfiguration jwtConfiguration)
        {
            db = context;
            userRepository = new UserRepository(db);
            _jwtConfiguration = jwtConfiguration;
        }

        [AllowAnonymous]
        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(User _user)
        {
            if (ModelState.IsValid) // если валидный объект User
            {
                User user = await userRepository.GetAsync(_user);
                if (user != null)
                {
                    var token = _jwtConfiguration.GenerateToken(user.ID, user.loginname, user.password);

                    HttpContext.Session.SetString("Token", token);

                    //ViewBag.Token = token;
                    //return Redirect("/");
                    if (token == null)
                        return Unauthorized();
                    else
                        return Ok(token);
                    //return Redirect("/");
                    //return View("~/Views/Home/Index.cshtml");
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
