using Exam.Api.Dtos;
using Exam.Api.Mappings;
using Exam.Api.Repositories;
using Microsoft.EntityFrameworkCore;

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
        var categories = await _categoryRepository.GetAllQuery().AsNoTracking().ToListAsync();
        return categories.Select(category => category.ToDto()).ToList();
    }

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        var category = await _categoryRepository.GetAllQuery()
            .AsNoTracking()
            .FirstOrDefaultAsync(categoryItem => categoryItem.Id == id);

        return category?.ToDto();
    }

    public async Task<CategoryDto?> CreateAsync(CategoryCreateDto dto)
    {
        var category = dto.ToEntity();
        await _categoryRepository.AddAsync(category);
        await _categoryRepository.SaveChangesAsync();

        return category.ToDto();
    }

    public async Task<CategoryDto?> UpdateAsync(int id, CategoryUpdateDto dto)
    {
        var category = await _categoryRepository.GetAllQuery().FirstOrDefaultAsync(categoryItem => categoryItem.Id == id);
        if (category == null)
        {
            return null;
        }

        category.ToUpdateEntity(dto);
        _categoryRepository.Update(category);
        await _categoryRepository.SaveChangesAsync();

        return category.ToDto();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category = await _categoryRepository.GetAllQuery().FirstOrDefaultAsync(categoryItem => categoryItem.Id == id);
        if (category == null)
        {
            return false;
        }

        _categoryRepository.Delete(category);
        await _categoryRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _categoryRepository.GetAllQuery().AnyAsync(category => category.Id == id);
    }
}