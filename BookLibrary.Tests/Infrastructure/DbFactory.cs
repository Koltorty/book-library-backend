using BookLibrary.Data;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Tests.Infrastructure;

public class DbFactory : IDbContextFactory<BookDbContext>
{
    private readonly string _databaseName = Guid.NewGuid().ToString();

    public BookDbContext CreateDbContext()
    {
        return new BookDbContext(new DbContextOptionsBuilder<BookDbContext>()
            .UseInMemoryDatabase(_databaseName)
            .Options);
    }
}
