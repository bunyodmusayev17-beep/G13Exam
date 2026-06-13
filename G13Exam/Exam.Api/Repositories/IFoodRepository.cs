using Exam.Api.Entities;

namespace Exam.Api.Repositories;

public interface IFoodRepository
{
    Task<List<Food>> GetAllAsync();
    Task<Food?> GetByIdAsync(long id);
    Task<Food> CreateAsync(Food food);
    Task<Food> UpdateAsync(Food food);
    Task<bool> DeleteAsync(long id);
}