
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
     
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<ApplicationUser> userManager
                              ,RoleManager<IdentityRole> roleManager
                              ,AppDbContext context)
        {
           
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user == null) return Content("NULL");

            var users = _userManager.Users.Select(c => new ViewModel.UserVM
            {
                Username = c.UserName,
                Email = c.Email,
                FullName = c.FullName,
               // RoleName = (_userManager.GetRolesAsync(c).Result.ToArray()).ToString(),

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
            //                                       join role in _context.Roles on userRole.ToString()
            //                                       equals role.Id
            //                                       select role.Name).ToList()
            //                      }).ToList().Select(p => new UserListVM()

            //                      {

            //                          UserName = p.Username,
            //                          Email = p.Email,
            //                          RoleName = string.Join(",", p.RoleNames)
            //                      });
            //return View(usersWithRoles);


        }
      
    }
}
