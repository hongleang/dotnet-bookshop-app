using BookStore.Models;
using BookStore.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Data.Configurations;

public class BookDetailConfiguration : IEntityTypeConfiguration<BookDetail>
{
    public void Configure(EntityTypeBuilder<BookDetail> builder)
    {
        builder.HasKey(d => d.Id);

        builder.HasIndex(d => d.Isbn).IsUnique();
        builder.Property(d => d.Isbn).IsRequired().HasMaxLength(20);
        builder.Property(d => d.Publisher).IsRequired().HasMaxLength(100);
        builder.Property(d => d.Language).HasMaxLength(100);
        builder.Property(d => d.Description).HasMaxLength(1000);
        
        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_BookDetails_Pages", "\"Pages\" > 0");
        });
        
        builder.HasQueryFilter(d => d.Book.IsDeleted == false);
    }
}
