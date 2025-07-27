using BL;
using BL.Contracts;
using BL.Mapping;
using BL.Services;
using DAL;
using DAL.Contracts;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Ui
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // Register services BEFORE building the application
            RegisterServicesHelper.RegisterServices(builder);       
            
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

            app.UseAuthorization();

            // Admin area route - MUST come first to have priority
            app.MapControllerRoute(
                name: "adminArea",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}",
                constraints: new { area = "admin" });

            // Generic area route for other areas
            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            // Default route for non-area controllers
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
