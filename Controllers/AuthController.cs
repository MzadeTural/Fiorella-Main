using Fiorella_second.DAL;
using Fiorella_second.Models;
using Fiorella_second.ViewModel.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using NETCore.MailKit.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Fiorella_second.Utilities.File.Helper;

namespace Fiorella_second.Controllers
{
    public interface IEmailSender
    {
        void SendEmail(Models.Message message);
       // Task SendEmailAsync(Message message);
    }
    public class AuthController : Controller
    {

        // private readonly IMapper _mapper;
        public IEmailService _emailService { get; set; }
        public AppDbContext _context { get; }
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        public RoleManager<IdentityRole> _roleManager;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,IEmailService emailService, RoleManager<IdentityRole> roleManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
             _emailService = emailService;
            _context = context;
        }
        // GET: AuthController1
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if (!ModelState.IsValid) return View(register);

            ApplicationUser newUser = new ApplicationUser
            {
                FullName = register.FullName,
                Email = register.Email,
                UserName = register.UserName,

            };
            IdentityResult identityResult = await _userManager.CreateAsync(newUser, register.Pasword);
            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(register);
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
          
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { userId = newUser.Id , token },Request.Scheme,Request.Host.ToString());
            //return Json(confirmationLink);
            
            await _emailService.SendAsync("tural.memmedzade04@gmail.com", "email verify", confirmationLink);

           await _userManager.AddToRoleAsync(newUser,UserRoles.Admin.ToString());
             
            return RedirectToAction(nameof(SuccessRegistration));
           


            }
       
           
        public async Task<IActionResult> ConfirmEmail(string token, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return View("Error");

            var result = await _userManager.ConfirmEmailAsync(user, token);
          
            if (result.Succeeded)
            {
                user.IsActivated = true;
                await _context.SaveChangesAsync();
                return View();
            }
            return View(result.Succeeded ? nameof(ConfirmEmail) : "Error");
        }

        [HttpGet]
        public IActionResult SuccessRegistration()
        {
            return View();
        }

        
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginVM userModel, string returnUrl )
        {
            if (!ModelState.IsValid)
            {
                return View(userModel);
            }
            ApplicationUser user = await _userManager.FindByEmailAsync(userModel.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Email or Pasword is wrong");
                return View(userModel);
            }
            if (!user.IsActivated)
            {
                ModelState.AddModelError(string.Empty, "Please,Active your account.Check your Email");
                return View(userModel);
            }

            var result = await _signInManager.PasswordSignInAsync(user, userModel.Password, userModel.RememberMe, true);
            
            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Please,Wait a few moment");
                return View(userModel);
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Email or Pasword is wrong");
                return View(userModel);              
            }
            if (returnUrl!=null)
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
          
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        #region CreateROle
        public async Task CreateRole()
        {
            foreach (var role in Enum.GetValues(typeof(UserRoles)))
            {
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
                }
            }
        }
        #endregion
        // GET: AuthController1/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AuthController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuthController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AuthController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AuthController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AuthController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AuthController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
