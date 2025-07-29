using BL.Contracts;
using BL.Services;
using BL.Mapping;
using DAL;
using DAL.Contracts;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace WebApi
{
    public class Program
    {

        public static void Main(string[] args)
        {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<ShippingContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Configure Serilog for logging

            //builder.Host.UseSerilog();            Log.Logger = new LoggerConfiguration()
            //    .WriteTo.Console()
            //    .WriteTo.MSSqlServer(
            //        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
            //        tableName: "Log",
            //        autoCreateSqlTable: true)
            //    .CreateLogger();

        builder.Services.AddAutoMapper(typeof(Program));
        builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
        }
    }
}
