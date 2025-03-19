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
    public class EmployeeRepository : GenericRepository<Employee> ,IEmployyRepository
    {
        private readonly CompanyDbContext _Context;
        public EmployeeRepository(CompanyDbContext Context) : base(Context)
        {
            _Context = Context;
        }

        public async Task<List<Employee>> SearchEmployeesByNameAsync(string Name)
        {
            return await _Context.Employees.Include(E=>E.Department)
                .Where(E=>E.Name.ToLower().Contains(Name.ToLower()))
                .ToListAsync();
        }
    }
}
