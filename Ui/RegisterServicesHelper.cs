using BL;
using BL.Contracts;
using BL.Mapping;
using BL.Services;
using DAL;
using DAL.Contracts;
using DAL.Repositories;
using DAL.UserModels;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace Ui
{
    public class RegisterServicesHelper
    {
        public static void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/login";
                options.AccessDeniedPath = "/access-denied";
            });



            // Configure Serilog with modern syntax
            var logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.MSSqlServer(
                    connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
                    sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true }
                )
                .CreateLogger();

            // Set Serilog as the global logger
            Log.Logger = logger;

            // Add Entity Framework DbContext first
            builder.Services.AddDbContext<ShippingContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add Identity with ASP.NET Core Identity
            builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<ShippingContext>();
            // Configure Authentication cookie options
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/login";
                options.AccessDeniedPath = "/access-denied";
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
        }   
    }
}
