using Exam.Api.Dtos;
using Exam.Api.Data;
using Exam.Api.Entities;
using Exam.Api.Repositories;
using Exam.Api.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Exam.Api.Tests;

public class CategoryServiceTests
{
    [Fact]
    public async Task GetAllAsync_ReturnsMappedCategories()
    {
        await using var context = CreateContext();
        context.Categories.AddRange(
            new Category { Id = 1, Name = "Starters" },
            new Category { Id = 2, Name = "Drinks" });
        await context.SaveChangesAsync();

        var repository = new InMemoryCategoryRepository(context);
        var service = new CategoryService(repository);

        var result = await service.GetAllAsync();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, category => category.CategoryId == 1 && category.Name == "Starters");
        Assert.Contains(result, category => category.CategoryId == 2 && category.Name == "Drinks");
    }

    [Fact]
    public async Task CreateAsync_PersistsCategoryAndReturnsDto()
    {
        await using var context = CreateContext();
        var repository = new InMemoryCategoryRepository(context);
        var service = new CategoryService(repository);
        var dto = new CategoryCreateDto { Name = "Desserts" };

        var result = await service.CreateAsync(dto);

        Assert.NotNull(result);
        Assert.Equal("Desserts", result!.Name);
        Assert.Equal(1, result.CategoryId);
        Assert.Single(context.Categories.Local);
        Assert.Equal("Desserts", context.Categories.Local.Single().Name);
    }

    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    private sealed class InMemoryCategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public InMemoryCategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<Category> GetAllQuery()
        {
            return _context.Categories;
        }

        public async Task AddAsync(Category t)
        {
            await _context.Categories.AddAsync(t);
        }

        public void Update(Category t)
        {
            _context.Categories.Update(t);
        }

        public void Delete(Category t)
        {
            _context.Categories.Remove(t);
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
