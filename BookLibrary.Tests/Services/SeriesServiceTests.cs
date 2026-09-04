using BookLibrary.Api.DTOs.BookDtos;
using BookLibrary.Api.DTOs.SeriesDtos;
using BookLibrary.Data.Entities.Enums;

namespace BookLibrary.Tests.Services;

public class SeriesServiceTests : ServiceTestBase
{
    [Fact]
    public async Task AddSeries_WithSameTitle_ShouldReturnExistingId()
    {
        // Act
        var firstId = await _seriesService.AddSeries(new SeriesCreateDto { Title = "Sandman" });
        var secondId = await _seriesService.AddSeries(new SeriesCreateDto { Title = "Sandman" });

        // Assert
        Assert.Equal(firstId, secondId);

        var series = await _seriesService.GetAllSeries(onlyActive: false);
        Assert.Single(series);
    }

    [Fact]
    public async Task GetSeries_ShouldHideRootsAndSubSeriesWithoutBooks()
    {
        // Arrange
        var rootId = await _seriesService.AddSeries(new SeriesCreateDto { Title = "Sandman" });
        var subWithBookId = await _seriesService.AddSeries(new SeriesCreateDto
        {
            Title = "Season of Mists",
            ParentSeriesId = rootId
        });
        await _seriesService.AddSeries(new SeriesCreateDto
        {
            Title = "Empty Sub-Series",
            ParentSeriesId = rootId
        });
        await _seriesService.AddSeries(new SeriesCreateDto { Title = "Empty Root" });

        var authorId = await _authorService.AddAuthor("Neil Gaiman");
        var categoryId = await _categoryService.AddCategory("Fiction");
        var publisherId = await _publisherService.AddPublisher("AST");

        await _bookService.AddBook(new BookSaveDto
        {
            Title = "Sandman Vol. 1",
            PagesCount = 240,
            Type = BookType.Paper,
            SeriesId = rootId,
            PublisherId = publisherId,
            CategoryIds = [categoryId],
            Works = [new SaveWorkDto { Title = "Sandman Vol. 1", Order = 1, AuthorIds = [authorId] }]
        });

        await _bookService.AddBook(new BookSaveDto
        {
            Title = "Sandman Vol. 2",
            PagesCount = 200,
            Type = BookType.Paper,
            SeriesId = subWithBookId,
            PublisherId = publisherId,
            CategoryIds = [categoryId],
            Works = [new SaveWorkDto { Title = "Sandman Vol. 2", Order = 1, AuthorIds = [authorId] }]
        });

        // Act
        var tree = await _seriesService.GetSeries();

        // Assert
        var root = Assert.Single(tree);
        Assert.Equal(rootId, root.Id);
        Assert.Equal("Sandman", root.Title);

        var sub = Assert.Single(root.SubSeries);
        Assert.Equal(subWithBookId, sub.Id);
        Assert.Equal("Season of Mists", sub.Title);
    }
}
