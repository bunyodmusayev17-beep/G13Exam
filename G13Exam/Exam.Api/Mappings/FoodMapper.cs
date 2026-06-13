using Exam.Api.Dtos;
using Exam.Api.Entities;

namespace Exam.Api.Mappings;

public static class FoodMapper
{
    public static FoodDto ToDto(this Food food)
    {
        return new FoodDto
        {
            FoodId = (int)food.FoodId,
            Name = food.Name,
            Description = food.Discription,
            Price = food.Price,
            IsAvailable = food.IsAvailable,
            CategoryId = (int)food.CategoryId
        };
    }

    public static Food ToEntity(this FoodCreateDto dto)
    {
        return new Food
        {
            Name = dto.Name,
            Discription = dto.Description,
            Price = dto.Price,
            IsAvailable = dto.IsAvailable,
            CategoryId = dto.CategoryId
        };
    }

    public static void ToUpdateEntity(this Food food, FoodUpdateDto dto)
    {
        food.Name = dto.Name;
        food.Discription = dto.Description;
        food.Price = dto.Price;
        food.IsAvailable = dto.IsAvailable;
        food.CategoryId = dto.CategoryId;
    }
}