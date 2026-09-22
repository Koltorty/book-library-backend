using BookLibrary.Api.DTOs.AuthorDtos;

namespace BookLibrary.Api.DTOs.BookDtos;

public class WorkDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Order { get; set; }
    public IReadOnlyList<AuthorListItemDto> Authors { get; set; } = [];
}
