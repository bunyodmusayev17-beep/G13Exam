using Exam.Api.Dtos;
using Exam.Api.Entities;
using Exam.Api.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Exam.Api.Controller
{
    [ApiController]
    [Route("categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IValidator<CategoryCreateDto> _categoryCreateValidator;
        private readonly IValidator<CategoryUpdateDto> _categoryUpdateValidator;

        public CategoriesController(
            ICategoryService categoryService,
            IValidator<CategoryCreateDto> categoryCreateValidator,
            IValidator<CategoryUpdateDto> categoryUpdateValidator)
        {
            _categoryService = categoryService;
            _categoryCreateValidator = categoryCreateValidator;
            _categoryUpdateValidator = categoryUpdateValidator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();

            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> Create(CategoryCreateDto dto)
        {
            var validationResult = await _categoryCreateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var category = await _categoryService.CreateAsync(dto);
            if (category == null)
            {
                return BadRequest("Category could not be created.");
            }

            return CreatedAtAction(nameof(GetById), new { id = category.CategoryId }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CategoryUpdateDto dto)
        {
            var validationResult = await _categoryUpdateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            await _categoryService.UpdateAsync(id, dto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _categoryService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
