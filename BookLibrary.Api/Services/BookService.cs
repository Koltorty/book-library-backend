using BookLibrary.Api.DTOs.AuthorDtos;
using BookLibrary.Api.DTOs.BookDtos;
using BookLibrary.Api.DTOs.CategoryDtos;
using BookLibrary.Api.DTOs.Common;
using BookLibrary.Data;
using BookLibrary.Data.Entities;
using BookLibrary.Data.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Api.Services;

public class BookService(IDbContextFactory<BookDbContext> factory)
{
    private const int DefaultPageSize = 12;

    public async Task<PagedResult<BookListItemDto>> GetBooks(BookFilter? filter = null, int page = 1, int pageSize = DefaultPageSize)
    {
        await using var db = await factory.CreateDbContextAsync();

        var query = db.Books
            .Include(b => b.Works)
                .ThenInclude(w => w.Authors)
            .AsQueryable();

        if (filter is not null)
        {
            if (filter.Type.HasValue)
                query = query.Where(b => b.Type == filter.Type.Value);

            if (filter.CoverType.HasValue)
                query = query.Where(b => b.CoverType == filter.CoverType.Value);

            if (filter.HasBeenRead.HasValue)
                query = query.Where(b => b.HasBeenRead == filter.HasBeenRead.Value);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(b => b.Title)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new BookListItemDto
            {
                Id = b.Id,
                Title = b.Title,
                VolumeNumber = b.VolumeNumber,
                Type = b.Type.ToString(),
                Authors = b.Works
                    .Where(w => !w.IsDeleted)
                    .SelectMany(w => w.Authors)
                    .Distinct()
                    .Select(a => a.Name)
                    .ToList(),
                HasBeenRead = b.HasBeenRead,
                CoverImage = b.CoverImage
            })
            .ToListAsync();

        var result = new PagedResult<BookListItemDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };

        return result;
    }

    public async Task<BookDetailDto?> GetBook(int id)
    {
        await using var db = await factory.CreateDbContextAsync();

        var book = await db.Books
            .AsNoTracking()
            .AsSplitQuery()
            .Include(b => b.Works).ThenInclude(w => w.Authors)
            .Include(b => b.Categories)
            .Include(b => b.Series)
            .Include(b => b.Publisher)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book is null) 
            return null;

        var result = new BookDetailDto
        {
            Id = book.Id,
            Title = book.Title,
            VolumeNumber = book.VolumeNumber,
            PagesCount = book.PagesCount,
            Type = book.Type.ToString(),
            CoverType = book.CoverType?.ToString(),
            HasBeenRead = book.HasBeenRead,
            DateRead = book.DateRead,
            CoverImage = book.CoverImage,
            SeriesId = book.SeriesId,
            SeriesTitle = book.Series?.Title,
            PublisherId = book.PublisherId,
            PublisherName = book.Publisher.Name,
            Works = book.Works
                .OrderBy(w => w.Order)
                .Select(w => new WorkDto
                {
                    Id = w.Id,
                    Title = w.Title,
                    Order = w.Order,
                    Authors = w.Authors.Select(a => a.Name).ToList()
                })
                .ToList(),
            Categories = book.Categories
                .Select(c => new CategoryDto { Id = c.Id, Name = c.Name })
                .ToList()
        };
        
        return result;
    }

    public async Task<int> AddBook(BookSaveDto dto)
    {
        await using var db = await factory.CreateDbContextAsync();

        var book = new Book
        {
            Title = dto.Title,
            VolumeNumber = dto.VolumeNumber,
            PagesCount = dto.PagesCount,
            Type = dto.Type,
            CoverType = dto.CoverType,
            HasBeenRead = dto.HasBeenRead,
            DateRead = dto.DateRead,
            CoverImage = dto.CoverImage,
            SeriesId = dto.SeriesId,
            PublisherId = dto.PublisherId
        };

        db.Books.Add(book);

        foreach (var workDto in dto.Works)
        {
            var work = new Work
            {
                Title = workDto.Title,
                Order = workDto.Order,
                Book = book
            };

            if (workDto.AuthorIds.Count > 0)
            {
                var authors = await db.Authors
                    .Where(a => workDto.AuthorIds.Contains(a.Id))
                    .ToListAsync();

                foreach (var author in authors)
                    work.Authors.Add(author);
            }

            db.Works.Add(work);
        }

        if (dto.CategoryIds.Count > 0)
        {
            var categories = await db.Categories
                .Where(c => dto.CategoryIds.Contains(c.Id))
                .ToListAsync();

            foreach (var category in categories)
                book.Categories.Add(category);
        }

        await db.SaveChangesAsync();
        return book.Id;
    }

    public async Task<bool> UpdateBook(int id, BookSaveDto dto)
    {
        await using var db = await factory.CreateDbContextAsync();

        var book = await db.Books
            .Include(b => b.Works)
                .ThenInclude(w => w.Authors)
            .Include(b => b.Categories)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book is null)
            return false;

        book.Title = dto.Title;
        book.VolumeNumber = dto.VolumeNumber;
        book.PagesCount = dto.PagesCount;
        book.Type = dto.Type;
        book.CoverType = dto.CoverType;
        book.HasBeenRead = dto.HasBeenRead;
        book.DateRead = dto.DateRead;
        book.CoverImage = dto.CoverImage;
        book.SeriesId = dto.SeriesId;
        book.PublisherId = dto.PublisherId;

        var incomingWorkIds = dto.Works
            .Where(w => w.Id.HasValue)
            .Select(w => w.Id!.Value)
            .ToHashSet();

        foreach (var existingWork in book.Works.Where(w => !w.IsDeleted))
        {
            if (!incomingWorkIds.Contains(existingWork.Id))
                existingWork.IsDeleted = true;
        }

        foreach (var workDto in dto.Works)
        {
            var work = workDto.Id.HasValue
                ? book.Works.FirstOrDefault(w => w.Id == workDto.Id.Value && !w.IsDeleted)
                : null;

            if (work is not null)
            {
                work.Title = workDto.Title;
                work.Order = workDto.Order;
                work.Authors.Clear();
            }
            else
            {
                work = new Work
                {
                    Title = workDto.Title,
                    Order = workDto.Order,
                    Book = book
                };
                db.Works.Add(work);
            }

            if (workDto.AuthorIds.Count > 0)
            {
                var authors = await db.Authors
                    .Where(a => workDto.AuthorIds.Contains(a.Id))
                    .ToListAsync();

                foreach (var author in authors)
                    work.Authors.Add(author);
            }
        }

        book.Categories.Clear();
        if (dto.CategoryIds.Count > 0)
        {
            var categories = await db.Categories
                .Where(c => dto.CategoryIds.Contains(c.Id))
                .ToListAsync();

            foreach (var category in categories)
                book.Categories.Add(category);
        }

        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteBook(int id)
    {
        await using var db = await factory.CreateDbContextAsync();

        var book = await db.Books
            .Include(b => b.Works)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book is null) return false;

        book.IsDeleted = true;

        foreach (var work in book.Works)
            work.IsDeleted = true;

        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RestoreBook(int id)
    {
        await using var db = await factory.CreateDbContextAsync();

        var book = await db.Books
            .IgnoreQueryFilters()
            .Include(b => b.Works)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book is null) return false;

        book.IsDeleted = false;

        foreach (var work in book.Works)
            work.IsDeleted = false;

        await db.SaveChangesAsync();
        return true;
    }
}
