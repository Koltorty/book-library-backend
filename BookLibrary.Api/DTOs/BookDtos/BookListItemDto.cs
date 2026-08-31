using BookLibrary.Data.Entities.Enums;

namespace BookLibrary.Api.DTOs.BookDtos;

public class BookListItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int? VolumeNumber { get; set; }
    public string Type { get; set; } = string.Empty;
    public IReadOnlyList<string> Authors { get; set; } = [];
    public bool HasBeenRead { get; set; }
    public string? CoverImage { get; set; }
}
