using Fiorella_second.Areas.AdminFiorella.ViewModel;
using Fiorella_second.DAL;
using Fiorella_second.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fiorella_second.Areas.AdminFiorella.Controllers
{

    [Area("AdminFiorella")]
    [Authorize(Roles = "SuperAdmin")]
    public class UserController : Controller
    {
        public AppDbContext _context { get; set; }
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<ApplicationUser> userManager
                              ,RoleManager<IdentityRole> roleManager
                              ,AppDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            //List<UserListVM> model = new List<UserListVM>();
            //model = _userManager.Users.Select(u => new UserListVM
            //{
            //    UserName = u.UserName,
            //    Email = u.Email,
            //    FullName=u.FullName
            //}).ToList();
            //return View(model);
            
                var users = _userManager.Users.Select(c => new UserListVM()
                {
                    UserName = c.UserName,
                    Email = c.Email,
                    FullName = c.FullName,
                  //  RoleName = string.Join(",", _userManager.GetRolesAsync(c).Result.ToArray())
                }).ToList();


                return View(users);
           
            
         
        }
    }
}
