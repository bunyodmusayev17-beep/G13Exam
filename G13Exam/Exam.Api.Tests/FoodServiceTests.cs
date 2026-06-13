using Exam.Api.Data;
using Exam.Api.Dtos;
using Exam.Api.Entities;
using Exam.Api.Repositories;
using Exam.Api.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Exam.Api.Tests;

public class FoodServiceTests
{
    [Fact]
    public async Task GetAllAsync_ReturnsMappedFoods()
    {
        await using var context = CreateContext();
        await SeedCategoryAsync(context);
        context.Foods.AddRange(
            new Food
            {
                Id = 1,
                Name = "Burger",
                Description = "Classic burger",
                Price = 12.50m,
                IsAvailable = true,
                CategoryId = 1
            },
            new Food
            {
                Id = 2,
                Name = "Salad",
                Description = "Fresh salad",
                Price = 8.00m,
                IsAvailable = false,
                CategoryId = 1
            });
        await context.SaveChangesAsync();

        var service = CreateService(context);

        var result = await service.GetAllAsync();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, food => food.FoodId == 1 && food.Name == "Burger");
        Assert.Contains(result, food => food.FoodId == 2 && food.Name == "Salad");
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsMappedFood()
    {
        await using var context = CreateContext();
        await SeedCategoryAsync(context);
        context.Foods.Add(new Food
        {
            Id = 1,
            Name = "Burger",
            Description = "Classic burger",
            Price = 12.50m,
            IsAvailable = true,
            CategoryId = 1
        });
        await context.SaveChangesAsync();

        var service = CreateService(context);

        var result = await service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result!.FoodId);
        Assert.Equal("Burger", result.Name);
        Assert.Equal("Classic burger", result.Description);
        Assert.Equal(12.50m, result.Price);
        Assert.True(result.IsAvailable);
        Assert.Equal(1, result.CategoryId);
    }

    [Fact]
    public async Task CreateAsync_PersistsFoodAndReturnsDto()
    {
        await using var context = CreateContext();
        await SeedCategoryAsync(context);
        var service = CreateService(context);
        var dto = new FoodCreateDto
        {
            Name = "Pizza",
            Description = "Cheese pizza",
            Price = 15.00m,
            IsAvailable = true,
            CategoryId = 1
        };

        var result = await service.CreateAsync(dto);

        Assert.NotNull(result);
        Assert.NotEqual(0, result!.FoodId);
        Assert.Equal("Pizza", result.Name);
        Assert.Single(context.Foods);
        Assert.Equal("Pizza", context.Foods.Single().Name);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesExistingFoodAndReturnsDto()
    {
        await using var context = CreateContext();
        await SeedCategoryAsync(context);
        context.Foods.Add(new Food
        {
            Id = 1,
            Name = "Burger",
            Description = "Classic burger",
            Price = 12.50m,
            IsAvailable = true,
            CategoryId = 1
        });
        await context.SaveChangesAsync();

        var service = CreateService(context);
        var dto = new FoodUpdateDto
        {
            FoodId = 1,
            Name = "Double Burger",
            Description = "Bigger burger",
            Price = 18.00m,
            IsAvailable = false,
            CategoryId = 1
        };

        var result = await service.UpdateAsync(1, dto);

        Assert.NotNull(result);
        Assert.Equal(1, result!.FoodId);
        Assert.Equal("Double Burger", result.Name);
        Assert.Equal("Bigger burger", result.Description);
        Assert.Equal(18.00m, result.Price);
        Assert.False(result.IsAvailable);
        Assert.Single(context.Foods);
        Assert.Equal("Double Burger", context.Foods.Single().Name);
    }

    [Fact]
    public async Task DeleteAsync_RemovesFoodAndReturnsTrue()
    {
        await using var context = CreateContext();
        await SeedCategoryAsync(context);
        context.Foods.Add(new Food
        {
            Id = 1,
            Name = "Burger",
            Description = "Classic burger",
            Price = 12.50m,
            IsAvailable = true,
            CategoryId = 1
        });
        await context.SaveChangesAsync();

        var service = CreateService(context);

        var deleted = await service.DeleteAsync(1);

        Assert.True(deleted);
        Assert.Empty(context.Foods);
    }

    private static FoodService CreateService(AppDbContext context)
    {
        return new FoodService(new FoodRepository(context), new CategoryRepository(context));
    }

    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    private static async Task SeedCategoryAsync(AppDbContext context)
    {
        context.Categories.Add(new Category { Id = 1, Name = "Main" });
        await context.SaveChangesAsync();
    }
}
