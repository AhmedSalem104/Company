using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Company.Data.Data.Contexts
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CompanyDbContext>
    {
        public CompanyDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CompanyDbContext>();

            // تحديد سلسلة الاتصال الخاصة بقاعدة البيانات في وقت التصميم
            optionsBuilder.UseSqlServer("Server=DESKTOP-3MG63U6;Database=CompanyFirstProject;Trusted_Connection=True;TrustServerCertificate=True");

            return new CompanyDbContext(optionsBuilder.Options);
        }
    }
}
