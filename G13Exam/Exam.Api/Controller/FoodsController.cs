using Exam.Api.Dtos;
using Exam.Api.Entities;
using Exam.Api.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Exam.Api.Controller
{
    [ApiController]
    [Route("foods")]
    public class FoodsController : ControllerBase
    {
        private readonly IBaseRepository<Food> _foodRepository;
        private readonly IBaseRepository<Category> _categoryRepository;
        private readonly IValidator<FoodCreateDto> _foodCreateValidator;
        private readonly IValidator<FoodUpdateDto> _foodUpdateValidator;

        public FoodsController(
            IBaseRepository<Food> foodRepository,
            IBaseRepository<Category> categoryRepository,
            IValidator<FoodCreateDto> foodCreateValidator,
            IValidator<FoodUpdateDto> foodUpdateValidator)
        {
            _foodRepository = foodRepository;
            _categoryRepository = categoryRepository;
            _foodCreateValidator = foodCreateValidator;
            _foodUpdateValidator = foodUpdateValidator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodReadDto>>> GetAll()
        {
            var foods = await _foodRepository.GetAllQuery()
                .Include(f => f.Category)
                .Select(f => new FoodReadDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    Price = f.Price,
                    IsAvailable = f.IsAvailable,
                    CategoryId = f.CategoryId,
                    CategoryName = f.Category.Name
                })
                .ToListAsync();

            return Ok(foods);
        }

        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<FoodReadDto>>> GetAvailable()
        {
            var foods = await _foodRepository.GetAllQuery()
                .Where(f => f.IsAvailable)
                .Include(f => f.Category)
                .Select(f => new FoodReadDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    Price = f.Price,
                    IsAvailable = f.IsAvailable,
                    CategoryId = f.CategoryId,
                    CategoryName = f.Category.Name
                })
                .ToListAsync();

            return Ok(foods);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<FoodReadDto>>> Search([FromQuery] string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Query parameter 'name' is required.");
            }

            var normalized = name.Trim();
            var foods = await _foodRepository.GetAllQuery()
                .Where(f => f.Name.Contains(normalized, StringComparison.OrdinalIgnoreCase))
                .Include(f => f.Category)
                .Select(f => new FoodReadDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    Price = f.Price,
                    IsAvailable = f.IsAvailable,
                    CategoryId = f.CategoryId,
                    CategoryName = f.Category.Name
                })
                .ToListAsync();

            return Ok(foods);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<FoodReadDto>>> GetByCategory(int categoryId)
        {
            var foods = await _foodRepository.GetAllQuery()
                .Where(f => f.CategoryId == categoryId)
                .Include(f => f.Category)
                .Select(f => new FoodReadDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    Price = f.Price,
                    IsAvailable = f.IsAvailable,
                    CategoryId = f.CategoryId,
                    CategoryName = f.Category.Name
                })
                .ToListAsync();

            return Ok(foods);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FoodReadDto>> GetById(int id)
        {
            var food = await _foodRepository.GetAllQuery()
                .Include(f => f.Category)
                .Select(f => new FoodReadDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    Price = f.Price,
                    IsAvailable = f.IsAvailable,
                    CategoryId = f.CategoryId,
                    CategoryName = f.Category.Name
                })
                .FirstOrDefaultAsync(f => f.Id == id);

            if (food == null)
            {
                return NotFound();
            }

            return Ok(food);
        }

        [HttpPost]
        public async Task<ActionResult<FoodReadDto>> Create(FoodCreateDto dto)
        {
            var validationResult = await _foodCreateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var categoryExists = await _categoryRepository.GetAllQuery().AnyAsync(c => c.Id == dto.CategoryId);
            if (!categoryExists)
            {
                return BadRequest("Category does not exist.");
            }

            var food = new Food
            {
                Name = dto.Name,
                Price = dto.Price,
                IsAvailable = dto.IsAvailable,
                CategoryId = dto.CategoryId
            };

            await _foodRepository.AddAsync(food);
            await _foodRepository.SaveChangesAsync();

            var createdDto = new FoodReadDto
            {
                Id = food.Id,
                Name = food.Name,
                Price = food.Price,
                IsAvailable = food.IsAvailable,
                CategoryId = food.CategoryId,
                CategoryName = await _categoryRepository.GetAllQuery()
                    .Where(c => c.Id == food.CategoryId)
                    .Select(c => c.Name)
                    .FirstOrDefaultAsync() ?? string.Empty
            };

            return CreatedAtAction(nameof(GetById), new { id = food.Id }, createdDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, FoodUpdateDto dto)
        {
            var validationResult = await _foodUpdateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var food = await _foodRepository.GetAllQuery().FirstOrDefaultAsync(f => f.Id == id);
            if (food == null)
            {
                return NotFound();
            }

            var categoryExists = await _categoryRepository.GetAllQuery().AnyAsync(c => c.Id == dto.CategoryId);
            if (!categoryExists)
            {
                return BadRequest("Category does not exist.");
            }

            food.Name = dto.Name;
            food.Price = dto.Price;
            food.IsAvailable = dto.IsAvailable;
            food.CategoryId = dto.CategoryId;

            _foodRepository.Update(food);
            await _foodRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var food = await _foodRepository.GetAllQuery().FirstOrDefaultAsync(f => f.Id == id);
            if (food == null)
            {
                return NotFound();
            }

            _foodRepository.Delete(food);
            await _foodRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
