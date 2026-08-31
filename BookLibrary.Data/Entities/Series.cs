namespace BookLibrary.Data.Entities;

public class Series
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }

    public int? ParentSeriesId { get; set; }
    public Series? ParentSeries { get; set; }
    public ICollection<Series> SubSeries { get; set; } = new HashSet<Series>();

    public ICollection<Book> Books { get; set; } = new HashSet<Book>();
}