namespace BookStore.Models.Entities;

public class Member
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    public DateOnly JoinedDate { get; set; }
    public ICollection<Loan> Loans { get; set; }
    public ICollection<Review> Reviews { get; set; }
}