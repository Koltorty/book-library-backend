using BookLibrary.Api.DTOs.SeriesDtos;
using FluentValidation;

namespace BookLibrary.Api.Validation;

public class SeriesCreateDtoValidator : AbstractValidator<SeriesCreateDto>
{
    public SeriesCreateDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
    }
}
