using BookLibrary.Api.DTOs.BookDtos;
using BookLibrary.Api.DTOs.CategoryDtos;
using BookLibrary.Data;
using BookLibrary.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Api.Services;

public class CategoryService(IDbContextFactory<BookDbContext> factory)
{
    public async Task<IReadOnlyList<CategoryDto>> GetCategories()
    {
        await using var db = await factory.CreateDbContextAsync();

        var categories = await db.Categories
            .Where(c => c.Books.Any())
            .OrderBy(c => c.Name)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToListAsync();

        return categories;
    }

    public async Task<CategoryDetailDto?> GetCategory(int id)
    {
        await using var db = await factory.CreateDbContextAsync();

        var category = await db.Categories
            .AsNoTracking()
            .Include(c => c.Books)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category is null)
            return null;

        var books = category.Books
            .OrderBy(b => b.Title)
            .Select(b => new BookListItemDto
            {
                Id = b.Id,
                Title = b.Title,
                VolumeNumber = b.VolumeNumber,
                CoverImage = b.CoverImage
            })
            .ToList();

        var result = new CategoryDetailDto
        {
            Id = category.Id,
            Name = category.Name,
            Books = books
        };

        return result;
    }

    public async Task<int> AddCategory(string name)
    {
        await using var db = await factory.CreateDbContextAsync();

        var existing = await db.Categories.FirstOrDefaultAsync(c => c.Name == name);

        if (existing is not null)
            return existing.Id;

        var category = new Category { Name = name };
        db.Categories.Add(category);
        await db.SaveChangesAsync();

        return category.Id;
    }

    public async Task<bool> UpdateCategory(int id, string name)
    {
        await using var db = await factory.CreateDbContextAsync();

        var category = await db.Categories.FindAsync(id);

        if (category is null)
            return false;

        category.Name = name;
        await db.SaveChangesAsync();

        return true;
    }
}
