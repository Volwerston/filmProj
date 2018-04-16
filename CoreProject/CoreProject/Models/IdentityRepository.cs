using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FilmDatabase.Models
{
    public class IdentityRepository:IIdentityRepository//used in controller
    {

        private ApplicationDbContext context;
        public RoleManager<IdentityRole> roleManager;

        public IdentityRepository(ApplicationDbContext _context, RoleManager<IdentityRole> rm)
        {
            context = _context;
            roleManager = rm;
        }


        public ApplicationUser FindById(string id)//when you want to change role, it is used to find it in controller
        {
            return context.Users.Single(i => i.Id == id);
        }

        public List<ApplicationUser> Users
        {
            get { return context.Users.ToList(); }
        }

        
        public void SaveChanges()
        {
            context.SaveChanges();
        }


        public List<ApplicationUser> GetUsersInRole(string role)
        {
            var role1 = context.Roles.SingleOrDefault(m => m.Name == "user");
            var usersInRole = context.Users.Where(m => roleManager.FindByIdAsync(m.Id) != null).ToList();
            return usersInRole;
        }


        public List<IdentityRole> Roles
        {
            get
            {
                return context.Roles.ToList();
               
            }
            set
            {
              
            }
        }


        public IdentityRole FindRole(string role)
        {
            return context.Roles.Where(a => a.Name == role).Single();
        }
    }
}