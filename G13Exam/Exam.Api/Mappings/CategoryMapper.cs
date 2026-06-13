using Exam.Api.Dtos;
using Exam.Api.Entities;

namespace Exam.Api.Mappings;

public static class CategoryMapper
{
    public static CategoryDto ToDto(this Category category)
    {
        return new CategoryDto
        {
            CategoryId = category.CategoryId,
            Name = category.Name
        };
    }

    public static Category ToEntity(this CategoryCreateDto dto)
    {
        return new Category
        {
            Name = dto.Name
        };
    }

    public static void ToUpdateEntity(this Category category, CategoryUpdateDto dto)
    {
        category.Name = dto.Name;
    }
}
