using BookStore.Models;
using BookStore.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Data.Configurations;

public class LoanConfiguration : IEntityTypeConfiguration<Loan>
{
    public void Configure(EntityTypeBuilder<Loan> builder)
    {
        builder.HasKey(l => l.Id);

        builder.HasOne(l => l.Book)
            .WithMany(b => b.Loans)
            .HasForeignKey(l => l.BookId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(l => l.Member)
            .WithMany(m => m.Loans)
            .HasForeignKey(l => l.MemberId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_Loans_LoanDate", "\"LoanDate\" <= \"DueDate\"");
            t.HasCheckConstraint("CK_Loans_ReturnedDate", "\"ReturnedDate\" IS NULL OR \"ReturnedDate\" >= \"LoanDate\"");
        });
        
        builder.HasQueryFilter(l => l.Book.IsDeleted == false);
    }
}
