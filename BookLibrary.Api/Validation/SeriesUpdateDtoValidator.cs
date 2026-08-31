using BookLibrary.Api.DTOs.SeriesDtos;
using FluentValidation;

namespace BookLibrary.Api.Validation;

public class SeriesUpdateDtoValidator : AbstractValidator<SeriesUpdateDto>
{
    public SeriesUpdateDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
    }
}
