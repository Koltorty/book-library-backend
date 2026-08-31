using BookLibrary.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookLibrary.Data.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");
        
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("title");

        builder.Property(x => x.VolumeNumber)
            .IsRequired(false)
            .HasColumnName("volume_number");

        builder.Property(x => x.PagesCount)
            .IsRequired()
            .HasColumnName("pages_count");

        builder.Property(x => x.Type)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(10)
            .HasColumnName("type");
        
        builder.Property(x => x.CoverType)
            .HasConversion<string>()
            .HasMaxLength(10)
            .HasColumnName("cover_type");

        builder.Property(x => x.HasBeenRead)
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnName("has_been_read");
        
        builder.Property(x => x.DateRead)
            .IsRequired(false)
            .HasColumnName("date_read");

        builder.Property(x => x.CoverImage)
            .HasColumnName("cover_image");
        
        builder.Property(x => x.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnName("is_deleted");
        
        builder.Property(x => x.SeriesId)
            .IsRequired(false)
            .HasColumnName("series_id");

        builder.Property(x => x.PublisherId)
            .IsRequired()
            .HasColumnName("publisher_id");

        builder.HasOne(x => x.Series)
            .WithMany(x => x.Books)
            .HasForeignKey(x => x.SeriesId)
            .IsRequired(false);

        builder.HasOne(x => x.Publisher)
            .WithMany(x => x.Books)
            .HasForeignKey(x => x.PublisherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Categories)
            .WithMany(x => x.Books)
            .UsingEntity("Books_Categories");

        builder.HasIndex(x => new { x.Title, x.VolumeNumber })
            .IsUnique()
            .HasDatabaseName("ix_books_title_volume_number");

        builder.HasIndex(x => x.Type)
            .HasDatabaseName("ix_books_type");

        builder.HasIndex(x => x.CoverType)
            .HasDatabaseName("ix_books_cover_type");
    }
}