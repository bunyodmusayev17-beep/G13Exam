using Serilog;
using Exam.Api.Configurations;
using Exam.Api.Repositories;
using Exam.Api.Services;
using FluentValidation;

namespace Exam.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build())
                .CreateLogger();

            try
            {
                Log.Information("Application starting up");

                var builder = WebApplication.CreateBuilder(args);

                builder.Host.UseSerilog();
                builder.ConfigureDB();
                builder.Services.AddControllers();
                builder.Services.AddScoped<IFoodRepository, FoodRepository>();
                builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
                builder.Services.AddScoped<IFoodService, FoodService>();
                builder.Services.AddScoped<ICategoryService, CategoryService>();
                builder.Services.AddValidatorsFromAssemblyContaining<Program>();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                var app = builder.Build();
                builder.ConfigureDB();
                builder.ConfigureRepository();

                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseSerilogRequestLogging();
                app.UseHttpsRedirection();
                app.UseAuthorization();
                app.MapControllers();
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application failed to start");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
