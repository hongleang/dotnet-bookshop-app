using BookStore.Models;
using BookStore.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Data.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Title).IsRequired().HasMaxLength(250);
        builder.Property(b => b.Price).HasPrecision(10, 2);
        builder.Property(b => b.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(b => b.Title);

        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_Books_Price", "\"Price\" >= 0");
            t.HasCheckConstraint("CK_Books_TotalCopies", "\"TotalCopies\" >= 0");
            t.HasCheckConstraint("CK_Books_AvailableCopies", "\"AvailableCopies\" >= 0 AND \"AvailableCopies\" <= \"TotalCopies\"");
        });

        builder.HasOne(b => b.BookDetail)
            .WithOne(d => d.Book)
            .HasForeignKey<BookDetail>(d => d.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(b => b.Categories)
            .WithMany(c => c.Books);

        // soft delete
        builder.HasQueryFilter(b => !b.IsDeleted);
    }
}
