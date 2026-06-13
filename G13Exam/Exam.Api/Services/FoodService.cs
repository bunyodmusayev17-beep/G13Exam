using Exam.Api.Dtos;
using Exam.Api.Mappings;
using Exam.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Exam.Api.Services;

public class FoodService : IFoodService
{
    private readonly IFoodRepository _foodRepository;
    private readonly ICategoryRepository _categoryRepository;

    public FoodService(IFoodRepository foodRepository, ICategoryRepository categoryRepository)
    {
        _foodRepository = foodRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<IReadOnlyCollection<FoodDto>> GetAllAsync()
    {
        var foods = await _foodRepository.GetAllQuery().AsNoTracking().ToListAsync();
        return foods.Select(food => food.ToDto()).ToList();
    }

    public async Task<IReadOnlyCollection<FoodDto>> GetAvailableAsync()
    {
        var foods = await _foodRepository.GetAllQuery()
            .AsNoTracking()
            .Where(food => food.IsAvailable)
            .ToListAsync();

        return foods.Select(food => food.ToDto()).ToList();
    }

    public async Task<IReadOnlyCollection<FoodDto>> SearchAsync(string name)
    {
        var normalizedName = name.Trim();
        var foods = await _foodRepository.GetAllQuery()
            .AsNoTracking()
            .Where(food => EF.Functions.Like(food.Name, $"%{normalizedName}%"))
            .ToListAsync();

        return foods.Select(food => food.ToDto()).ToList();
    }

    public async Task<IReadOnlyCollection<FoodDto>> GetByCategoryAsync(int categoryId)
    {
        var foods = await _foodRepository.GetAllQuery()
            .AsNoTracking()
            .Where(food => food.CategoryId == categoryId)
            .ToListAsync();

        return foods.Select(food => food.ToDto()).ToList();
    }

    public async Task<FoodDto?> GetByIdAsync(int id)
    {
        var food = await _foodRepository.GetAllQuery()
            .AsNoTracking()
            .FirstOrDefaultAsync(foodItem => foodItem.Id == id);

        return food?.ToDto();
    }

    public async Task<FoodDto?> CreateAsync(FoodCreateDto dto)
    {
        var categoryExists = await _categoryRepository.GetAllQuery().AnyAsync(category => category.Id == dto.CategoryId);
        if (!categoryExists)
        {
            return null;
        }

        var food = dto.ToEntity();
        await _foodRepository.AddAsync(food);
        await _foodRepository.SaveChangesAsync();

        return food.ToDto();
    }

    public async Task<FoodDto?> UpdateAsync(int id, FoodUpdateDto dto)
    {
        var food = await _foodRepository.GetAllQuery().FirstOrDefaultAsync(foodItem => foodItem.Id == id);
        if (food == null)
        {
            return null;
        }

        var categoryExists = await _categoryRepository.GetAllQuery().AnyAsync(category => category.Id == dto.CategoryId);
        if (!categoryExists)
        {
            return null;
        }

        food.ToUpdateEntity(dto);
        _foodRepository.Update(food);
        await _foodRepository.SaveChangesAsync();

        return food.ToDto();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var food = await _foodRepository.GetAllQuery().FirstOrDefaultAsync(foodItem => foodItem.Id == id);
        if (food == null)
        {
            return false;
        }

        _foodRepository.Delete(food);
        await _foodRepository.SaveChangesAsync();
        return true;
    }
}