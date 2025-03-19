using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Services.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IDepartmentRepository DepartmentRepository { get; }
        IEmployyRepository EmployyRepository { get; }
        int Complete();
    }
}
