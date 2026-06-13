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

    public async Task<List<CategoryDto>> GetCategoriesAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();

        return categories
            .Select(category => category.ToDto())
            .ToList();
    }

    public async Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto dto)
    {
        var category = dto.ToEntity();

        var createdCategory = await _categoryRepository.CreateAsync(category);

        return createdCategory.ToDto();
    }
}
