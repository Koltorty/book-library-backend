using BookLibrary.Api.DTOs.AuthorDtos;
using FluentValidation;

namespace BookLibrary.Api.Validation;

public class AuthorCreateDtoValidator : AbstractValidator<AuthorCreateDto>
{
    public AuthorCreateDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}
