using AutoMapper;
using Company.Data.Models;
using Company.Services.Interfaces;
using Company.Services.Repositories;
using Company.Web.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.Web.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        #region Fields & Constructor
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _Mapper;


        public DepartmentController(IUnitOfWork UnitOfWork, IMapper Mapper)
        {
            _UnitOfWork = UnitOfWork;
            _Mapper = Mapper;
        }
        #endregion

        #region Index
        public async Task<IActionResult> Index()
        {
            var departments = await _UnitOfWork.DepartmentRepository.GetAllAsync();
            return View(departments);
        }
        #endregion

        #region Create
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateDepartmentDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.Code) || string.IsNullOrWhiteSpace(model.Name))
            {
                ModelState.AddModelError("", "Code and Name fields are required.");
                return View(model);
            }

            try
            {
                var department = _Mapper.Map<Department>(model);


                await _UnitOfWork.DepartmentRepository.AddAsync(department);
                var count = await _UnitOfWork.CompleteAsync();
                if (department.Id == 0)
                {
                    ModelState.AddModelError("", "An error occurred while creating the department.");
                    return View(model);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while processing your request. Please try again.");
                return View(model);
            }
        }
        #endregion

        #region Details

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid department ID.");
            }
            var department = await _UnitOfWork.DepartmentRepository.GetAsync(id);
            if (department == null)
            {
                return NotFound($"Department with ID {id} not found.");
            }
            return View(department);
        }
        #endregion

        #region Update
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid department ID.");
            }
            var department = await _UnitOfWork.DepartmentRepository.GetAsync(id);
            if (department == null)
            {
                return NotFound($"Department with ID {id} not found.");
            }
            var depart = _Mapper.Map<CreateDepartmentDTO>(department);
            return View(depart);
        }



        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromRoute] int id, Department model)
        {
            if (ModelState.IsValid)
            {
                if (id == model.Id)
                {
                    _UnitOfWork.DepartmentRepository.Update(model);
                    var count = await _UnitOfWork.CompleteAsync();
                    if (count > 0)
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return BadRequest();
                }

            }
            return View(model);
        }

        #endregion

        #region Delete

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid ID" });

            var department = await _UnitOfWork.DepartmentRepository.GetAsync(id);
            if (department == null)
                return NotFound(new { message = "Department not found" });

            _UnitOfWork.DepartmentRepository.Delete(department);
            await _UnitOfWork.CompleteAsync();

            return Ok(new { message = "Department deleted successfully" });
        }

        #endregion

        public async Task<IActionResult> Search(string? SearchName)
        {
            var Departs = await _UnitOfWork.DepartmentRepository.SearchDepartmentsByNameAsync(SearchName);
            return PartialView("_DepartmentTablePartialView", Departs);
        }

    }

}
