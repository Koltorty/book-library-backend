using BookLibrary.Data;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Tests.Infrastructure;

public class TestDbContextFactory(string databaseName) : IDbContextFactory<BookDbContext>
{
    public BookDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<BookDbContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;

        return new BookDbContext(options);
    }

    public async Task<BookDbContext> CreateDbContextAsync()
    {
        await Task.CompletedTask;
        return CreateDbContext();
    }
}
