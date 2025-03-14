﻿using Company.Data.Models;
using Company.Services.Interfaces;
using Company.Web.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Company.Web.Controllers
{
    public class EmployeeController : Controller
    {
        #region Fields & Constructor
        private readonly IEmployyRepository _EmployeeRepository;

        public EmployeeController(IEmployyRepository EmployeeRepository)
        {
            _EmployeeRepository = EmployeeRepository;
        }
        #endregion

        #region Index
        public IActionResult Index()
        {
            var Employees = _EmployeeRepository.GetAll();
            return View(Employees);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateEmployeeDTO model)
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
                var Employee = new Employee()
                {
                    
                    Name = model.Name,
                    Address = model.Address,
                    Age = model.Age,                    
                    CreateAt = model.CreateAt,
                    HiringDate = model.HiringDate,
                    Email  = model.Email,
                    IsActive = model.IsActive,  
                    IsDelete = model.IsDelete,
                    Phone  = model.Phone,
                    Salary =  model.Salary,
                };

                _EmployeeRepository.Add(Employee);

                if (Employee.Id == 0)
                {
                    ModelState.AddModelError("", "An error occurred while creating the Employee.");
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
        public IActionResult Details(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid Employee ID.");
            }
            var Employee = _EmployeeRepository.Get(id);
            if (Employee == null)
            {
                return NotFound($"Employee with ID {id} not found.");
            }
            return View(Employee);
        }
        #endregion

        #region Update
        [HttpGet]
        public IActionResult Update(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid Employee ID.");
            }
            var Employee = _EmployeeRepository.Get(id);
            if (Employee == null)
            {
                return NotFound($"Employee with ID {id} not found.");
            }
            var Emp = new Employee()
            {

                Name = Employee.Name,
                Address = Employee.Address,
                Age = Employee.Age,
                CreateAt = Employee.CreateAt,
                HiringDate = Employee.HiringDate,
                Email = Employee.Email,
                IsActive = Employee.IsActive,
                IsDelete = Employee.IsDelete,
                Phone = Employee.Phone,
                Salary = Employee.Salary,
            };

            return View(Emp);
        }


        // EX 01

        [HttpPost]
        public IActionResult Update([FromRoute] int id, Employee model)
        {
            if (ModelState.IsValid)
            {
                if (id == model.Id)
                {
                    var count = _EmployeeRepository.Update(model);
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
        //public IActionResult Update([FromRoute] int id, UpdateEmployeeDTO model)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        var Employee = new Employee()
        //        {
        //            Id = id,
        //            Code = model.Code,
        //            Name = model.Name,
        //            CreateAt = model.CreateAt
        //        };



        //            var count = _EmployeeRepository.Update(Employee);
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
        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid Employee ID.");
            }

            var Employee = _EmployeeRepository.Get(id);

            if (Employee == null)
            {
                return NotFound($"Employee with ID {id} not found.");
            }

            var Emp = new Employee()
            {
                Id = Employee.Id,
               
                Name = Employee.Name,
                CreateAt = Employee.CreateAt
            };

            return View(Emp);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(Employee model)
        {
            if (model.Id <= 0)
            {
                return BadRequest("Invalid Employee ID.");
            }
            var existingEmployee = _EmployeeRepository.Get(model.Id);
            if (existingEmployee == null)
            {
                return NotFound($"Employee with ID {model.Id} not found.");
            }

            try
            {
                _EmployeeRepository.Delete(existingEmployee);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while deleting the Employee. Please try again.");
                return View(model);
            }
        }
        #endregion
    }
}
