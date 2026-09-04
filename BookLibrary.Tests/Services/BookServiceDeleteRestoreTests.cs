using BookLibrary.Api.DTOs.BookDtos;
using BookLibrary.Data.Entities.Enums;

namespace BookLibrary.Tests.Services;

public class BookServiceDeleteRestoreTests : ServiceTestBase
{
    private async Task<int> AddSimpleBookAsync(string title)
    {
        var authorId = await _authorService.AddAuthor("Leo Tolstoy");
        var categoryId = await _categoryService.AddCategory("Fiction");
        var publisherId = await _publisherService.AddPublisher("AST");

        return await _bookService.AddBook(new BookSaveDto
        {
            Title = title,
            PagesCount = 1225,
            Type = BookType.Paper,
            PublisherId = publisherId,
            CategoryIds = [categoryId],
            Works = [new SaveWorkDto { Title = title, Order = 1, AuthorIds = [authorId] }]
        });
    }

    [Fact]
    public async Task DeleteBook_ShouldHideBookFromDetailsAndLists()
    {
        // Arrange
        var bookId = await AddSimpleBookAsync("War and Peace");

        // Act
        var deleted = await _bookService.DeleteBook(bookId);

        // Assert
        Assert.True(deleted);
        Assert.Null(await _bookService.GetBook(bookId));

        var books = await _bookService.GetBooks();
        Assert.Equal(0, books.TotalCount);
        Assert.Empty(books.Items);
    }

    [Fact]
    public async Task DeleteBook_WhenBookDoesNotExist_ShouldReturnFalse()
    {
        // Act
        var deleted = await _bookService.DeleteBook(999);

        // Assert
        Assert.False(deleted);
    }

    [Fact]
    public async Task RestoreBook_AfterDelete_ShouldMakeBookVisibleAgain()
    {
        // Arrange
        var bookId = await AddSimpleBookAsync("War and Peace");
        await _bookService.DeleteBook(bookId);

        // Act
        var restored = await _bookService.RestoreBook(bookId);

        // Assert
        Assert.True(restored);

        var book = await _bookService.GetBook(bookId);
        Assert.NotNull(book);
        Assert.Equal("War and Peace", book!.Title);

        var work = Assert.Single(book.Works);
        Assert.Equal("War and Peace", work.Title);
        Assert.Equal("Leo Tolstoy", Assert.Single(work.Authors));

        var books = await _bookService.GetBooks();
        Assert.Equal(1, books.TotalCount);
    }

    [Fact]
    public async Task RestoreBook_WhenBookDoesNotExist_ShouldReturnFalse()
    {
        // Act
        var restored = await _bookService.RestoreBook(999);

        // Assert
        Assert.False(restored);
    }

    [Fact]
    public async Task AddBook_AfterAllBooksOfAuthorDeleted_ShouldReuseAuthorWithoutDuplicates()
    {
        // Arrange
        var authorId = await _authorService.AddAuthor("Ivan Ivanov");
        var categoryId = await _categoryService.AddCategory("Fiction");
        var publisherId = await _publisherService.AddPublisher("AST");

        var firstBookId = await _bookService.AddBook(new BookSaveDto
        {
            Title = "First Book",
            PagesCount = 300,
            Type = BookType.Paper,
            PublisherId = publisherId,
            CategoryIds = [categoryId],
            Works = [new SaveWorkDto { Title = "First Story", Order = 1, AuthorIds = [authorId] }]
        });

        // Act
        await _bookService.DeleteBook(firstBookId);

        // Assert: активный список пуст, полный содержит Иванова
        var active = await _authorService.GetAuthors(onlyActive: true);
        Assert.Empty(active);

        var all = await _authorService.GetAuthors(onlyActive: false);
        Assert.Single(all);
        Assert.Equal(authorId, all[0].Id);

        var secondBookId = await _bookService.AddBook(new BookSaveDto
        {
            Title = "Second Book",
            PagesCount = 250,
            Type = BookType.Paper,
            PublisherId = publisherId,
            CategoryIds = [categoryId],
            Works = [new SaveWorkDto { Title = "Second Story", Order = 1, AuthorIds = [all[0].Id] }]
        });

        var secondBook = await _bookService.GetBook(secondBookId);
        Assert.NotNull(secondBook);
        Assert.Equal("Ivan Ivanov", Assert.Single(secondBook!.Works).Authors.Single());

        var allAfter = await _authorService.GetAuthors(onlyActive: false);
        Assert.Single(allAfter);
    }
}
