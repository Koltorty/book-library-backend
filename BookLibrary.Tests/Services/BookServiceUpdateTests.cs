using BookLibrary.Api.DTOs.BookDtos;
using BookLibrary.Api.DTOs.SeriesDtos;
using BookLibrary.Data.Entities.Enums;

namespace BookLibrary.Tests.Services;

public class BookServiceUpdateTests : ServiceTestBase
{
    [Fact]
    public async Task UpdateBook_WithChangedSimpleFields_ShouldOverwriteThem()
    {
        // Arrange
        var authorId = await _authorService.AddAuthor("Leo Tolstoy");
        var categoryId = await _categoryService.AddCategory("Fiction");
        var publisherId = await _publisherService.AddPublisher("AST");

        var bookId = await _bookService.AddBook(new BookSaveDto
        {
            Title = "War and Peace",
            PagesCount = 1225,
            Type = BookType.Paper,
            CoverType = BookCoverType.HardCover,
            HasBeenRead = true,
            DateRead = new DateOnly(2025, 1, 15),
            VolumeNumber = 1,
            PublisherId = publisherId,
            CategoryIds = [categoryId],
            Works = [new SaveWorkDto { Title = "War and Peace", Order = 1, AuthorIds = [authorId] }]
        });

        var workId = Assert.Single((await _bookService.GetBook(bookId))!.Works).Id;

        var dto = new BookSaveDto
        {
            Title = "Anna Karenina",
            PagesCount = 864,
            Type = BookType.Ebook,
            CoverType = null,
            HasBeenRead = false,
            DateRead = null,
            VolumeNumber = 2,
            PublisherId = publisherId,
            CategoryIds = [categoryId],
            Works = [new SaveWorkDto { Id = workId, Title = "Anna Karenina", Order = 1, AuthorIds = [authorId] }]
        };

        // Act
        var updated = await _bookService.UpdateBook(bookId, dto);

        // Assert
        Assert.True(updated);

        var book = await _bookService.GetBook(bookId);
        Assert.NotNull(book);
        Assert.Equal("Anna Karenina", book!.Title);
        Assert.Equal(864, book.PagesCount);
        Assert.Equal("Ebook", book.Type);
        Assert.Null(book.CoverType);
        Assert.False(book.HasBeenRead);
        Assert.Null(book.DateRead);
        Assert.Equal(2, book.VolumeNumber);
    }

    [Fact]
    public async Task UpdateBook_WithChangedAuthor_Series_AndCategory_ShouldOverwriteThem()
    {
        // Arrange
        var tolstoyId = await _authorService.AddAuthor("Leo Tolstoy");
        var dostoevskyId = await _authorService.AddAuthor("Fyodor Dostoevsky");
        var fictionId = await _categoryService.AddCategory("Fiction");
        var scienceId = await _categoryService.AddCategory("Science");
        var astId = await _publisherService.AddPublisher("AST");
        var eksmoId = await _publisherService.AddPublisher("Eksmo");

        var bookId = await _bookService.AddBook(new BookSaveDto
        {
            Title = "War and Peace",
            PagesCount = 1225,
            Type = BookType.Paper,
            PublisherId = astId,
            CategoryIds = [fictionId],
            Works = [new SaveWorkDto { Title = "War and Peace", Order = 1, AuthorIds = [tolstoyId] }]
        });

        var seriesId = await _seriesService.AddSeries(new SeriesCreateDto { Title = "Classics Collection" });

        var workId = Assert.Single((await _bookService.GetBook(bookId))!.Works).Id;

        var dto = new BookSaveDto
        {
            Title = "Crime and Punishment",
            PagesCount = 673,
            Type = BookType.Paper,
            SeriesId = seriesId,
            PublisherId = eksmoId,
            CategoryIds = [scienceId],
            Works = [new SaveWorkDto { Id = workId, Title = "Crime and Punishment", Order = 1, AuthorIds = [dostoevskyId] }]
        };

        // Act
        var updated = await _bookService.UpdateBook(bookId, dto);

        // Assert
        Assert.True(updated);

        var book = await _bookService.GetBook(bookId);
        Assert.NotNull(book);
        Assert.Equal(seriesId, book!.SeriesId);
        Assert.Equal("Classics Collection", book.SeriesTitle);
        Assert.Equal(eksmoId, book.PublisherId);
        Assert.Equal("Eksmo", book.PublisherName);

        var work = Assert.Single(book.Works);
        Assert.Equal(workId, work.Id);
        Assert.Equal("Crime and Punishment", work.Title);
        Assert.Equal("Fyodor Dostoevsky", Assert.Single(work.Authors));

        var category = Assert.Single(book.Categories);
        Assert.Equal("Science", category.Name);
    }

    [Fact]
    public async Task UpdateBook_WhenWorkMissingFromRequest_ShouldMarkItDeleted()
    {
        // Arrange
        var authorId = await _authorService.AddAuthor("Mikhail Bulgakov");
        var categoryId = await _categoryService.AddCategory("Fiction");
        var publisherId = await _publisherService.AddPublisher("AST");

        var bookId = await _bookService.AddBook(new BookSaveDto
        {
            Title = "The Master and Margarita",
            PagesCount = 480,
            Type = BookType.Paper,
            PublisherId = publisherId,
            CategoryIds = [categoryId],
            Works =
            [
                new SaveWorkDto { Title = "Part One", Order = 1, AuthorIds = [authorId] },
                new SaveWorkDto { Title = "Part Two", Order = 2, AuthorIds = [authorId] }
            ]
        });

        var works = (await _bookService.GetBook(bookId))!.Works;
        var keptWorkId = works[0].Id;

        var dto = new BookSaveDto
        {
            Title = "The Master and Margarita",
            PagesCount = 480,
            Type = BookType.Paper,
            PublisherId = publisherId,
            CategoryIds = [categoryId],
            Works = [new SaveWorkDto { Id = keptWorkId, Title = "Part One", Order = 1, AuthorIds = [authorId] }]
        };

        // Act
        var updated = await _bookService.UpdateBook(bookId, dto);

        // Assert
        Assert.True(updated);

        var book = await _bookService.GetBook(bookId);
        Assert.NotNull(book);
        var work = Assert.Single(book!.Works);
        Assert.Equal(keptWorkId, work.Id);
        Assert.Equal("Part One", work.Title);
    }

    [Fact]
    public async Task UpdateBook_WhenBookDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        var dto = new BookSaveDto
        {
            Title = "Some Book",
            Type = BookType.Paper,
            PublisherId = 1,
            CategoryIds = [1],
            Works = [new SaveWorkDto { Title = "Some Work", Order = 1, AuthorIds = [1] }]
        };

        // Act
        var updated = await _bookService.UpdateBook(999, dto);

        // Assert
        Assert.False(updated);
    }
}
