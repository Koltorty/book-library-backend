using BookLibrary.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookLibrary.Data.Configurations;

public class PublisherConfiguration : IEntityTypeConfiguration<Publisher>
{
    public void Configure(EntityTypeBuilder<Publisher> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("name");

        builder.HasIndex(x => x.Name)
            .IsUnique()
            .HasDatabaseName("ix_publishers_name");

        builder.HasData(
            new Publisher { Id = 1,  Name = "АСТ" },
            new Publisher { Id = 2,  Name = "Эксмо" },
            new Publisher { Id = 3,  Name = "МИФ" },
            new Publisher { Id = 4,  Name = "Альпина Паблишер" },
            new Publisher { Id = 5,  Name = "Альпина Нон-фикшн" },
            new Publisher { Id = 6,  Name = "Азбука" },
            new Publisher { Id = 7,  Name = "Corpus" },
            new Publisher { Id = 8,  Name = "Ad Marginem" },
            new Publisher { Id = 9,  Name = "Livebook" },
            new Publisher { Id = 10, Name = "Individuum" },
            new Publisher { Id = 11, Name = "Питер" },
            new Publisher { Id = 12, Name = "ДМК Пресс" },
            new Publisher { Id = 13, Name = "O'Reilly Media" },
            new Publisher { Id = 14, Name = "Manning Publications" },
            new Publisher { Id = 15, Name = "No Starch Press" },
            new Publisher { Id = 16, Name = "Addison-Wesley" },
            new Publisher { Id = 17, Name = "Pearson" },
            new Publisher { Id = 18, Name = "Apress" }
        );
    }
}