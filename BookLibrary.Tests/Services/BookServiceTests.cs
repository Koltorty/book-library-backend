using BookLibrary.Api.DTOs.BookDtos;
using BookLibrary.Api.Services;
using BookLibrary.Data;
using BookLibrary.Data.Entities;
using BookLibrary.Data.Entities.Enums;
using BookLibrary.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Tests.Services;

public class BookServiceTests
{
    private static BookService CreateService(string databaseName)
    {
        var factory = new TestDbContextFactory(databaseName);
        return new BookService(factory);
    }

    [Fact]
    public async Task AddBook_CreatesBookWithWorks()
    {
        using var testDb = new TestDbContext();
        var service = CreateService(testDb.DatabaseName);

        var author = new Author { Name = "Test Author" };
        var category = new Category { Name = "Test Category" };
        var publisher = new Publisher { Name = "Test Publisher" };

        await using (var seed = testDb.CreateContext())
        {
            seed.Authors.Add(author);
            seed.Categories.Add(category);
            seed.Publishers.Add(publisher);
            await seed.SaveChangesAsync();
        }

        var dto = new BookSaveDto
        {
            Title = "Test Book",
            PagesCount = 100,
            Type = BookType.Paper,
            PublisherId = publisher.Id,
            CategoryIds = [category.Id],
            Works =
            [
                new SaveWorkDto
                {
                    Title = "Work 1",
                    Order = 1,
                    AuthorIds = [author.Id]
                }
            ]
        };

        var bookId = await service.AddBook(dto);

        await using var verify = testDb.CreateContext();
        var book = await verify.Books
            .Include(b => b.Works).ThenInclude(w => w.Authors)
            .Include(b => b.Categories)
            .FirstAsync(b => b.Id == bookId);

        Assert.Equal("Test Book", book.Title);
        Assert.Single(book.Works);
        Assert.Single(book.Categories);
        Assert.Single(book.Works.First().Authors);
    }

    [Fact]
    public async Task GetBook_ReturnsNull_WhenNotFound()
    {
        using var testDb = new TestDbContext();
        var service = CreateService(testDb.DatabaseName);

        var result = await service.GetBook(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetBook_ReturnsBookWithDetails()
    {
        using var testDb = new TestDbContext();
        var service = CreateService(testDb.DatabaseName);

        var author = new Author { Name = "Author" };
        var category = new Category { Name = "Category" };
        var publisher = new Publisher { Name = "Publisher" };
        var book = new Book
        {
            Title = "Book",
            PagesCount = 200,
            Type = BookType.Ebook,
            PublisherId = 1
        };
        book.Works.Add(new Work { Title = "Work", Order = 1, Book = book });
        book.Works.First().Authors.Add(author);
        book.Categories.Add(category);

        await using (var seed = testDb.CreateContext())
        {
            seed.Publishers.Add(publisher);
            seed.Authors.Add(author);
            seed.Categories.Add(category);
            seed.Books.Add(book);
            await seed.SaveChangesAsync();
        }

        var result = await service.GetBook(book.Id);

        Assert.NotNull(result);
        Assert.Equal("Book", result.Title);
        Assert.Single(result.Works);
        Assert.Single(result.Categories);
    }

    [Fact]
    public async Task DeleteBook_SetsIsDeleted()
    {
        using var testDb = new TestDbContext();
        var service = CreateService(testDb.DatabaseName);

        var publisher = new Publisher { Name = "P" };
        var book = new Book
        {
            Title = "To Delete",
            PagesCount = 1,
            Type = BookType.Paper,
            PublisherId = 1
        };
        var work = new Work { Title = "W", Order = 1, Book = book };
        book.Works.Add(work);

        await using (var seed = testDb.CreateContext())
        {
            seed.Publishers.Add(publisher);
            seed.Books.Add(book);
            await seed.SaveChangesAsync();
        }

        var result = await service.DeleteBook(book.Id);

        Assert.True(result);

        await using (var verify = testDb.CreateContext())
        {
            var deletedBook = await verify.Books.IgnoreQueryFilters()
                .FirstAsync(b => b.Id == book.Id);
            Assert.True(deletedBook.IsDeleted);

            var deletedWork = await verify.Works.IgnoreQueryFilters()
                .FirstAsync(w => w.Id == work.Id);
            Assert.True(deletedWork.IsDeleted);
        }
    }
}
