using Company.Data.Models;
using Company.Web.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Company.Web.Controllers
{
    //[AllowAnonymous]
    [Authorize]
    public class AccountController : Controller
    {
		private readonly UserManager<AppUser> _UserManager;
		private readonly SignInManager<AppUser> _SignInManager;
        public AccountController(UserManager<AppUser> UserManager,SignInManager<AppUser> SignInManager)
		{
			_UserManager = UserManager;
            _SignInManager = SignInManager;

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
    }
}
