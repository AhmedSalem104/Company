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

        private readonly IDepartmentRepository _departmentRepository;


        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        public IActionResult Index()
        {          
            var departments = _departmentRepository.GetAll();
            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {                     
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateDepartmentDTO model)
        {
            if (ModelState.IsValid)
            {
                var department = new Department() { 
                
                Code = model.Code,
                Name = model.Name,
                CreateAt = model.CreateAt
                };

                _departmentRepository.Add(department);
                
                return RedirectToAction("Index");
            }


            return View(model);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var department = _departmentRepository.Get(id);
            var depart = new CreateDepartmentDTO()
            {
                Code = department.Code,
                Name = department.Name,
                CreateAt = department.CreateAt
            };

            if (department == null)
            {
                return NotFound();
            }
            return View(depart);
        }

        [HttpGet]
        public IActionResult Update(int id)
        {

            var department = _departmentRepository.Get(id);
            var depart = new UpdateDepartmentDTO()
            {
                Id = department.Id,
                Code = department.Code,
                Name = department.Name,
                CreateAt = department.CreateAt
            };

            if (department == null)
            {
                return NotFound();
            }
            return View(depart);
        }

        [HttpPost]
        public IActionResult Update(UpdateDepartmentDTO model)
        {
            if (ModelState.IsValid)
            {
                var department = new Department()
                {
                    Id = model.Id,
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt
                };

                _departmentRepository.Update(department);

                return RedirectToAction("Index");
            }


            return View(model);
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {

            var department = _departmentRepository.Get(id);
            var depart = new DeleteDepartmentDTO()
            {
                Id = department.Id,
                Code = department.Code,
                Name = department.Name,
                CreateAt = department.CreateAt
            };

            if (department == null)
            {
                return NotFound();
            }
            return View(depart);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(DeleteDepartmentDTO model)
        {
            if (model.Id != 0)
            {
                var department = new Department()
                {
                    Id = model.Id,
                   
                };

                _departmentRepository.Delete(department);

                
            }
            return RedirectToAction("Index");


        }



    }
}
