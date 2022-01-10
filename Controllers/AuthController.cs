using Fiorella_second.DAL;
using Fiorella_second.Models;
using Fiorella_second.ViewModel.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using NETCore.MailKit.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using static Fiorella_second.Utilities.File.Helper;

namespace Fiorella_second.Controllers
{

    [Authorize(Roles = "SuperAdmin,Admin")]
    public class AuthController : Controller
    {

        // private readonly IMapper _mapper;
        public IEmailService _emailService { get; set; }
        public AppDbContext _context { get; }
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        public RoleManager<IdentityRole> _roleManager;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailService emailService, RoleManager<IdentityRole> roleManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _context = context;
        }
        // GET: AuthController1
        [AllowAnonymous]
        public IActionResult Register()
        {
            IsAuthenticated();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
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

            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { userId = newUser.Id, token }, Request.Scheme, Request.Host.ToString());

            Email.SendEmail("tural.memmedzade025@gmail.com", newUser.Email, confirmationLink, "tural2025", "Email Confirmation");
            #region
            //using (var client = new SmtpClient("smtp.googlemail.com", 587))
            //{
            //    client.Credentials =
            //        new System.Net.NetworkCredential("tural.memmedzade025@gmail.com", "tural2025");
            //    client.EnableSsl = true;
            //    var msg = new MailMessage("tural.memmedzade025@gmail.com", newUser.Email);
            //    msg.Body = confirmationLink;
            //    msg.Subject = "Email Veryfication";

            //    client.Send(msg);
            //}
            //  await _emailService.SendAsync(register.Email, "email verify", confirmationLink);
            #endregion

            await _userManager.AddToRoleAsync(newUser, UserRoles.Member.ToString());

            return RedirectToAction(nameof(SuccessRegistration));



        }

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string token, string userId)
        {

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return View("Error");

            var result = await _userManager.ConfirmEmailAsync(user, token);
          
            var users = _userManager.Users.Select(c => new ViewModel.UserListVM()
            {

                Email = c.Email,

            }).ToList();
            if (result.Succeeded)
            {
                user.IsActivated = true;
                await _context.SaveChangesAsync();
                return View(users);
            }
            return View(result.Succeeded ? nameof(ConfirmEmail) : "Error");
        }

        [AllowAnonymous]
        public IActionResult SuccessRegistration()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            IsAuthenticated();
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginVM userModel, string returnUrl)
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
            if (returnUrl != null)
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

        #region CreateROle
        //public async Task CreateRole()
        //{
        //    foreach (var role in Enum.GetValues(typeof(UserRoles)))
        //    {
        //        if (!await _roleManager.RoleExistsAsync(role.ToString()))
        //        {
        //            await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
        //        }
        //    }
        //}
        #endregion
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPasswordVM { Token = token, Email = email };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordModel)
         {
            if (!ModelState.IsValid)
                return View(resetPasswordModel);

            var user = await _userManager.FindByEmailAsync(resetPasswordModel.Email);
            if (user == null)
                return Content("NULL");

            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);

            if (!resetPassResult.Succeeded)
            {
                
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(resetPasswordModel);
            }
           
            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM forgotPassword)
         {
            if (!ModelState.IsValid) return View(forgotPassword);

           // var user = await _userManager.FindByEmailAsync(forgotPassword.Email);
            ApplicationUser user = await _userManager.FindByEmailAsync(forgotPassword.Email);
            if (user == null) return Content("NULL"); 
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
           var confirmationLink = Url.Action(nameof(ResetPassword), "Auth", new { token, email = user.Email }, Request.Scheme);

            Email.SendEmail("tural.memmedzade025@gmail.com", user.Email, confirmationLink, "tural2025", "Reset Password");

            return RedirectToAction(nameof(ForgotPasswordConfirmation));
           
        }
            public IActionResult ForgotPasswordConfirmation()
                {
                    return View();
                }
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM changePassword)
        {
            if (!ModelState.IsValid) return View(changePassword);
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user == null) return Content("NULL");
           
            var changetPassResult = await _userManager.ChangePasswordAsync(user,changePassword.CurrentPassword,changePassword.NewPassword);
            if (!changetPassResult.Succeeded)
            {

                foreach (var error in changetPassResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(changePassword);
            }

            return RedirectToAction(nameof(Login));
        }
            // GET: AuthController1/Details/5
            public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AuthController1/Create
        public IActionResult ChangeUserName()
        {

            return View();
        }

        // POST: AuthController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserName(ChangeUserNameVM userNameVM)
        {
            if (!ModelState.IsValid) return View(userNameVM);
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User is Not Found");
                return View(userNameVM);
            }

            var changeUserName = await _userManager.SetUserNameAsync(user, userNameVM.UserName);
            if (!changeUserName.Succeeded)
            {

                foreach (var error in changeUserName.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(userNameVM);
            }
            await _signInManager.RefreshSignInAsync(user);
            return RedirectToAction(nameof(Profile));
        }

        // GET: AuthController1/Edit/5

        public async Task<IActionResult> Profile(string email)
        {
            
            ApplicationUser userI = await  _userManager.GetUserAsync(User);
            var user = _userManager.Users.Where(u=>u.Email==userI.Email).Select(c => new UserProfileVM
            {
                Username = c.UserName,
                Email = c.Email,
                FullName = c.FullName,
               
            }).FirstOrDefault();
            return View(user);
        }

        public IActionResult ChangeEmail(int id)
        {
            return View();
        }
        // POST: AuthController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeEmail(ChangeMailVM changeMail)
        {
            if (!ModelState.IsValid) return View(changeMail);
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user == null) return Content("NULL");
            var token = await _userManager.GenerateChangeEmailTokenAsync(user, changeMail.NewEmail);
            

            var changetPassResult = await _userManager.ChangeEmailAsync(user, changeMail.NewEmail,token);

          
            if (changetPassResult.Succeeded)
            {
                Email.SendEmail("tural.memmedzade025@gmail.com", user.Email, "Your Mail Is Changed", "tural2025", "Fiorello-Change Mail");
              
                return View(nameof(Profile));
            }
            else
            {
                foreach (var error in changetPassResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(nameof(ChangeEmail));
            }

        }

        // GET: AuthController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        private void IsAuthenticated()
        {
            if (User.Identity.IsAuthenticated)
            {
                throw new Exception("You already authenticated!");
               

            }
              
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
