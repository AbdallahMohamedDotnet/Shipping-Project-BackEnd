using BL.Contract;
using BL.Contract.Shipment;
using BL.Contracts;
using BL.Mapping;
using BL.Services;
using BL.Services.Shipment;
using DAL;
using DAL.Contracts;
using DAL.Repositories;
using DAL.UserModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Serilog;
using System.Net.Http.Headers;
using WebApi.Services;
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

            // Register GenericApiClient service
            builder.Services.AddScoped<GenericApiClient>();

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

            // Register custom IMapper wrapper
            builder.Services.AddScoped<BL.Mapping.IMapper, BL.Mapping.AutoMapper>();

            //builder.Services.AddScoped<IGenericRepository<TbShippingType>, DAL.Repositories.GenericRepository<TbShippingType>>();
            builder.Services.AddScoped(typeof(ITableRepository<>), typeof(TableRepository<>));
            builder.Services.AddScoped(typeof(IViewRepository<>), typeof(ViewRepository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IShippingType, ShippingTypeServices>();
            builder.Services.AddScoped<ICountry, CountryServices>();
            builder.Services.AddScoped<ICity, CityServices>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IPaymentMethodServices, PaymentMethodServices>();
            //builder.Services.AddScoped<IPackgingTypes, ShipingPackgingService>();
            builder.Services.AddScoped<IUserSender, UserSenderServices>();
            builder.Services.AddScoped<IUserReceiver, UserReceiverServices>();

            builder.Services.AddScoped<IShipment, ShipmentService>();
            builder.Services.AddScoped<ITrackingNumberCreator, TrackingNumberCreatorService>();
            builder.Services.AddScoped<IRateCalculator, RateCalculatorService>();

            builder.Services.AddSingleton<TokenService>();
            builder.Services.AddScoped<IRefreshTokens, RefreshTokenService>();

        }
    }
}
