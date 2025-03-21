﻿using Company.Data.Data.Contexts;
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
        public DepartmentRepository(CompanyDbContext context) : base(context)
        {

        }
    }
}
