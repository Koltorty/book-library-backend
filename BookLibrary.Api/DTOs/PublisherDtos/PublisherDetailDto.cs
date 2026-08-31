using BookLibrary.Api.DTOs.BookDtos;

namespace BookLibrary.Api.DTOs.PublisherDtos;

public class PublisherDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public IReadOnlyList<BookListItemDto> Books { get; set; } = [];
}
