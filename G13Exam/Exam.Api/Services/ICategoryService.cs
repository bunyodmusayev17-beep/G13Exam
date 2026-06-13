using Exam.Api.Dtos;

namespace Exam.Api.Services;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetCategoriesAsync();
    Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto dto);
}