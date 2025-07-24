using BL.Contracts;
using BL;
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
            var logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.MSSqlServer(
            connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
            tableName: "Logs",
            autoCreateSqlTable: true
                )
                 .CreateLogger();

            // Add logging services
            builder.Services.AddLogging();
            
            // Add AutoMapper
            builder.Services.AddAutoMapper(typeof(Program));
            
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ShippingContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<DbContext>(provider => provider.GetService<ShippingContext>());

            builder.Services.AddScoped(typeof(ITableRepository<>), typeof(TableRepository<>));
            builder.Services.AddScoped<IShippingType, ShippingTypeServices>();
            builder.Services.AddScoped<ICity, CityServices>(); // Assuming you have a CityServices implementation

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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
