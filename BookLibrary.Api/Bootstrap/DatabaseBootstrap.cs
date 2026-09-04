using BookLibrary.Data;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Api.Bootstrap;

public static class DatabaseBootstrap
{
    public static void ConfigureDatabase(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContextFactory<BookDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsAssembly(typeof(BookDbContext).Assembly.GetName().Name);
            });
        });
    }

    public static void MigrateDatabase(this WebApplication app)
    {
        var factory = app.Services.GetRequiredService<IDbContextFactory<BookDbContext>>();

        using var db = factory.CreateDbContext();
        db.Database.Migrate();
    }
}