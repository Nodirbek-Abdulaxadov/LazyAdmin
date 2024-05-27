namespace LazyAdmin.Models;

public class Test
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Number { get; set; }
    public int BookId { get; set; }
    public Book Book { get; set; } = new();
}