using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookLibrary.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Publishers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publishers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Series",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    parent_series_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Series", x => x.id);
                    table.ForeignKey(
                        name: "FK_Series_Series_parent_series_id",
                        column: x => x.parent_series_id,
                        principalTable: "Series",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    volume_number = table.Column<int>(type: "integer", nullable: true),
                    pages_count = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    cover_type = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    has_been_read = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    date_read = table.Column<DateOnly>(type: "date", nullable: true),
                    cover_image = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    series_id = table.Column<int>(type: "integer", nullable: true),
                    publisher_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.id);
                    table.ForeignKey(
                        name: "FK_Books_Publishers_publisher_id",
                        column: x => x.publisher_id,
                        principalTable: "Publishers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Books_Series_series_id",
                        column: x => x.series_id,
                        principalTable: "Series",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Books_Categories",
                columns: table => new
                {
                    BooksId = table.Column<int>(type: "integer", nullable: false),
                    CategoriesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books_Categories", x => new { x.BooksId, x.CategoriesId });
                    table.ForeignKey(
                        name: "FK_Books_Categories_Books_BooksId",
                        column: x => x.BooksId,
                        principalTable: "Books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Books_Categories_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Works",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    work_order = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    book_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Works", x => x.id);
                    table.ForeignKey(
                        name: "FK_Works_Books_book_id",
                        column: x => x.book_id,
                        principalTable: "Books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Works_Authors",
                columns: table => new
                {
                    AuthorsId = table.Column<int>(type: "integer", nullable: false),
                    WorksId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Works_Authors", x => new { x.AuthorsId, x.WorksId });
                    table.ForeignKey(
                        name: "FK_Works_Authors_Authors_AuthorsId",
                        column: x => x.AuthorsId,
                        principalTable: "Authors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Works_Authors_Works_WorksId",
                        column: x => x.WorksId,
                        principalTable: "Works",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Биографии и мемуары" },
                    { 2, "Биология" },
                    { 3, "Культура и искусство" },
                    { 4, "Лженаука" },
                    { 5, "Математика" },
                    { 6, "Медицина и биология человека" },
                    { 7, "Политика и история" },
                    { 8, "Поп-культура и миры" },
                    { 9, "Саморазвитие и коммуникация" },
                    { 10, "Спорт" },
                    { 11, "Тру-крайм" },
                    { 12, "Физика" },
                    { 13, "Фольклор и мифология" },
                    { 14, "Экономика и статистика" },
                    { 15, "Алгоритмы" },
                    { 16, "Архитектура компьютера" },
                    { 17, "Операционные системы" },
                    { 18, "Компьютерные сети" },
                    { 19, "Computer Science (общее)" },
                    { 20, "Базы данных" },
                    { 21, "Компьютерная безопасность" },
                    { 22, "Инструменты" },
                    { 23, "Проектирование и архитектура" },
                    { 24, "Микросервисы" },
                    { 25, "ООП" },
                    { 26, "Паттерны проектирования" },
                    { 27, "Рефакторинг" },
                    { 28, "API" },
                    { 29, "System Design" },
                    { 30, "DDD и моделирование данных" },
                    { 31, "Саморазвитие в IT" },
                    { 32, "Тестирование" },
                    { 33, "C# и .NET" },
                    { 34, "Frontend" },
                    { 35, "Go" },
                    { 36, "TypeScript" },
                    { 37, "Художественная литература" }
                });

            migrationBuilder.InsertData(
                table: "Publishers",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "АСТ" },
                    { 2, "Эксмо" },
                    { 3, "МИФ" },
                    { 4, "Альпина Паблишер" },
                    { 5, "Альпина Нон-фикшн" },
                    { 6, "Азбука" },
                    { 7, "Corpus" },
                    { 8, "Ad Marginem" },
                    { 9, "Livebook" },
                    { 10, "Individuum" },
                    { 11, "Питер" },
                    { 12, "ДМК Пресс" },
                    { 13, "O'Reilly Media" },
                    { 14, "Manning Publications" },
                    { 15, "No Starch Press" },
                    { 16, "Addison-Wesley" },
                    { 17, "Pearson" },
                    { 18, "Apress" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_authors_name",
                table: "Authors",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_books_cover_type",
                table: "Books",
                column: "cover_type");

            migrationBuilder.CreateIndex(
                name: "IX_Books_publisher_id",
                table: "Books",
                column: "publisher_id");

            migrationBuilder.CreateIndex(
                name: "IX_Books_series_id",
                table: "Books",
                column: "series_id");

            migrationBuilder.CreateIndex(
                name: "ix_books_title_volume_number",
                table: "Books",
                columns: new[] { "title", "volume_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_books_type",
                table: "Books",
                column: "type");

            migrationBuilder.CreateIndex(
                name: "IX_Books_Categories_CategoriesId",
                table: "Books_Categories",
                column: "CategoriesId");

            migrationBuilder.CreateIndex(
                name: "ix_categories_name",
                table: "Categories",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_publishers_name",
                table: "Publishers",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_series_parent_series_id",
                table: "Series",
                column: "parent_series_id");

            migrationBuilder.CreateIndex(
                name: "ix_series_title",
                table: "Series",
                column: "title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_works_book_id",
                table: "Works",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "IX_Works_Authors_WorksId",
                table: "Works_Authors",
                column: "WorksId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books_Categories");

            migrationBuilder.DropTable(
                name: "Works_Authors");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Works");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Publishers");

            migrationBuilder.DropTable(
                name: "Series");
        }
    }
}
