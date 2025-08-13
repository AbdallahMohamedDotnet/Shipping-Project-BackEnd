using BL.Mapping;
using BL.Services;
using DAL.Contracts;
using DAL.Repositories;
using DAL;
using Microsoft.EntityFrameworkCore.Internal;
using Serilog;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using DAL.UserModels;
using Microsoft.AspNetCore.Identity;
using System.Net.Http.Headers;
using BL.Contracts;
namespace Ui.Services
{
    public class RegisterServciesHelper
    {
        public static void RegisteredServices(WebApplicationBuilder builder)
        {
            // Add HttpContextAccessor
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddHttpClient("ApiClient", client =>
            {
                // Base URL will be configured in GenericApiClient constructor using appsettings.json
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // Set a reasonable timeout
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/access-denied";
            });

            builder.Services.AddDbContext<ShippingContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<ShippingContext>()
    .AddDefaultTokenProviders();

            builder.Services.AddAuthorization();

            // Configure Serilog for logging
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.MSSqlServer(
                    connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
                    tableName: "Log",
                    autoCreateSqlTable: true)
                .CreateLogger();
            builder.Host.UseSerilog();

            //builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

            //builder.Services.AddScoped<IGenericRepository<TbShippingType>, DAL.Repositories.GenericRepository<TbShippingType>>();
            builder.Services.AddScoped<GenericApiClient>();
            builder.Services.AddScoped(typeof(ITableRepository<>), typeof(TableRepository<>));
            builder.Services.AddScoped(typeof(IViewRepository<>), typeof(ViewRepository<>));
            builder.Services.AddScoped<IShippingType, ShippingTypeServices>();
            builder.Services.AddScoped<ICountry, CountryServices>();
            builder.Services.AddScoped<ICity, CityServices>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IRefreshTokens, RefreshTokenService>();
        }
    }
}
