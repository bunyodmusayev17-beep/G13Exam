using Exam.Api.Entities;

namespace Exam.Api.Repositories;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(long id);
    Task<Category> CreateAsync(Category category);
    Task<Category> UpdateAsync(Category category);
    Task<bool> DeleteAsync(long id);
}
