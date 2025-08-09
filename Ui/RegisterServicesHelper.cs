using BL;
using BL.Contracts;
using BL.Mapping;
using BL.Services;
using DAL;
using DAL.Contracts;
using DAL.Repositories;
using DAL.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using Ui.Services;

namespace Ui
{
    public class RegisterServicesHelper
    {
        public static void RegisterServices(WebApplicationBuilder builder)
        {
            // Configure Serilog with modern syntax
            //var logger = new LoggerConfiguration()
            //    .WriteTo.Console()
            //    .WriteTo.MSSqlServer(
            //        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
            //        sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true }
            //    )
            //    .CreateLogger();

            // Set Serilog as the global logger
            //Log.Logger = logger;

            // Add Entity Framework DbContext first
            builder.Services.AddDbContext<ShippingContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add Identity with ASP.NET Core Identity (with roles support)
            // Identity automatically configures cookie authentication
            builder.Services.AddIdentity<ApplicationUser, Microsoft.AspNetCore.Identity.IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ShippingContext>()
            .AddDefaultTokenProviders();

            // Configure cookie options for Identity - redirect to Account/Login
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromHours(24);
                options.SlidingExpiration = true;
            });

            // Add Authorization
            builder.Services.AddAuthorization();

            // Add AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

            // Add Razor Pages and MVC services (since this is a Razor Pages project)
            builder.Services.AddRazorPages();
            builder.Services.AddControllersWithViews();

            // Register the DbContext as a service (though this is redundant with AddDbContext above)
            builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<ShippingContext>());

            // Register generic repository
            builder.Services.AddScoped(typeof(ITableRepository<>), typeof(TableRepository<>));
            builder.Services.AddScoped(typeof(IViewRepository<>), typeof(ViewRepository<>));
            // Register all business logic services
            builder.Services.AddScoped<ICarrierServices, CarrierServices>();
            builder.Services.AddScoped<ICity, CityServices>();
            builder.Services.AddScoped<ICountry, CountryServices>();
            builder.Services.AddScoped<IPaymentMethodServices, PaymentMethodServices>();
            builder.Services.AddScoped<ISetting, SettingServices>();
            builder.Services.AddScoped<IShippingType, ShippingTypeServices>();
            builder.Services.AddScoped<IShippment, ShippmentServices>();
            builder.Services.AddScoped<IShippmentStatus, ShippmentStatusServices>();
            builder.Services.AddScoped<ISubscriptionPackage, SubscriptionPackageServices>();
            builder.Services.AddScoped<IUserReceiver, UserReceiverServices>();
            builder.Services.AddScoped<IUserSebder, UserSebderServices>();
            builder.Services.AddScoped<IUserSubscription, UserSubscriptionServices>();
            builder.Services.AddScoped<IUserService, UserService>();
        }   
    }
}
