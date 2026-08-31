using BookLibrary.Api.DTOs.BookDtos;

namespace BookLibrary.Api.DTOs.AuthorDtos;

public class AuthorDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int BookCount { get; set; }
    public IReadOnlyList<BookListItemDto> Books { get; set; } = [];
}
