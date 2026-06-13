using Exam.Api.Dtos;
using Exam.Api.Mappings;
using Exam.Api.Repositories;

namespace Exam.Api.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IReadOnlyCollection<CategoryDto>> GetAllAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return categories.Select(c => c.ToDto()).ToList();
    }

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        return category?.ToDto();
    }

    public async Task<CategoryDto?> CreateAsync(CategoryCreateDto dto)
    {
        var category = dto.ToEntity();
        var created = await _categoryRepository.CreateAsync(category);
        return created.ToDto();
    }

    public async Task<CategoryDto?> UpdateAsync(int id, CategoryUpdateDto dto)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null) return null;
        category.ToUpdateEntity(dto);
        var updated = await _categoryRepository.UpdateAsync(category);
        return updated.ToDto();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _categoryRepository.DeleteAsync(id);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        return category != null;
    }
}
