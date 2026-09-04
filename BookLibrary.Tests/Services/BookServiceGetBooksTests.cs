using BookLibrary.Api.DTOs.BookDtos;
using BookLibrary.Api.DTOs.Common;
using BookLibrary.Data.Entities.Enums;

namespace BookLibrary.Tests.Services;

public class BookServiceGetBooksTests : ServiceTestBase
{
    private async Task<int> AddBookAsync(
        string title,
        BookType type = BookType.Paper,
        BookCoverType? coverType = null,
        bool hasBeenRead = false)
    {
        var authorId = await _authorService.AddAuthor("Leo Tolstoy");
        var categoryId = await _categoryService.AddCategory("Fiction");
        var publisherId = await _publisherService.AddPublisher("AST");

        return await _bookService.AddBook(new BookSaveDto
        {
            Title = title,
            PagesCount = 100,
            Type = type,
            CoverType = coverType,
            HasBeenRead = hasBeenRead,
            PublisherId = publisherId,
            CategoryIds = [categoryId],
            Works = [new SaveWorkDto { Title = title, Order = 1, AuthorIds = [authorId] }]
        });
    }

    [Fact]
    public async Task GetBooks_WithTypeFilter_ShouldReturnOnlyMatchingBooks()
    {
        // Arrange
        await AddBookAsync("Paper Book", BookType.Paper);
        await AddBookAsync("Digital Book", BookType.Ebook);

        // Act
        var result = await _bookService.GetBooks(new BookFilter { Type = BookType.Ebook });

        // Assert
        Assert.Equal(1, result.TotalCount);
        Assert.Equal("Digital Book", Assert.Single(result.Items).Title);
    }

    [Fact]
    public async Task GetBooks_WithCoverTypeFilter_ShouldReturnOnlyBooksWithCover()
    {
        // Arrange
        await AddBookAsync("Hardcover Book", coverType: BookCoverType.HardCover);
        await AddBookAsync("Softcover Book", coverType: BookCoverType.SoftCover);

        // Act
        var result = await _bookService.GetBooks(new BookFilter { CoverType = BookCoverType.SoftCover });

        // Assert
        Assert.Equal(1, result.TotalCount);
        Assert.Equal("Softcover Book", Assert.Single(result.Items).Title);
    }

    [Fact]
    public async Task GetBooks_WithHasBeenReadFilter_ShouldReturnOnlyMatchingBooks()
    {
        // Arrange
        await AddBookAsync("Read Book", hasBeenRead: true);
        await AddBookAsync("Unread Book", hasBeenRead: false);

        // Act
        var result = await _bookService.GetBooks(new BookFilter { HasBeenRead = true });

        // Assert
        Assert.Equal(1, result.TotalCount);
        Assert.Equal("Read Book", Assert.Single(result.Items).Title);
    }

    [Fact]
    public async Task GetBooks_WithCombinedFilter_ShouldApplyAllConditions()
    {
        // Arrange
        await AddBookAsync("Ebook Unread", BookType.Ebook, hasBeenRead: false);
        await AddBookAsync("Ebook Read", BookType.Ebook, hasBeenRead: true);
        await AddBookAsync("Paper Read", BookType.Paper, hasBeenRead: true);

        // Act
        var result = await _bookService.GetBooks(new BookFilter
        {
            Type = BookType.Ebook,
            HasBeenRead = true
        });

        // Assert
        Assert.Equal(1, result.TotalCount);
        Assert.Equal("Ebook Read", Assert.Single(result.Items).Title);
    }

    [Fact]
    public async Task GetBooks_WithNoFilter_ShouldReturnAll()
    {
        // Arrange
        await AddBookAsync("Book A");
        await AddBookAsync("Book B");
        await AddBookAsync("Book C");

        // Act
        var result = await _bookService.GetBooks();

        // Assert
        Assert.Equal(3, result.TotalCount);
        Assert.Equal(3, result.Items.Count);
    }

    [Fact]
    public async Task GetBooks_WithPagination_ShouldReturnRequestedPageWithTotalCount()
    {
        // Arrange
        for (var i = 1; i <= 15; i++)
            await AddBookAsync($"Book {i:00}");

        // Act
        var firstPage = await _bookService.GetBooks();
        var secondPage = await _bookService.GetBooks(page: 2);

        // Assert
        Assert.Equal(15, firstPage.TotalCount);
        Assert.Equal(12, firstPage.Items.Count);
        Assert.Equal("Book 01", firstPage.Items[0].Title);

        Assert.Equal(15, secondPage.TotalCount);
        Assert.Equal(3, secondPage.Items.Count);
        Assert.Equal("Book 13", secondPage.Items[0].Title);
        Assert.Equal("Book 15", secondPage.Items[^1].Title);
    }
}
