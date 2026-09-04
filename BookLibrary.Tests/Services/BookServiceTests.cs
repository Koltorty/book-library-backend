using BookLibrary.Api.DTOs.BookDtos;
using BookLibrary.Data.Entities.Enums;

namespace BookLibrary.Tests.Services;

public class BookServiceTests : ServiceTestBase
{
    [Fact]
    public async Task AddBook_WithOneAuthor_WithOneWork_ShouldSaveBookWithAllFields()
    {
        // Arrange
        var authorId = await _authorService.AddAuthor("Leo Tolstoy");
        var categoryId = await _categoryService.AddCategory("Fiction");
        var publisherId = await _publisherService.AddPublisher("AST");

        var dto = new BookSaveDto
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
            Works =
            [
                new SaveWorkDto
                {
                    Title = "War and Peace",
                    Order = 1,
                    AuthorIds = [authorId]
                }
            ]
        };

        // Act
        var bookId = await _bookService.AddBook(dto);

        // Assert
        var book = await _bookService.GetBook(bookId);
        Assert.NotNull(book);
        Assert.Equal("War and Peace", book.Title);
        Assert.Equal(1225, book.PagesCount);
        Assert.Equal("Paper", book.Type);
        Assert.Equal("HardCover", book.CoverType);
        Assert.True(book.HasBeenRead);
        Assert.Equal(new DateOnly(2025, 1, 15), book.DateRead);
        Assert.Equal(1, book.VolumeNumber);
        Assert.Equal(publisherId, book.PublisherId);
        Assert.Equal("AST", book.PublisherName);

        var work = Assert.Single(book.Works);
        Assert.Equal("War and Peace", work.Title);
        Assert.Equal(1, work.Order);
        Assert.Equal("Leo Tolstoy", Assert.Single(work.Authors));

        var bookCategory = Assert.Single(book.Categories);
        Assert.Equal("Fiction", bookCategory.Name);
    }

    [Fact]
    public async Task AddBook_WithOneAuthor_WithMultipleWorks_ShouldSaveAllWorks()
    {
        // Arrange
        var authorId = await _authorService.AddAuthor("Mikhail Bulgakov");
        var categoryId = await _categoryService.AddCategory("Fiction");
        var publisherId = await _publisherService.AddPublisher("AST");

        var dto = new BookSaveDto
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
        };

        // Act
        var bookId = await _bookService.AddBook(dto);

        // Assert
        var book = await _bookService.GetBook(bookId);
        Assert.NotNull(book);
        Assert.Equal(2, book.Works.Count);
        Assert.Equal("Part One", book.Works[0].Title);
        Assert.Equal(1, book.Works[0].Order);
        Assert.Equal("Part Two", book.Works[1].Title);
        Assert.Equal(2, book.Works[1].Order);
        Assert.All(book.Works, w => Assert.Equal("Mikhail Bulgakov", Assert.Single(w.Authors)));
    }

    [Fact]
    public async Task AddBook_WithMultipleAuthors_WithOneWork_ShouldSaveAllAuthors()
    {
        // Arrange
        var ilfId = await _authorService.AddAuthor("Ilf");
        var petrovId = await _authorService.AddAuthor("Petrov");
        var categoryId = await _categoryService.AddCategory("Fiction");
        var publisherId = await _publisherService.AddPublisher("AST");

        var dto = new BookSaveDto
        {
            Title = "The Twelve Chairs",
            PagesCount = 400,
            Type = BookType.Paper,
            PublisherId = publisherId,
            CategoryIds = [categoryId],
            Works =
            [
                new SaveWorkDto
                {
                    Title = "The Twelve Chairs",
                    Order = 1,
                    AuthorIds = [ilfId, petrovId]
                }
            ]
        };

        // Act
        var bookId = await _bookService.AddBook(dto);

        // Assert
        var book = await _bookService.GetBook(bookId);
        Assert.NotNull(book);
        var work = Assert.Single(book.Works);
        Assert.Contains("Ilf", work.Authors);
        Assert.Contains("Petrov", work.Authors);
        Assert.Equal(2, work.Authors.Count);
    }

    [Fact]
    public async Task AddBook_WithMultipleAuthors_WithMultipleWorks_ShouldSaveEverything()
    {
        // Arrange
        var pushkinId = await _authorService.AddAuthor("Alexander Pushkin");
        var gogolId = await _authorService.AddAuthor("Nikolai Gogol");
        var categoryId = await _categoryService.AddCategory("Fiction");
        var publisherId = await _publisherService.AddPublisher("AST");

        var dto = new BookSaveDto
        {
            Title = "Anthology of Russian Classics",
            PagesCount = 600,
            Type = BookType.Paper,
            PublisherId = publisherId,
            CategoryIds = [categoryId],
            Works =
            [
                new SaveWorkDto
                {
                    Title = "The Queen of Spades",
                    Order = 1,
                    AuthorIds = [pushkinId]
                },
                new SaveWorkDto
                {
                    Title = "The Overcoat",
                    Order = 2,
                    AuthorIds = [gogolId]
                },
                new SaveWorkDto
                {
                    Title = "Ruslan and Ludmila",
                    Order = 3,
                    AuthorIds = [pushkinId]
                }
            ]
        };

        // Act
        var bookId = await _bookService.AddBook(dto);

        // Assert
        var book = await _bookService.GetBook(bookId);
        Assert.NotNull(book);
        Assert.Equal(3, book.Works.Count);

        Assert.Equal("The Queen of Spades", book.Works[0].Title);
        Assert.Equal(1, book.Works[0].Order);
        Assert.Equal("Alexander Pushkin", Assert.Single(book.Works[0].Authors));

        Assert.Equal("The Overcoat", book.Works[1].Title);
        Assert.Equal(2, book.Works[1].Order);
        Assert.Equal("Nikolai Gogol", Assert.Single(book.Works[1].Authors));

        Assert.Equal("Ruslan and Ludmila", book.Works[2].Title);
        Assert.Equal(3, book.Works[2].Order);
        Assert.Equal("Alexander Pushkin", Assert.Single(book.Works[2].Authors));
    }
}
