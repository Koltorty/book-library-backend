using BookLibrary.Data.Entities.Enums;

namespace BookLibrary.Api.DTOs.BookDtos;

public class BookSaveDto
{
    public string Title { get; set; } = string.Empty;
    public int? VolumeNumber { get; set; }
    public int PagesCount { get; set; }
    public BookType Type { get; set; }
    public BookCoverType? CoverType { get; set; }
    public bool HasBeenRead { get; set; }
    public DateOnly? DateRead { get; set; }
    public string? CoverImage { get; set; }
    public int? SeriesId { get; set; }
    public int PublisherId { get; set; }
    public IReadOnlyList<int> CategoryIds { get; set; } = [];
    public IReadOnlyList<SaveWorkDto> Works { get; set; } = [];
}

public class SaveWorkDto
{
    public int? Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Order { get; set; }
    public IReadOnlyList<int> AuthorIds { get; set; } = [];
}
