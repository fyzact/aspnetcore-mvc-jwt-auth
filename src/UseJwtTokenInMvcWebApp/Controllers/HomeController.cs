using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UseJwtTokenInMvcWebApp.Helper;
using UseJwtTokenInMvcWebApp.Models;

namespace UseJwtTokenInMvcWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
       private readonly IHttpContextAccessor _contextAccessor;
        readonly JwtHelper _jwtHelper;
        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor IHttpContextAccessor, JwtHelper jwtHelper)
        {
            _logger = logger;
            _contextAccessor = IHttpContextAccessor;
            _jwtHelper = jwtHelper;
        }
    

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles ="Admin")]
        public IActionResult Privacy()
        {
            //Get user informatin
            var name = _contextAccessor.HttpContext.User;
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel loginModel)
        {
            var model = user.FirstOrDefault(p => p.UserName == loginModel.UserName && p.Password == loginModel.Password);
            if (model is null) return View("Index", loginModel);

            var token = _jwtHelper.CreateAuthenticationTicket(loginModel);
            _contextAccessor.HttpContext.Response.Cookies.Append("jwt-token", token);

            return RedirectToAction("Privacy"); 

        }

        readonly List<LoginModel> user =
            new List<LoginModel> {
                new LoginModel { UserName = "feyyaz", Password = "123456" },
                new LoginModel { UserName = "Ismail", Password = "123456" }
            };


    }

    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
