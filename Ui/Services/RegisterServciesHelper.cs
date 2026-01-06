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
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;
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

            // Configure Serilog for logging with custom columns for user tracking
            ConfigureSerilog(builder);

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
            builder.Services.AddScoped<IShippmentStatus, ShippmentStatusServices>();
            builder.Services.AddScoped<ITrackingNumberCreator, TrackingNumberCreatorService>();
            builder.Services.AddScoped<IRateCalculator, RateCalculatorService>();

            builder.Services.AddSingleton<TokenService>();
            builder.Services.AddScoped<IRefreshTokens, RefreshTokenService>();
        }

        private static void ConfigureSerilog(WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Configure custom columns for user tracking and exception details
            var columnOptions = new ColumnOptions
            {
                AdditionalColumns = new Collection<SqlColumn>
                {
                    new SqlColumn { ColumnName = "UserId", DataType = SqlDbType.NVarChar, DataLength = 450, AllowNull = true },
                    new SqlColumn { ColumnName = "UserName", DataType = SqlDbType.NVarChar, DataLength = 256, AllowNull = true },
                    new SqlColumn { ColumnName = "RequestPath", DataType = SqlDbType.NVarChar, DataLength = 500, AllowNull = true },
                    new SqlColumn { ColumnName = "RequestMethod", DataType = SqlDbType.NVarChar, DataLength = 10, AllowNull = true },
                    new SqlColumn { ColumnName = "ClientIp", DataType = SqlDbType.NVarChar, DataLength = 50, AllowNull = true },
                    new SqlColumn { ColumnName = "CorrelationId", DataType = SqlDbType.NVarChar, DataLength = 50, AllowNull = true },
                    new SqlColumn { ColumnName = "MachineName", DataType = SqlDbType.NVarChar, DataLength = 100, AllowNull = true }
                }
            };

            // Remove columns we don't need
            columnOptions.Store.Remove(StandardColumn.Properties);
            columnOptions.Store.Add(StandardColumn.LogEvent);

            // Configure sink options
            var sinkOptions = new MSSqlServerSinkOptions
            {
                TableName = "ApplicationLogs",
                AutoCreateSqlTable = true,
                BatchPostingLimit = 50,
                BatchPeriod = TimeSpan.FromSeconds(5)
            };

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentName()
                .Enrich.WithProcessId()
                .Enrich.WithThreadId()
                .Enrich.WithClientIp()
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
                .WriteTo.MSSqlServer(
                    connectionString: connectionString,
                    sinkOptions: sinkOptions,
                    columnOptions: columnOptions)
                .CreateLogger();

            builder.Host.UseSerilog();
        }
    }
}
