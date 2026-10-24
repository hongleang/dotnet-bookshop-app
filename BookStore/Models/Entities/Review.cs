namespace BookStore.Models.Entities;

public class Review
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public Book Book { get; set; } = null!;
    
    public int MemberId { get; set; }
    public Member Member { get; set; } = null!;
    
    public int Rating { get; set; }
    public string? Comment { get; set; }
}