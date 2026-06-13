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
        var foods = await _foodRepository.GetAllAsync();
        return foods.Select(f => f.ToDto()).ToList();
    }

    public async Task<IReadOnlyCollection<FoodDto>> GetAvailableAsync()
    {
        var foods = await _foodRepository.GetAllAsync();
        return foods.Where(f => f.IsAvailable).Select(f => f.ToDto()).ToList();
    }

    public async Task<IReadOnlyCollection<FoodDto>> SearchAsync(string name)
    {
        var foods = await _foodRepository.GetAllAsync();
        return foods.Where(f => f.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                    .Select(f => f.ToDto()).ToList();
    }

    public async Task<IReadOnlyCollection<FoodDto>> GetByCategoryAsync(int categoryId)
    {
        var foods = await _foodRepository.GetAllAsync();
        return foods.Where(f => f.CategoryId == categoryId).Select(f => f.ToDto()).ToList();
    }

    public async Task<FoodDto?> GetByIdAsync(int id)
    {
        var food = await _foodRepository.GetByIdAsync(id);
        return food?.ToDto();
    }

    public async Task<FoodDto?> CreateAsync(FoodCreateDto dto)
    {
        var food = dto.ToEntity();
        var created = await _foodRepository.CreateAsync(food);
        return created.ToDto();
    }

    public async Task<FoodDto?> UpdateAsync(int id, FoodUpdateDto dto)
    {
        var food = await _foodRepository.GetByIdAsync(id);
        if (food == null) return null;

        food.ToUpdateEntity(dto);
        var updated = await _foodRepository.UpdateAsync(food);
        return updated.ToDto();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _foodRepository.DeleteAsync(id);
    }
}
