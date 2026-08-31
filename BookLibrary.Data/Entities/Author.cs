namespace BookLibrary.Data.Entities;

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<Work> Works { get; set; } = new HashSet<Work>();
}