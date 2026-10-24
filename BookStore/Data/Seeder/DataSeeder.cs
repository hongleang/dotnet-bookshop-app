using Bogus;
using BookStore.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data.Seeder;

public class DataSeeder
{
    private readonly AppDbContext _db;

    public DataSeeder(AppDbContext db) => _db = db;

    public async Task SeedAsync(CancellationToken ct)
    {
        if (await _db.Authors.AnyAsync()) return;

        var authors = new Faker<Author>()
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.LastName, f => f.Name.LastName())
            .RuleFor(u => u.Bio, (f, u) => f.Lorem.Text())
            .RuleFor(u => u.DateOfBirth, f => DateOnly.FromDateTime(f.Date.Past(30, DateTime.Now.AddYears(-18))))
            .Generate(20);

        _db.Authors.AddRange(authors);
        await _db.SaveChangesAsync(ct);

        var detailFaker = new Faker<BookDetail>()
            .RuleFor(d => d.Isbn, f => f.Random.ReplaceNumbers("978##########"))
            .RuleFor(d => d.Pages, f => f.Random.Number(100, 800))
            .RuleFor(d => d.Publisher, f => f.Company.CompanyName())
            .RuleFor(d => d.Description, f => f.Lorem.Paragraph());

        var categoryNames = new[]
        {
            "Fantasy", "Science Fiction", "Mystery", "Romance", "Horror",
            "Biography", "History", "Science", "Technology", "Children"
        };

        var categories = categoryNames
            .Select(name => new Category
            {
                Name = name,
                Description = $"Books in the {name} category."
            })
            .ToList();

        var books = new Faker<Book>()
            .RuleFor(b => b.Title, f => f.Commerce.ProductName())
            .RuleFor(b => b.PublishedDate, f => f.Date.PastDateOnly(f.Random.Number(5, 10)))
            .RuleFor(b => b.Price, f => f.Finance.Amount(10, 20))
            .RuleFor(b => b.TotalCopies, f => f.Random.Number(1, 100))
            .RuleFor(b => b.AuthorId, f => f.PickRandom(authors).Id)
            .RuleFor(b => b.BookDetail, _ => detailFaker.Generate())
            .RuleFor(b => b.Categories, (f) => f.PickRandom(categories, f.Random.Int(1, 3)).ToList())
            .FinishWith((f, b) => b.AvailableCopies = b.TotalCopies)
            .Generate(20);

        _db.Books.AddRange(books);
        await _db.SaveChangesAsync(ct);

        var members = new Faker<Member>()
            .RuleFor(m => m.FirstName, f => f.Name.FirstName())
            .RuleFor(m => m.LastName, f => f.Name.LastName())
            .RuleFor(m => m.Email, f => f.Internet.Email())
            .RuleFor(m => m.Phone, f => f.Phone.PhoneNumber())
            .RuleFor(m => m.JoinedDate, f => DateOnly.FromDateTime(f.Date.Past(1, DateTime.Now)))
            .Generate(20);

        _db.Members.AddRange(members);
        await _db.SaveChangesAsync(ct);

        var today = DateOnly.FromDateTime(DateTime.Today);

        var loans = new Faker<Loan>()
            .RuleFor(l => l.BookId, f => f.PickRandom(books).Id)
            .RuleFor(l => l.MemberId, f => f.PickRandom(members).Id)
            .RuleFor(l => l.LoanDate, f => f.Date.PastDateOnly(1, today.AddDays(-1)))
            .RuleFor(l => l.DueDate, (f, l) => l.LoanDate.AddDays(14))
            .RuleFor(l => l.ReturnedDate, (f, l) =>
            {
                if (f.Random.Bool(0.3f)) return null;
                var returned = l.LoanDate.AddDays(f.Random.Number(1, 21));
                return returned <= today ? returned : null;
            })
            .Generate(20);

        _db.Loans.AddRange(loans);
        await _db.SaveChangesAsync(ct);

        var reviews = new Faker<Review>()
            .RuleFor(r => r.BookId, f => f.PickRandom(books).Id)
            .RuleFor(r => r.MemberId, f => f.PickRandom(members).Id)
            .RuleFor(r => r.Comment, f => f.Lorem.Paragraph())
            .RuleFor(r => r.Rating, f => Math.Round(f.Random.Decimal(1, 5) * 2, MidpointRounding.AwayFromZero) / 2)
            .Generate(20);
        
        _db.Reviews.AddRange(reviews);
        await _db.SaveChangesAsync(ct);
    }
}