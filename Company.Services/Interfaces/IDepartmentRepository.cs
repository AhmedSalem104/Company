using Company.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Services.Interfaces
{
    public interface IDepartmentRepository
    {
        IEnumerable<Department> GetAll();
        Department? Get(int Id);
        int Add(Department model);
        int Update(Department model);
        int Delete(Department model);
    }
}
