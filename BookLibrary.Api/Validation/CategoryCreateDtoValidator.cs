using BookLibrary.Api.DTOs.CategoryDtos;
using FluentValidation;

namespace BookLibrary.Api.Validation;

public class CategoryCreateDtoValidator : AbstractValidator<CategoryCreateDto>
{
    public CategoryCreateDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
    }
}
