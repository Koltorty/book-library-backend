using BookLibrary.Api.DTOs.AuthorDtos;
using BookLibrary.Api.DTOs.BookDtos;
using BookLibrary.Data;
using BookLibrary.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Api.Services;

public class AuthorService(IDbContextFactory<BookDbContext> factory)
{
    public async Task<IReadOnlyList<AuthorListItemDto>> GetAuthors(bool onlyActive)
    {
        await using var db = await factory.CreateDbContextAsync();

        var query = db.Authors.AsNoTracking();

        if (onlyActive)
            query = query.Where(a => a.Works.Any());

        var authors = await query
            .OrderBy(a => a.Name)
            .Select(a => new AuthorListItemDto
            {
                Id = a.Id,
                Name = a.Name
            })
            .ToListAsync();

        return authors;
    }

    public async Task<AuthorDetailDto?> GetAuthor(int id)
    {
        await using var db = await factory.CreateDbContextAsync();

        var author = await db.Authors
            .AsNoTracking()
            .Include(a => a.Works)
                .ThenInclude(w => w.Book)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (author is null)
            return null;

        var books = author.Works
            .Select(w => w.Book)
            .DistinctBy(b => b.Id)
            .OrderBy(b => b.Title)
            .Select(b => new BookListItemDto
            {
                Id = b.Id,
                Title = b.Title,
                VolumeNumber = b.VolumeNumber,
                CoverImage = b.CoverImage
            })
            .ToList();

        var result = new AuthorDetailDto
        {
            Id = author.Id,
            Name = author.Name,
            BookCount = books.Count,
            Books = books
        };

        return result;
    }

    public async Task<int> AddAuthor(string name)
    {
        await using var db = await factory.CreateDbContextAsync();

        var existing = await db.Authors.FirstOrDefaultAsync(a => a.Name == name);

        if (existing is not null)
            return existing.Id;

        var author = new Author { Name = name };
        db.Authors.Add(author);
        await db.SaveChangesAsync();

        return author.Id;
    }

    public async Task<bool> UpdateAuthor(int id, string name)
    {
        await using var db = await factory.CreateDbContextAsync();

        var author = await db.Authors.FindAsync(id);

        if (author is null)
            return false;

        author.Name = name;
        await db.SaveChangesAsync();

        return true;
    }
}