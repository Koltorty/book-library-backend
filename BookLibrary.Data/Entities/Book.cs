using BookLibrary.Data.Entities.Enums;

namespace BookLibrary.Data.Entities;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int? VolumeNumber { get; set; }
    public int PagesCount { get; set; }
    public BookType Type { get; set; }
    public BookCoverType? CoverType { get; set; } // null for Ebooks
    public bool HasBeenRead { get; set; }
    public DateOnly? DateRead { get; set; }
    public string? CoverImage { get; set; }
    public bool IsDeleted { get; set; }

    // Navigation Properties Keys
    public int? SeriesId { get; set; }
    public int PublisherId { get; set; }

    // Navigation Properties
    public ICollection<Work> Works { get; set; } = new HashSet<Work>();
    public ICollection<Category> Categories { get; set; } = new HashSet<Category>();
    public Series? Series { get; set; }
    public Publisher Publisher { get; set; } = null!;
}