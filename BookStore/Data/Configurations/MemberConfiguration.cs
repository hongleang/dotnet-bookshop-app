using BookStore.Models;
using BookStore.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Data.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(m => m.LastName).IsRequired().HasMaxLength(100);
        
        builder.HasIndex(m => m.Email).IsUnique();
        builder.Property(m => m.Email).IsRequired().HasMaxLength(254);
        builder.Property(m => m.Phone).HasMaxLength(50);
    }
}
