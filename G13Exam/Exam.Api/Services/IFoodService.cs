using Exam.Api.Dtos;

namespace Exam.Api.Services;

public interface IFoodService
{
    Task<IReadOnlyCollection<FoodDto>> GetAllAsync();
    Task<IReadOnlyCollection<FoodDto>> GetAvailableAsync();
    Task<IReadOnlyCollection<FoodDto>> SearchAsync(string name);
    Task<IReadOnlyCollection<FoodDto>> GetByCategoryAsync(int categoryId);
    Task<FoodDto?> GetByIdAsync(int id);
    Task<FoodDto?> CreateAsync(FoodCreateDto dto);
    Task<FoodDto?> UpdateAsync(int id, FoodUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}