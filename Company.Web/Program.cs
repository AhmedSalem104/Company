using AutoMapper;
using Company.Data.Data.Contexts;
using Company.Data.Models;
using Company.Services.Interfaces;
using Company.Services.Repositories;
using Company.Web.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Company.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            //builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();   // Allow DI For DepartmentRepository
            //builder.Services.AddScoped<IEmployyRepository, EmployeeRepository>();        // Allow DI For EmployeeRepository
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();        // Allow DI For UnitOfWork


            //builder.Services.AddAutoMapper(typeof(EmployeeProfile));                     // Allow DI For EmployeeRepositor
            builder.Services.AddAutoMapper(M=>M.AddProfile(new EmployeeProfile()));


            // builder.Services.AddScoped<CompanyDbContext>(); // Allow DI For CompanyDbContext
            //builder.Services.AddDbContext<CompanyDbContext>(); // Allow DI For CompanyDbContext

            //builder.Services.AddScoped();     // Create Object life Time Per Request
            //builder.Services.AddTransient();  // Create Object life Time Per Operation
            //builder.Services.AddSingleton();  // Create Object life Time Per Application


            builder.Services.AddDbContext<CompanyDbContext>(option => { option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); });
            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<CompanyDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/SignIn";
            });


            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Limits.MaxRequestLineSize = 8192; 
                options.Limits.MaxRequestHeadersTotalSize = 16384; 
            });



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
