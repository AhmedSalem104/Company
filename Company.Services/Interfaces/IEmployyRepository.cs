﻿using Company.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Services.Interfaces
{
    public interface IEmployyRepository : IGenericRepository<Employee>
    {
        Task<List<Employee>> SearchEmployeesByNameAsync(string Name);

    }
}
