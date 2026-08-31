using BookLibrary.Api.DTOs.BookDtos;

namespace BookLibrary.Api.DTOs.SeriesDtos;

public class SeriesDetailDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int? ParentSeriesId { get; set; }
    public string? ParentSeriesTitle { get; set; }
    public IReadOnlyList<SeriesListItemDto> SubSeries { get; set; } = [];
    public IReadOnlyList<BookListItemDto> Books { get; set; } = [];
}
