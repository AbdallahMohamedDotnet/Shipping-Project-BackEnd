using BL.Contracts;
using BL.Mapping;
using BL.Services;
using BL;
using DAL;
using DAL.Contracts;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace Ui
{
    public class RegisterServicesHelper
    {
        public static void RegisterServices(WebApplicationBuilder builder)
        {
            // Configure Serilog with modern syntax
            var logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.MSSqlServer(
                    connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
                    sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true }
                )
                .CreateLogger();

            // Add logging services
            builder.Services.AddLogging();

            // Add AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

            // Add services to the container
            builder.Services.AddControllersWithViews();
            
            // Add Entity Framework DbContext
            builder.Services.AddDbContext<ShippingContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register the DbContext as a service
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
