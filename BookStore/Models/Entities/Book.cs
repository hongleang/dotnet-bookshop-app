namespace BookStore.Models.Entities;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateOnly PublishedDate { get; set; }
    public decimal Price { get; set; }
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }
    
    public int AuthorId { get; set; }
    public Author Author { get; set; }
    
    public BookDetail BookDetail { get; set; }
    
    public ICollection<Category> Categories { get; set; }
    
    public ICollection<Loan>  Loans { get; set; }
    
    public ICollection<Review> Reviews { get; set; }
    
    public bool IsDeleted  {get; set;}
}