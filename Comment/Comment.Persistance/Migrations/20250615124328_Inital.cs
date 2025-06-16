using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Comment.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class Inital : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    PostId = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Medias = table.Column<string[]>(type: "text[]", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EditedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "AuthorId", "Content", "CreatedAt", "EditedAt", "Medias", "PostId" },
                values: new object[] { new Guid("75c9cdb1-8706-4207-b0ee-792349916511"), new Guid("a1234567-89ab-4cde-9012-3456789abcde"), "This is a sample comment.", new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), null, new string[0], new Guid("4e0e9111-9e6b-4e20-8b40-b5fd06fb9069") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");
        }
    }
}
