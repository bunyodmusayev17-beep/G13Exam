using Exam.Api.Dtos;
using FluentValidation;

namespace Exam.Api.Validators
{
    public class FoodUpdateDtoValidator : AbstractValidator<FoodUpdateDto>
    {
        public FoodUpdateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Price)
                .GreaterThan(0);

            RuleFor(x => x.CategoryId)
                .GreaterThan(0);
        }
    }
}
