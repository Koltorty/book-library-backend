using BookLibrary.Data;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Tests.Infrastructure;

public class TestDbContext : IDisposable
{
    public string DatabaseName { get; } = Guid.NewGuid().ToString();

    public BookDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<BookDbContext>()
            .UseInMemoryDatabase(DatabaseName)
            .Options;

        return new BookDbContext(options);
    }

    public void Dispose()
    {
    }
}
