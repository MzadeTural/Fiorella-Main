
using Fiorella_second.DAL;
using Fiorella_second.Models;
using Fiorella_second.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

           

            var users = _userManager.Users.Select(c => new ViewModel.UserVM
            {
                Username = c.UserName,
                Email = c.Email,
                FullName = c.FullName,
                RoleName = (_context.Roles.ToArray()).ToString(),

                 // RoleName = string.Join(",", _userManager.GetRolesAsync(c).Result.ToArray())
            }).ToList();



            return View(users);
            //var usersWithRoles = (from user in _context.Users
            //                      select new
            //                      {
            //                          UserId = user.Id,
            //                          Username = user.UserName,
            //                          Email = user.Email,
            //                          RoleNames = (from userRole in user.Id
            //                                       join role in _context.Roles on userRole.
            //                                       equals role.Id
            //                                       select role.Name).ToList()
            //                      }).ToList().Select(p => new UserListVM()

            //                      {

            //                          Username = p.Username,
            //                          Email = p.Email,
            //                          RoleName = string.Join(",", p.RoleNames)
            //                      });
            //return View(usersWithRoles);


        }
      
    }
}
