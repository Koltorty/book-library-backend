using BookLibrary.Api.DTOs.BookDtos;
using BookLibrary.Api.DTOs.PublisherDtos;
using BookLibrary.Data;
using BookLibrary.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Api.Services;

public class PublisherService(IDbContextFactory<BookDbContext> factory)
{
    public async Task<IReadOnlyList<PublisherListItemDto>> GetPublishers()
    {
        await using var db = await factory.CreateDbContextAsync();

        var publishers = await db.Publishers
            .Where(p => p.Books.Any())
            .OrderBy(p => p.Name)
            .Select(p => new PublisherListItemDto
            {
                Id = p.Id,
                Name = p.Name
            })
            .ToListAsync();

        return publishers;
    }

    public async Task<PublisherDetailDto?> GetPublisher(int id)
    {
        await using var db = await factory.CreateDbContextAsync();

        var publisher = await db.Publishers
            .AsNoTracking()
            .Include(p => p.Books)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (publisher is null)
            return null;

        var books = publisher.Books
            .OrderBy(b => b.Title)
            .Select(b => new BookListItemDto
            {
                Id = b.Id,
                Title = b.Title,
                VolumeNumber = b.VolumeNumber,
                CoverImage = b.CoverImage
            })
            .ToList();

        var result = new PublisherDetailDto
        {
            Id = publisher.Id,
            Name = publisher.Name,
            Books = books
        };

        return result;
    }

    public async Task<int> AddPublisher(string name)
    {
        await using var db = await factory.CreateDbContextAsync();

        var existing = await db.Publishers.FirstOrDefaultAsync(p => p.Name == name);

        if (existing is not null)
            return existing.Id;

        var publisher = new Publisher { Name = name };
        db.Publishers.Add(publisher);
        await db.SaveChangesAsync();

        return publisher.Id;
    }

    public async Task<bool> UpdatePublisher(int id, string name)
    {
        await using var db = await factory.CreateDbContextAsync();

        var publisher = await db.Publishers.FindAsync(id);

        if (publisher is null)
            return false;

        publisher.Name = name;
        await db.SaveChangesAsync();

        return true;
    }
}
