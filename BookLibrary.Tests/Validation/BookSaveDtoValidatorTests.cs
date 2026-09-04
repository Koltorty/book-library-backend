using BookLibrary.Api.DTOs.BookDtos;
using BookLibrary.Api.Validation;
using BookLibrary.Data.Entities.Enums;

namespace BookLibrary.Tests.Validation;

public class BookSaveDtoValidatorTests
{
    private readonly BookSaveDtoValidator _validator = new();

    private static BookSaveDto CreateValidDto() => new()
    {
        Title = "War and Peace",
        Type = BookType.Paper,
        PagesCount = 1225,
        PublisherId = 1,
        CategoryIds = [1],
        Works = [new SaveWorkDto { Title = "War and Peace", Order = 1, AuthorIds = [1] }]
    };

    [Fact]
    public async Task Validate_WithValidDto_ShouldPass()
    {
        // Arrange
        var dto = CreateValidDto();

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task Validate_WithEmptyTitle_ShouldFail()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.Title = "";

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.Title));
    }

    [Fact]
    public async Task Validate_WithTooLongTitle_ShouldFail()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.Title = new string('x', 201);

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.Title));
    }

    [Fact]
    public async Task Validate_WithUndefinedType_ShouldFail()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.Type = (BookType)999;

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.Type));
    }

    [Fact]
    public async Task Validate_WithZeroPagesCount_ShouldFail()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.PagesCount = 0;

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.PagesCount));
    }

    [Fact]
    public async Task Validate_WithZeroPublisherId_ShouldFail()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.PublisherId = 0;

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.PublisherId));
    }

    [Fact]
    public async Task Validate_WithoutCategories_ShouldFail()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.CategoryIds = [];

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.CategoryIds));
    }

    [Fact]
    public async Task Validate_WithoutWorks_ShouldFail()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.Works = [];

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.Works));
    }

    [Fact]
    public async Task Validate_WithWorkWithoutAuthors_ShouldFail()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.Works = [new SaveWorkDto { Title = "War and Peace", Order = 1, AuthorIds = [] }];

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "At least one author is required");
    }

    [Fact]
    public async Task Validate_WithWorkWithoutTitle_ShouldFail()
    {
        // Arrange
        var dto = CreateValidDto();
        dto.Works = [new SaveWorkDto { Title = "", Order = 1, AuthorIds = [1] }];

        // Act
        var result = await _validator.ValidateAsync(dto);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName.Contains("Title"));
    }
}
