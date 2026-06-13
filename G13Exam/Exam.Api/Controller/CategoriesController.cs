using Exam.Api.Dtos;
using Exam.Api.Entities;
using Exam.Api.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Exam.Api.Controller
{
    [ApiController]
    [Route("categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly IBaseRepository<Category> _categoryRepository;
        private readonly IValidator<CategoryCreateDto> _categoryCreateValidator;
        private readonly IValidator<CategoryUpdateDto> _categoryUpdateValidator;

        public CategoriesController(
            IBaseRepository<Category> categoryRepository,
            IValidator<CategoryCreateDto> categoryCreateValidator,
            IValidator<CategoryUpdateDto> categoryUpdateValidator)
        {
            _categoryRepository = categoryRepository;
            _categoryCreateValidator = categoryCreateValidator;
            _categoryUpdateValidator = categoryUpdateValidator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryReadDto>>> GetAll()
        {
            var categories = await _categoryRepository.GetAllQuery()
                .Select(c => new CategoryReadDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();

            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryReadDto>> GetById(int id)
        {
            var category = await _categoryRepository.GetAllQuery()
                .Where(c => c.Id == id)
                .Select(c => new CategoryReadDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .FirstOrDefaultAsync();

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryReadDto>> Create(CategoryCreateDto dto)
        {
            var validationResult = await _categoryCreateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var category = new Category
            {
                Name = dto.Name
            };

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = category.Id }, new CategoryReadDto
            {
                Id = category.Id,
                Name = category.Name
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CategoryUpdateDto dto)
        {
            var validationResult = await _categoryUpdateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var category = await _categoryRepository.GetAllQuery().FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            category.Name = dto.Name;
            _categoryRepository.Update(category);
            await _categoryRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRepository.GetAllQuery().FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            _categoryRepository.Delete(category);
            await _categoryRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
