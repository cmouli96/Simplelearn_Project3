using e_commerce.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;


namespace e_commerce.Controllers
{
    public class AccountController : Controller
    {
        UserManager<IdentityUser> _userManager = null;
        SignInManager<IdentityUser> _signInManager = null;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                ViewBag.msg = "User registered successfully...";
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View();
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model,string returnUrl)
        {
           

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {

                //Session to be created after succeful login
                HttpContext.Session.SetString("uname", model.Email);


                if (!string.IsNullOrEmpty(returnUrl))
                {

                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Dashboard");
                }
            }

            return View(model);
        }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("uname") != null)
            {
                string name = HttpContext.Session.GetString("uname");
                ViewBag.msg = $"Hello {name}, Welcome to dashboard";
            }
            else
            {
                ViewBag.msg = "landed on this page using technique";
            }
            return View();
        }

        public IActionResult Logout()
        {

           

            if (HttpContext.Session.GetString("uname") != null)
            {
                string name = HttpContext.Session.GetString("uname");
                ViewBag.msg = $"Hello {name}, logged out successfully";
                //HttpContext.Session.SetString("uname", null);

                HttpContext.Session.Clear();
                Response.Cookies.Delete("LoginCookie");

                
            }

            else
            {
                ViewBag.msg = $"No User available to logout ...";
            }

            return View();

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {

            return View();
        }
    }
}
