using Company.Services.Interfaces;
using Company.Services.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Company.Web.Controllers
{
    public class DepartmentController : Controller
    {

        private readonly IDepartmentRepository _DepartmentRepository;


        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _DepartmentRepository = departmentRepository;
        }
        public IActionResult Index()
        {
            
            var departments = _DepartmentRepository.GetAll();

            return View(departments);
        }
    }
}
