namespace BookStore.Data.DTOs;

public record LoanDto
{
    public required int Id { get; init; }
    public required int BookId { get; init; }
    public required int MemberId { get; init; }
    public required DateOnly LoanDate { get; init; }
    public required DateOnly DueDate { get; init; }
    public DateOnly? ReturnDate { get; init; }
}
