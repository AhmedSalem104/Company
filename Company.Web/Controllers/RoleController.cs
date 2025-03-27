using Company.Data.Models;
using Company.Web.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Company.Web.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole>  _RoleManager;



        public RoleController(RoleManager<IdentityRole> RoleManager)
        {
            _RoleManager = RoleManager;
        }

        #region Index
        public async Task<IActionResult> Index(string? SearchName)
        {
            IEnumerable<RoleToReturnDTO> roles;
            if (string.IsNullOrEmpty(SearchName))
            {
                roles = _RoleManager.Roles.Select(R => new RoleToReturnDTO()
                {
                    Id = R.Id,
                    Name = R.Name
                
                });

            }
            else
            {
                roles = _RoleManager.Roles.Select(R => new RoleToReturnDTO()
                {
                    Id = R.Id,
                    Name = R.Name

                }).Where(U => U.Name.ToLower().Contains(SearchName.ToLower()));
            }

            return View(roles);
        }
        #endregion

        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleToReturnDTO model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {        
                var role = await _RoleManager.FindByNameAsync(model.Name);
                if (role is  null)
                {
                    role = new IdentityRole()
                    {
                        Name = model.Name
                    };
                    var result = await _RoleManager.CreateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }

                ModelState.AddModelError("", "Invalid Operation");

            }

            return View(model);
        }
        #endregion

        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            if (id is null)
            {
                return BadRequest("Invalid Employee ID.");
            }
            var role = await _RoleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound($"User with ID {id} not found.");
            }
            var dto = new RoleToReturnDTO()
            {
                Id = role.Id,
                Name = role.Name,
           
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


  

        [HttpPost]
        public async Task<IActionResult> Update([FromRoute] string id, RoleToReturnDTO model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest(error: "Invalid Operations !");

                var role = await _RoleManager.FindByIdAsync(id);
                if (role is null) return BadRequest(error: "Invalid Operations !");
                var roleResult = await _RoleManager.FindByNameAsync(model.Name);
                if(roleResult is  null)
                {
                   
                    role.Name = model.Name;

                    var result = await _RoleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }

                ModelState.AddModelError("", "Invalid Operation");

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
                var role = await _RoleManager.FindByIdAsync(id);
                if (role is null) return BadRequest(error: "Invalid Operations !");
                var result = await _RoleManager.DeleteAsync(role);
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
