using Exam.Api.Dtos;
using Exam.Api.Entities;
using Exam.Api.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Exam.Api.Controller
{
    [ApiController]
    [Route("foods")]
    public class FoodsController : ControllerBase
    {
        private readonly IFoodService _foodService;
        private readonly ICategoryService _categoryService;
        private readonly IValidator<FoodCreateDto> _foodCreateValidator;
        private readonly IValidator<FoodUpdateDto> _foodUpdateValidator;

        public FoodsController(
            IFoodService foodService,
            ICategoryService categoryService,
            IValidator<FoodCreateDto> foodCreateValidator,
            IValidator<FoodUpdateDto> foodUpdateValidator)
        {
            _foodService = foodService;
            _categoryService = categoryService;
            _foodCreateValidator = foodCreateValidator;
            _foodUpdateValidator = foodUpdateValidator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodDto>>> GetAll()
        {
            var foods = await _foodService.GetAllAsync();

            return Ok(foods);
        }

        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<FoodDto>>> GetAvailable()
        {
            var foods = await _foodService.GetAvailableAsync();

            return Ok(foods);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<FoodDto>>> Search([FromQuery] string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Query parameter 'name' is required.");
            }

            var foods = await _foodService.SearchAsync(name);

            return Ok(foods);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<FoodDto>>> GetByCategory(int categoryId)
        {
            var foods = await _foodService.GetByCategoryAsync(categoryId);

            return Ok(foods);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FoodDto>> GetById(int id)
        {
            var food = await _foodService.GetByIdAsync(id);

            if (food == null)
            {
                return NotFound();
            }

            return Ok(food);
        }

        [HttpPost]
        public async Task<ActionResult<FoodDto>> Create(FoodCreateDto dto)
        {
            var validationResult = await _foodCreateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var categoryExists = await _categoryService.ExistsAsync(dto.CategoryId);
            if (!categoryExists)
            {
                return BadRequest("Category does not exist.");
            }

            var createdDto = await _foodService.CreateAsync(dto);
            if (createdDto == null)
            {
                return BadRequest("Food could not be created.");
            }

            return CreatedAtAction(nameof(GetById), new { id = createdDto.FoodId }, createdDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, FoodUpdateDto dto)
        {
            var validationResult = await _foodUpdateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var food = await _foodService.GetByIdAsync(id);
            if (food == null)
            {
                return NotFound();
            }

            var categoryExists = await _categoryService.ExistsAsync(dto.CategoryId);
            if (!categoryExists)
            {
                return BadRequest("Category does not exist.");
            }

            await _foodService.UpdateAsync(id, dto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _foodService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
