using Company.Data.Data.Contexts;
using Company.Data.Models;
using Company.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Services.Repositories
{
    public class DepartmentRepository : GenericRepository<Department> ,IDepartmentRepository
    {
        private readonly CompanyDbContext _Context;
        public DepartmentRepository(CompanyDbContext context) : base(context)
        {
            _Context = context;
        }


        public async Task<List<Department>> SearchDepartmentsByNameAsync(string Name)
        {
            return await _Context.Departments.Where(E => E.Name.ToLower().Contains(Name.ToLower()))
                .ToListAsync();
        }
      
   

     

    }
}
