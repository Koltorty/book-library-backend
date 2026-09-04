using BookLibrary.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookLibrary.Data.Configurations;

public class SeriesConfiguration : IEntityTypeConfiguration<Series>
{
    public void Configure(EntityTypeBuilder<Series> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");
        
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("title");
        
        builder.Property(x => x.ParentSeriesId)
            .IsRequired(false)
            .HasColumnName("parent_series_id");

        builder.HasOne(x => x.ParentSeries)
            .WithMany(x => x.SubSeries)
            .HasForeignKey(x => x.ParentSeriesId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.Title)
            .IsUnique()
            .HasDatabaseName("ix_series_title");

        builder.HasIndex(x => x.ParentSeriesId)
            .HasDatabaseName("ix_series_parent_series_id");
    }
}