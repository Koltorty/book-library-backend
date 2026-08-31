using BookLibrary.Data.Entities.Enums;

namespace BookLibrary.Api.DTOs.Common;

public class BookFilter
{
    public BookType? Type { get; set; }
    public BookCoverType? CoverType { get; set; }
    public bool? HasBeenRead { get; set; }
}
