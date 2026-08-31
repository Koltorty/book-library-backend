using BookLibrary.Api.DTOs.CategoryDtos;

namespace BookLibrary.Api.DTOs.BookDtos;

public class BookDetailDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int? VolumeNumber { get; set; }
    public int PagesCount { get; set; }
    public string Type { get; set; } = string.Empty;
    public string? CoverType { get; set; }
    public bool HasBeenRead { get; set; }
    public DateOnly? DateRead { get; set; }
    public string? CoverImage { get; set; }

    public string? SeriesTitle { get; set; }
    public int? SeriesId { get; set; }
    public string PublisherName { get; set; } = string.Empty;
    public int PublisherId { get; set; }

    public IReadOnlyList<WorkDto> Works { get; set; } = [];
    public IReadOnlyList<CategoryDto> Categories { get; set; } = [];
}
