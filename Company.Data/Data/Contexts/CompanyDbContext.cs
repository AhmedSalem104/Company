using Company.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace Company.Data.Data.Contexts
{
    public class CompanyDbContext : DbContext
    {

        public CompanyDbContext() : base() 
        {
        
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server = DESKTOP-M87APL7\\SQLEXPRESS ; Database = CompanyFirstProject; Trusted_Connection = True ; TrustServerCertificate = True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Department> Departments { get; set; }

    }
}
