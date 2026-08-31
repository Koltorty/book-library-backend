using BookLibrary.Api.DTOs.BookDtos;

namespace BookLibrary.Api.DTOs.CategoryDtos;

public class CategoryDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public IReadOnlyList<BookListItemDto> Books { get; set; } = [];
}
