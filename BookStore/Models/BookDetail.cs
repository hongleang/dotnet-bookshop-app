namespace BookStore.Models;

public class BookDetail
{
    public int Id { get; set; }
    public string Isbn { get; set; }
    public int Pages { get; set; }
    public string Publisher { get; set; }
    public string? Language { get; set; }
    public string? Description { get; set; }
    public int BookId { get; set; }
    public Book Book { get; set; }
}