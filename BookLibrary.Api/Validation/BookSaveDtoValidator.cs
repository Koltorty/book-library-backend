using BookLibrary.Api.DTOs.BookDtos;
using BookLibrary.Data.Entities.Enums;
using FluentValidation;

namespace BookLibrary.Api.Validation;

public class BookSaveDtoValidator : AbstractValidator<BookSaveDto>
{
    public BookSaveDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.CoverType).NotEmpty().When(x => x.Type == BookType.Paper);
        RuleFor(x => x.PagesCount).GreaterThan(0);
        RuleFor(x => x.PublisherId).GreaterThan(0);
        RuleFor(x => x.CategoryIds).NotEmpty().WithMessage("At least one category is required");
        RuleFor(x => x.Works).NotEmpty().WithMessage("At least one work is required");
        RuleForEach(x => x.Works).SetValidator(new SaveWorkDtoValidator());
    }
}

public class SaveWorkDtoValidator : AbstractValidator<SaveWorkDto>
{
    public SaveWorkDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
    }
}
