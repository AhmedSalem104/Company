using Company.Data.Models;
using Company.Web.DTO;
using Company.Web.Helper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Drawing;

namespace Company.Web.Controllers
{
   public class AccountController : Controller
    {
		private readonly UserManager<AppUser> _UserManager;
		private readonly SignInManager<AppUser> _SignInManager;
		private readonly IEmailServices _MailKitSendEmailSetting;

        public AccountController(UserManager<AppUser> UserManager,SignInManager<AppUser> SignInManager , IEmailServices MailKitSendEmailSetting)
		{
			_UserManager = UserManager;
            _SignInManager = SignInManager;
            _MailKitSendEmailSetting = MailKitSendEmailSetting;
        }

        #region SignUp
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
		[HttpPost]
		public async Task<IActionResult> SignUp(SignUpDTO model)
		{
            if (ModelState.IsValid) // Server Side Validation
            {

                var user = await _UserManager.FindByNameAsync(model.UserName);
                if (user is null)
                {
                    user = await _UserManager.FindByEmailAsync(model.Email);
                    if (user is null)
                    {
                         user = new AppUser()
                        {
                            UserName = model.UserName,
                            FName = model.FName,
                            LName = model.LName,
                            Email = model.Email,
                            IsAgree = model.IsAgree,
                        };

                        var result = await _UserManager.CreateAsync(user, model.Password);
                        if (result.Succeeded) { return RedirectToAction(nameof(SignIn)); }
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);

                        }
                    }
                }
                ModelState.AddModelError("", "Invalid SignUp");
            }

            return View(model);
		}
        #endregion

        #region SignIn
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        } 
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = await _UserManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var flag = await _UserManager.CheckPasswordAsync(user, model.Password);
                    if (flag)
                    {
                        // Sign In
                        var result = await _SignInManager.PasswordSignInAsync(user, model.Password,model.RememberMe, false);
                        if (result.Succeeded)
                        {
                            return RedirectToAction(nameof(HomeController.Index), controllerName: "Home");

                        }
                    }

                }
            }

            ModelState.AddModelError("", "Invalid Login !");

            return View(model);
        }

        #endregion

        #region SignOut

        [HttpGet]
        public new async Task<IActionResult> SignOut()
        {
            await _SignInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));

        }

        #endregion

        #region Forget Password

        [HttpGet]
        public new async Task<IActionResult> ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public new async Task<IActionResult> ForgetPassword(ForgetPasswordDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = await _UserManager.FindByEmailAsync(model.Email);
                if(user is not null)
                {
                    // Generate Token 
                    var token = await _UserManager.GeneratePasswordResetTokenAsync(user);

                    // Create URL
                    var URL =  Url.Action("ResetPassword", "Account", new { email = model.Email, token }, Request.Scheme);

                    // Reset Password --> Send Email
                    // Create Email 
                    var email = new Helper.Email()
                    {
                        To = model.Email,
                        Subject = "Reset Password",
                        Body = URL
                    };


                    // 1.Way ( Use MailKit Service )

                    var flage = _MailKitSendEmailSetting.SendEmail(email);
                    if (flage)
                    {                        
                        return RedirectToAction(nameof(CheckYourInbox));
                    }

                    // 2.Way ( Use OutLook Service ) 

                    //OutLookSendEmailSetting outLookSendEmailSetting = new OutLookSendEmailSetting();
                    //    var flage = outLookSendEmailSetting.SendEmail(email);
                    //    var flage = outLookSendEmailSetting.SendEmail(email);
                    //if (flage)
                    //{
                    //    // Success      Check Your Email
                    //     return RedirectToAction(nameof(CheckYourInbox));
                    //}
                }
            }
            ModelState.AddModelError("", "Invalid Reste Password Operation");
            return View("ForgetPassword",model);
        }

        #endregion

        #region CheckYourInbox

        [HttpGet]
        public IActionResult CheckYourInbox()
        {
            return View();
        }

        #endregion

        #region ResetPassword

        [HttpGet]
        public IActionResult ResetPassword( string email,string token)
        {
            TempData["Email"] = email;
            TempData["token"] = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
        {
            if (ModelState.IsValid) 
            {
                var email = TempData["Email"] as string;
                var token = TempData["token"] as string;

                if(email is null  || token is null)
                {
                    return BadRequest(" Invalid Operation");
                }
                var user = await _UserManager.FindByEmailAsync(email);
                if (user is not null) 
                {
                    var result = await _UserManager.ResetPasswordAsync(user, token, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(SignIn));
                    }
                }
                ModelState.AddModelError("", "Invalid Reset Password Operation");
            }
            return View();
        }



        #endregion

        #region AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }
        #endregion

        #region AuthWithGoogle
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties()
            {
                RedirectUri = Url.Action("GoogleResponse")
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!result.Succeeded)
            {
                return BadRequest("فشل تسجيل الدخول باستخدام جوجل");
            }
            var claims = result.Principal.Identities
                .FirstOrDefault()?.Claims.Select(claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value
                });

            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region AuthWithFacebook
        public IActionResult FacebookLogin()
        {
            var properties = new AuthenticationProperties()
            {
                RedirectUri = Url.Action("FacebookResponse")
            };
            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> FacebookResponse()
        {
            var result = await HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);
            if (!result.Succeeded)
            {
                return BadRequest("فشل تسجيل الدخول باستخدام جوجل");
            }
            var claims = result.Principal.Identities
                .FirstOrDefault()?.Claims.Select(claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value
                });

            return RedirectToAction("Index", "Home");
        }
        #endregion


    }
}
