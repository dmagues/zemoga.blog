using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using zemoga.blog.webui.Models;
using zemoga.blog.webui.Business.REST;

namespace zemoga.blog.webui.Controllers
{
    public class UserController : Controller
    {
        private IUserRestApiClient _userRestApiClient;

        public UserController(IUserRestApiClient userRestApiClient)
        {
            this._userRestApiClient = userRestApiClient;
        }

        public IActionResult Login(string ReturnUrl = "/")
        {
            UserModel userModel = new()
            {
                ReturnUrl = ReturnUrl
            };
            return View(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserModel model)
        {
            if (ModelState.IsValid)
            {
                UserModel user = await this._userRestApiClient.Login(model.Username, model.Password);

                if (user == null)
                {
                    //Add logic here to display some message to user    
                    ViewBag.Message = "Invalid Credential";
                    return View(model);
                }
                else
                {
                    
                    string tokenEncoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
                               .GetBytes($"{model.Username}:{model.Password}"));
                     

                    var claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.UserId)),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim("BasicToken", tokenEncoded)
                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    // user roles
                    foreach (var r in user.Roles)
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, r));
                    }
                    //Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity    
                    var principal = new ClaimsPrincipal(identity);
                    //SignInAsync is a Extension method for Sign in a principal for the specified scheme.    
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                    {
                        IsPersistent = model.RememberLogin
                    });
                    return LocalRedirect(model.ReturnUrl);

                }                
                
            }
            return View(model);
        }

        public async Task<IActionResult> LogOut()
        {                
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //Redirect to home page    
            return LocalRedirect("/");
        }


        public IActionResult Register()
        {
            RegisterModel registerModel = new();
            return View(registerModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                await this._userRestApiClient.Register(model);
                return LocalRedirect("/User/Login");

            }
            return View(model);
        }
    }
}
