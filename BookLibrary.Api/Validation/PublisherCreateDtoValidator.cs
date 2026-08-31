using BookLibrary.Api.DTOs.PublisherDtos;
using FluentValidation;

namespace BookLibrary.Api.Validation;

public class PublisherCreateDtoValidator : AbstractValidator<PublisherCreateDto>
{
    public PublisherCreateDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
    }
}
