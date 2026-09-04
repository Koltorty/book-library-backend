using BookLibrary.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookLibrary.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
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
            .HasDatabaseName("ix_categories_name");

        builder.HasData(
            // Нон-фикшн
            new Category { Id = 1,  Name = "Биографии и мемуары" },
            new Category { Id = 2,  Name = "Биология" },
            new Category { Id = 3,  Name = "Культура и искусство" },
            new Category { Id = 4,  Name = "Лженаука" },
            new Category { Id = 5,  Name = "Математика" },
            new Category { Id = 6,  Name = "Медицина и биология человека" },
            new Category { Id = 7,  Name = "Политика и история" },
            new Category { Id = 8,  Name = "Поп-культура и миры" },
            new Category { Id = 9,  Name = "Саморазвитие и коммуникация" },
            new Category { Id = 10, Name = "Спорт" },
            new Category { Id = 11, Name = "Тру-крайм" },
            new Category { Id = 12, Name = "Физика" },
            new Category { Id = 13, Name = "Фольклор и мифология" },
            new Category { Id = 14, Name = "Экономика и статистика" },
            // Программистское
            new Category { Id = 15, Name = "Алгоритмы" },
            new Category { Id = 16, Name = "Архитектура компьютера" },
            new Category { Id = 17, Name = "Операционные системы" },
            new Category { Id = 18, Name = "Компьютерные сети" },
            new Category { Id = 19, Name = "Computer Science (общее)" },
            new Category { Id = 20, Name = "Базы данных" },
            new Category { Id = 21, Name = "Компьютерная безопасность" },
            new Category { Id = 22, Name = "Инструменты" },
            new Category { Id = 23, Name = "Проектирование и архитектура" },
            new Category { Id = 24, Name = "Микросервисы" },
            new Category { Id = 25, Name = "ООП" },
            new Category { Id = 26, Name = "Паттерны проектирования" },
            new Category { Id = 27, Name = "Рефакторинг" },
            new Category { Id = 28, Name = "API" },
            new Category { Id = 29, Name = "System Design" },
            new Category { Id = 30, Name = "DDD и моделирование данных" },
            new Category { Id = 31, Name = "Саморазвитие в IT" },
            new Category { Id = 32, Name = "Тестирование" },
            new Category { Id = 33, Name = "C# и .NET" },
            new Category { Id = 34, Name = "Frontend" },
            new Category { Id = 35, Name = "Go" },
            new Category { Id = 36, Name = "TypeScript" },
            // Художественное
            new Category { Id = 37, Name = "Художественная литература" }
        );
    }
}