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
       
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
      

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
         
            _emailService = emailService;
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
          
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { token, userId = newUser.Id }, Request.Scheme);
            return Json(confirmationLink);
             //var message = new Message(new string[] { newUser.Email }, "Confirmation email link", confirmationLink, null);
            await _emailService.SendAsync( newUser.Email , "email verify",confirmationLink);

            await _userManager.AddToRoleAsync(newUser, "Visitor");

            return RedirectToAction(nameof(SuccessRegistration));
           


            }
       
            [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return View("Error");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return View(result.Succeeded ? nameof(ConfirmEmail) : "Error");
        }

        [HttpGet]
        public IActionResult SuccessRegistration()
        {
            return View();
        }

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
