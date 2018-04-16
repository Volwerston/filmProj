using System.Linq;
using System.Security.Claims;
using FilmDatabase.Filters;
using FilmDatabase.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace FilmDatabase.Controllers
{
    [Culture]
    public class AdminController : Controller
    {
        IFilmRepository repo;
        IIdentityRepository repos;
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;

        public AdminController(IFilmRepository context, IIdentityRepository repository, UserManager<ApplicationUser> userMgr, RoleManager<IdentityRole> rm)
        {
            repo = context;
            repos = repository;
            userManager = userMgr;
            roleManager = rm;
        }

        private string GetUserId() => this.User.FindFirstValue(ClaimTypes.NameIdentifier);

        //[Authorize(Roles = "admin")]
        //public ViewResult ViewUsers()
        //{
        //   foreach(var user in repos.Users)
        //   {
        //       if(user.Roles.Count==0)
        //       {
        //           userManager.AddToRoleAsync(user, "user");
        //       }
        //   }
        //    ViewBag.UserCount = repos.GetUsersInRole("user").Count;
        //    return View(repos.Users.Where(a=>a.Id!=GetUserId()).ToList());

        //}

        [Authorize(Roles = "admin")]
        public ActionResult Ban(string id)
        {

            var user = repos.FindById(id);

            if (user.Blocked)
            {
                user.Blocked = false;
            }
            else
            {
                user.Blocked = true;
            }

            repos.SaveChanges();
            return RedirectToAction("ViewUsers", "Admin");
        }

        //[Authorize(Roles = "admin")]
        //public ActionResult PromoteToRole(string id, string role)
        //{
        //  //  var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            
        //    var user = repos.FindById(id);
        //    var role1 = repos.FindRole(role);
        //    user.Roles.Clear();
        //    IdentityUserRole userRole = new IdentityUserRole { RoleId = role1.Id, UserId = user.Id };
        //    user.Roles.Add(userRole);
        //    repos.SaveChanges();

           
          
        //  return RedirectToAction("ViewUsers", "Admin");

        //}

    }
}