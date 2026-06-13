using Exam.Api.Repositories;

namespace Exam.Api.Configurations;

public static class RepositoryConfiguration
{
    public static void ConfigureRepository(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<IFoodRepository, FoodRepository>();
    }
}