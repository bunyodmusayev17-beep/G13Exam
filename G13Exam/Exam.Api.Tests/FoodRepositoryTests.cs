using Exam.Api.Data;
using Exam.Api.Entities;
using Exam.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Exam.Api.Tests;

public class FoodRepositoryTests
{
    [Fact]
    public async Task AddAsync_PersistsFoodToDatabase()
    {
        await using var context = CreateContext();
        SeedCategory(context);
        var repository = new FoodRepository(context);
        var food = new Food
        {
            Name = "Burger",
            Description = "Classic burger",
            Price = 12.50m,
            IsAvailable = true,
            CategoryId = 1
        };

        await repository.AddAsync(food);
        await repository.SaveChangesAsync();

        var storedFood = await context.Foods.SingleAsync();

        Assert.Equal("Burger", storedFood.Name);
        Assert.Equal(12.50m, storedFood.Price);
        Assert.True(storedFood.IsAvailable);
        Assert.Equal(1, storedFood.CategoryId);
    }

    [Fact]
    public async Task GetAllQuery_ReturnsFoodsFromDatabase()
    {
        await using var context = CreateContext();
        SeedCategory(context);
        context.Foods.AddRange(
            new Food
            {
                Name = "Burger",
                Description = "Classic burger",
                Price = 12.50m,
                IsAvailable = true,
                CategoryId = 1
            },
            new Food
            {
                Name = "Salad",
                Description = "Fresh salad",
                Price = 8.00m,
                IsAvailable = false,
                CategoryId = 1
            });
        await context.SaveChangesAsync();

        var repository = new FoodRepository(context);
        var foods = await repository.GetAllQuery().AsNoTracking().ToListAsync();

        Assert.Equal(2, foods.Count);
        Assert.Contains(foods, food => food.Name == "Burger");
        Assert.Contains(foods, food => food.Name == "Salad");
    }

    [Fact]
    public async Task Delete_RemovesFoodFromDatabase()
    {
        await using var context = CreateContext();
        SeedCategory(context);
        var food = new Food
        {
            Name = "Burger",
            Description = "Classic burger",
            Price = 12.50m,
            IsAvailable = true,
            CategoryId = 1
        };
        context.Foods.Add(food);
        await context.SaveChangesAsync();

        var repository = new FoodRepository(context);
        repository.Delete(food);
        await repository.SaveChangesAsync();

        Assert.Empty(await context.Foods.ToListAsync());
    }

    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    private static void SeedCategory(AppDbContext context)
    {
        context.Categories.Add(new Category { Id = 1, Name = "Main" });
        context.SaveChanges();
    }
}
