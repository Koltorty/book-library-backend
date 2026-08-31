namespace BookLibrary.Data.Entities;

public class Work
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Order { get; set; }
    public bool IsDeleted { get; set; }

    public int BookId { get; set; }
    public Book Book { get; set; } = null!;

    public ICollection<Author> Authors { get; set; } = new HashSet<Author>();
}
