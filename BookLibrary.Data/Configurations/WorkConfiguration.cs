using BookLibrary.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookLibrary.Data.Configurations;

public class WorkConfiguration : IEntityTypeConfiguration<Work>
{
    public void Configure(EntityTypeBuilder<Work> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("title");

        builder.Property(x => x.Order)
            .IsRequired()
            .HasColumnName("work_order");

        builder.Property(x => x.BookId)
            .IsRequired()
            .HasColumnName("book_id");

        builder.Property(x => x.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnName("is_deleted");

        builder.HasOne(x => x.Book)
            .WithMany(x => x.Works)
            .HasForeignKey(x => x.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Authors)
            .WithMany(x => x.Works)
            .UsingEntity("Works_Authors");

        builder.HasIndex(x => x.BookId)
            .HasDatabaseName("ix_works_book_id");
    }
}
