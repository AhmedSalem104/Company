using AutoMapper;
using Company.Data.Models;
using Company.Services.Interfaces;
using Company.Services.Repositories;
using Company.Web.DTO;
using Company.Web.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace Company.Web.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        #region Fields & Constructor
        private readonly IUnitOfWork _UnitOfWork;
        //private readonly IEmployyRepository _EmployeeRepository;
        //private readonly IDepartmentRepository _DepartmentRepository;
        private readonly IMapper _Mapper;

        public EmployeeController(IUnitOfWork UnitOfWork,
            IMapper Mapper)
        {
            _UnitOfWork = UnitOfWork;
            _Mapper = Mapper;
        }
        #endregion

        #region Index
        public async Task<IActionResult> Index(string? SearchName)
        {
            IEnumerable<Employee> Employees;
            if (string.IsNullOrEmpty(SearchName))
            {
                 Employees = await _UnitOfWork.EmployyRepository.GetAllAsync();

            }
            else
            {
                 Employees = await _UnitOfWork.EmployyRepository.SearchEmployeesByNameAsync(SearchName);

            }
            // Dictionary : 3 Property
            // 1.ViewData : Transfer Extra Information From Controller (Action) To View

            //ViewData["Message"] = "Hello Message Form ViewData...!";

            // 2.ViewBag : Transfer Extra Information From Controller (Action) To View  ===> Flexable
            // علشان الdatatype    بيتحدد عند فى مرحلة ال RunTime ولكن عيبها التحميل على ال CLR 

            //ViewBag.Message = "Hello  Message Form ViewBag";



            // 2.TempData : Transfer Extra Information From Controller (Action) To the same View or anoter view  Ex : From Create To Index  

            return View(Employees);
        }
        #endregion

        #region Create
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var departments = await _UnitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["Departments"] = departments;
            
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateEmployeeDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if ( string.IsNullOrWhiteSpace(model.Name))
            {
                ModelState.AddModelError("", "Code and Name fields are required.");
                return View(model);
            }

            try
            {


                if (model.Image is not null)
                {
                    model.ImageName = DocumentSettings.UploadFile(model.Image, folderName: "images");
                }

                var Employee = _Mapper.Map<Employee>(model);
                //var Employee = new Employee()
                //{

                //    Name = model.Name,
                //    Address = model.Address,
                //    Age = model.Age,                    
                //    CreateAt = model.CreateAt,
                //    HiringDate = model.HiringDate,
                //    Email  = model.Email,
                //    IsActive = model.IsActive,  
                //    IsDelete = model.IsDelete,
                //    Phone  = model.Phone,
                //    Salary =  model.Salary,
                //    DepartmentId =  model.DepartmentId,
                //};

                await _UnitOfWork.EmployyRepository.AddAsync(Employee);

                 await _UnitOfWork.CompleteAsync();

                if (Employee.Id == 0)
                {
                    ModelState.AddModelError("", "An error occurred while creating the Employee.");
                    return View(model);
                }
                TempData["Message"] = "Employee is Created"; 
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
                return BadRequest("Invalid Employee ID.");
            }
            var Employee = await _UnitOfWork.EmployyRepository.GetAsync(id);
            if (Employee == null)
            {
                return NotFound($"Employee with ID {id} not found.");
            }
            return View(Employee);
        }
        #endregion

        #region Update
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid Employee ID.");
            }
            var Employee = await _UnitOfWork.EmployyRepository.GetAsync(id);
            if (Employee == null)
            {
                return NotFound($"Employee with ID {id} not found.");
            }
            var departments = await _UnitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["Departments"] = departments;

            var Emp = _Mapper.Map<CreateEmployeeDTO>(Employee);


            return View(Emp);
        }
        // EX 01

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromRoute] int id, CreateEmployeeDTO model)
        {
            if (ModelState.IsValid)
            {


                if (model.ImageName is not null)
                {
                    DocumentSettings.DeleteFile(model.ImageName, folderName: "images");
                }

                if (model.Image is not null)
                {
                    model.ImageName = DocumentSettings.UploadFile(model.Image, folderName: "images");
                }

                var employee = _Mapper.Map<Employee>(model);
                employee.Id = id;
                _UnitOfWork.EmployyRepository.Update(employee);
                var count = await _UnitOfWork.CompleteAsync();

                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
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

            var employee = await _UnitOfWork.EmployyRepository.GetAsync(id);
            if (employee == null)
                return NotFound(new { message = "Department not found" });

            _UnitOfWork.EmployyRepository.Delete(employee);
            await _UnitOfWork.CompleteAsync();

            return Ok(new { message = "Employee deleted successfully" });
        }
        #endregion
    }
}
