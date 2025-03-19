using Company.Data.Data.Contexts;
using Company.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Services.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CompanyDbContext _Context;

        public IDepartmentRepository DepartmentRepository { get; }
        public IEmployyRepository EmployyRepository { get; }

        public UnitOfWork(CompanyDbContext Context)
        {
            _Context = Context;
            DepartmentRepository = new DepartmentRepository(_Context);
            EmployyRepository = new EmployeeRepository(_Context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _Context.SaveChangesAsync();
        }



        public async ValueTask DisposeAsync() // هذه الدالة ستنفذ تلقائيا بعد الانتهاء من الانتهاء من التعامل مع ال Database
        {
            await _Context.DisposeAsync();
        }
    }
}
