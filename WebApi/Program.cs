using BL.Contracts;
using BL.Mapping;
using BL.Services;
using DAL;
using DAL.Contracts;
using DAL.Repositories;
using DAL.UserModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Serilog;
using WebApi.Services;

namespace WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // Configure CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("https://localhost:7279", "http://localhost:5022") // UI project URLs
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials(); // Required for cookies (refresh token)
                });
            });

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configure Entity Framework
            builder.Services.AddDbContext<ShippingContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Configure Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ShippingContext>()
                .AddDefaultTokenProviders();

            // Add AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

            // Register repositories and services
            builder.Services.AddScoped(typeof(ITableRepository<>), typeof(TableRepository<>));
            builder.Services.AddScoped<IShippingType, ShippingTypeServices>();
            builder.Services.AddScoped<ICity, CityServices>();
            builder.Services.AddScoped<IRefreshTokens, RefreshTokenService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<TokenService>();
            builder.Services.AddScoped(typeof(IViewRepository<>), typeof(ViewRepository<>));

            // Add HTTP Context Accessor
            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowFrontend");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
