using AutoMapper;
using Company.Data.Models;
using Company.Services.Interfaces;
using Company.Services.Repositories;
using Company.Web.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.Web.Controllers
{
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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
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




            //var depart = new CreateDepartmentDTO()
            //{

            //    Code = department.Code,
            //    Name = department.Name,
            //    CreateAt = department.CreateAt
            //};

            return View(depart);
        }


        // EX 01

        [HttpPost]
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

        // EX 02

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public IActionResult Update([FromRoute] int id, UpdateDepartmentDTO model)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        var department = new Department()
        //        {
        //            Id = id,
        //            Code = model.Code,
        //            Name = model.Name,
        //            CreateAt = model.CreateAt
        //        };


              
        //            var count = _departmentRepository.Update(department);
        //            if (count > 0)
        //            {
        //                return RedirectToAction(nameof(Index));
        //            }
                
               

        //    }
        //    return View(model);
        //}


        #endregion

        #region Delete


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
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

            var depart = _Mapper.Map<Department>(department);

            //var depart = new DeleteDepartmentDTO()
            //{
            //    Id = department.Id,
            //    Code = department.Code,
            //    Name = department.Name,
            //    CreateAt = department.CreateAt
            //};

            return View(depart);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(DeleteDepartmentDTO model)
        {
            if (model.Id <= 0)
            {
                return BadRequest("Invalid department ID.");
            }
            var existingDepartment = await _UnitOfWork.DepartmentRepository.GetAsync(model.Id);
            if (existingDepartment == null)
            {
                return NotFound($"Department with ID {model.Id} not found.");
            }

            try
            {
                _UnitOfWork.DepartmentRepository.Delete(existingDepartment);
               await _UnitOfWork.CompleteAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while deleting the department. Please try again.");
                return View(model);
            }
        }
        #endregion
    }

}
