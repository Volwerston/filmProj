using System;
using System.Linq;
using System.Threading.Tasks;
using FilmDatabase.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using FilmDatabase.Filters;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace FilmDatabase.Controllers
{
    [Culture]
    [Authorize]
    public class AccountController : Controller
    {

        private IIdentityRepository repo;

        public AccountController(IIdentityRepository _r)
        {
            repo = _r;
        }

        private string GetUserId() => this.User.FindFirstValue(ClaimTypes.NameIdentifier);

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> sm)
        {
            UserManager = userManager;
            SignInManager = sm;
        }



        public UserManager<ApplicationUser> UserManager { get; private set; }
        public SignInManager<ApplicationUser> SignInManager { get; private set; }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, model.RememberMe);
                    if (String.IsNullOrEmpty(returnUrl))
                    {
                        if (await UserManager.IsInRoleAsync(user, "admin"))
                        {
                            return RedirectToAction("ViewUsers", "Admin");
                        }

                        if (await UserManager.IsInRoleAsync(user, "moderator"))
                        {
                            return RedirectToAction("Index", "Moderator");
                        }

                        if (await UserManager.IsInRoleAsync(user, "user"))
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = model.UserName,
                    Blocked = false,
                    Age = model.Age,
                    Name = model.Name,
                    Surname = model.Surname,
                    City = model.City,
                    Email = model.Email
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user, "user");
                    await SignInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult EditInfo()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditInfo(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                string id = GetUserId();
                var user = repo.Users.First(a => a.Id == id);

                if (!string.IsNullOrEmpty(model.Name))
                {
                    user.Name = model.Name;
                }

                if (!string.IsNullOrEmpty(model.Surname))
                {
                    user.Surname = model.Surname;
                }

                if (!string.IsNullOrEmpty(model.City))
                {
                    user.City = model.City;
                }

                if (!string.IsNullOrEmpty(model.Email))
                {
                    user.Email = model.Email;
                }

                repo.SaveChanges();
            }

            return RedirectToAction("Index", "Home");

        }


    }

}