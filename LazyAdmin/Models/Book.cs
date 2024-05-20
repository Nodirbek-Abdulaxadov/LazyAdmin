namespace LazyAdmin.Models;

public class Book 
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public BookType Type { get; set; }
    public int Pages { get; set; }
    public int Year { get; set; }
    public string Publisher { get; set; }
    public string ISBN { get; set; }
    public string Language { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public string File { get; set; }

}

public enum BookType
{
    Fiction,
    NonFiction
}