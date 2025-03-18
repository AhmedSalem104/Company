using Company.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Data.Data.Configrations
{
    public class EmployeeConfigrations :IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
         

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                   .UseIdentityColumn(1, 1);

            builder.Property(e => e.Name)
                   .HasMaxLength(100)
                   .IsRequired(false);

            builder.Property(e => e.Age)
                   .IsRequired(false);

            builder.Property(e => e.Email)
                   .HasMaxLength(150)
                   .IsRequired(false);

            builder.Property(e => e.Address)
                   .HasMaxLength(250)
                   .IsRequired(false);

            builder.Property(e => e.Phone)
                   .HasMaxLength(15)
                   .IsRequired(false);

            builder.Property(e => e.Salary)
                   .HasColumnType("decimal(18,2)");

            builder.Property(e => e.IsActive)
                   .HasDefaultValue(true);

            builder.Property(e => e.IsDelete)
                   .HasDefaultValue(false);

            builder.Property(e => e.HiringDate)
                   .HasColumnType("datetime2")
                   .IsRequired();

            builder.Property(e => e.CreateAt)
                   .HasColumnType("datetime2")
                   .HasDefaultValueSql("GETDATE()");


            // Relation
            builder.HasOne(E => E.Department)
                .WithMany(D => D.Employees)
                .HasForeignKey(E => E.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull);

            //builder.ToTable("Employees");

        }

    }
}
