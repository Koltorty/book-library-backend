using BookLibrary.Api.DTOs.BookDtos;
using BookLibrary.Api.DTOs.SeriesDtos;
using BookLibrary.Data;
using BookLibrary.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Api.Services;

public class SeriesService(IDbContextFactory<BookDbContext> factory)
{
    public async Task<IReadOnlyList<SeriesListItemDto>> GetSeries()
    {
        await using var db = await factory.CreateDbContextAsync();

        var roots = await db.Series
            .AsNoTracking()
            .AsSplitQuery()
            .Where(s => s.ParentSeriesId == null && s.Books.Any())
            .OrderBy(s => s.Title)
            .Select(s => new SeriesListItemDto
            {
                Id = s.Id,
                Title = s.Title,
                SubSeries = s.SubSeries
                    .Where(ss => ss.Books.Any())
                    .OrderBy(ss => ss.Title)
                    .Select(ss => new SeriesListItemDto
                    {
                        Id = ss.Id,
                        Title = ss.Title
                    })
                    .ToList()
            })
            .ToListAsync();

        return roots;
    }

    public async Task<IReadOnlyList<SeriesListItemDto>> GetAllSeries(bool onlyActive)
    {
        await using var db = await factory.CreateDbContextAsync();

        var query = db.Series.AsNoTracking();

        if (onlyActive)
            query = query.Where(s => s.Books.Any());

        var all = await query
            .Select(s => new SeriesListItemDto
            {
                Id = s.Id,
                Title = s.Title
            })
            .OrderBy(s => s.Title)
            .ToListAsync();

        return all;
    }

    public async Task<SeriesDetailDto?> GetSeries(int id)
    {
        await using var db = await factory.CreateDbContextAsync();

        var series = await db.Series
            .AsNoTracking()
            .AsSplitQuery()
            .Include(s => s.ParentSeries)
            .Include(s => s.SubSeries)
            .Include(s => s.Books)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (series is null)
            return null;

        var books = series.Books
            .OrderBy(b => b.Title)
            .Select(b => new BookListItemDto
            {
                Id = b.Id,
                Title = b.Title,
                VolumeNumber = b.VolumeNumber,
                CoverImage = b.CoverImage
            })
            .ToList();

        var subSeries = series.SubSeries
            .OrderBy(ss => ss.Title)
            .Select(ss => new SeriesListItemDto
            {
                Id = ss.Id,
                Title = ss.Title
            })
            .ToList();

        var result = new SeriesDetailDto
        {
            Id = series.Id,
            Title = series.Title,
            ParentSeriesId = series.ParentSeriesId,
            ParentSeriesTitle = series.ParentSeries?.Title,
            SubSeries = subSeries,
            Books = books
        };

        return result;
    }

    public async Task<int> AddSeries(SeriesCreateDto dto)
    {
        await using var db = await factory.CreateDbContextAsync();

        var existing = await db.Series.FirstOrDefaultAsync(s => s.Title == dto.Title);

        if (existing is not null)
            return existing.Id;

        var series = new Series
        {
            Title = dto.Title,
            ParentSeriesId = dto.ParentSeriesId
        };

        db.Series.Add(series);
        await db.SaveChangesAsync();

        return series.Id;
    }

    public async Task<bool> UpdateSeries(int id, SeriesUpdateDto dto)
    {
        await using var db = await factory.CreateDbContextAsync();

        var series = await db.Series.FirstOrDefaultAsync(s => s.Id == id);

        if (series is null)
            return false;

        series.Title = dto.Title;
        series.ParentSeriesId = dto.ParentSeriesId;

        await db.SaveChangesAsync();

        return true;
    }
}
