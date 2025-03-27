using Company.Data.Models;
using Company.Web.DTO;
using Company.Web.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _UserManager;



       public UserController(UserManager<AppUser> UserManager)
        {
            _UserManager = UserManager;
        }

        #region Index
        public async Task<IActionResult> Index(string? SearchName)
        {
            IEnumerable<UserToReturnDTO> Users;
            if (string.IsNullOrEmpty(SearchName))
            {
                Users =  _UserManager.Users.Select( U => new UserToReturnDTO()
                {
                    Id = U.Id,
                    Email = U.Email,
                    FName = U.FName,
                    LName = U.LName,
                    UserName = U.UserName,
                    Roles = _UserManager.GetRolesAsync(U).Result 
                });

            }
            else
            {
                Users = _UserManager.Users.Select(U => new UserToReturnDTO()
                {
                    Id = U.Id,
                    Email = U.Email,
                    FName = U.FName,
                    LName = U.LName,
                    UserName = U.UserName,
                    Roles = _UserManager.GetRolesAsync(U).Result
                }).Where(U=>U.UserName.ToLower().Contains(SearchName.ToLower()));

            }

            return View(Users);
        }
        #endregion

        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
          

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserToReturnDTO model)
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
                        if (result.Succeeded) { return RedirectToAction(nameof(Index)); }
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

        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(string? id ,string viewName= "Details")
        {
            if (id is null)
            {
                return BadRequest("Invalid Employee ID.");
            }
            var User = await _UserManager.FindByIdAsync(id);
            if (User == null)
            {
                return NotFound($"User with ID {id} not found.");
            }
            var dto = new UserToReturnDTO()
            {
                Id = User.Id,
                Email = User.Email,
                FName = User.FName,
                LName = User.LName,
                UserName = User.UserName,
                Roles = _UserManager.GetRolesAsync(User).Result

            };
            return View(viewName, dto);
        }
        #endregion

        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(string? id)
        {
           
            return await Details(id, "Update");
        }


        // EX 01

        [HttpPost]
        public async Task<IActionResult> Update([FromRoute] string id, UserToReturnDTO model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest(error: "Invalid Operations !");

                var user = await _UserManager.FindByIdAsync(id);
                if (user is null) return BadRequest(error: "Invalid Operations !");

                user.UserName = model.UserName;
                user.FName = model.FName;
                user.LName = model.LName;
                user.Email = model.Email;             
                var result = await _UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }


            }
            return View(model);
        }




        #endregion

        #region Delete


        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                var user = await _UserManager.FindByIdAsync(id);
                if (user is null) return BadRequest(error: "Invalid Operations !");
                var result = await _UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return Ok(new { message = "User deleted successfully" });
        }
        #endregion
    }
}
