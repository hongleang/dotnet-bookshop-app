using BookStore.Models;
using BookStore.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<BookDetail> BookDetails { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}