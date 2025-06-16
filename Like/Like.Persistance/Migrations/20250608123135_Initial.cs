using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Like.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommentLikes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CommentId = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentLikes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PostLikes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PostId = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostLikes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CommentLikes",
                columns: new[] { "Id", "AuthorId", "CommentId", "CreatedAt", "TargetId" },
                values: new object[] { new Guid("6186e3f3-4b71-4555-9aeb-69c0c64a27c8"), new Guid("a1234567-89ab-4cde-9012-3456789abcde"), new Guid("75c9cdb1-8706-4207-b0ee-792349916511"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("75c9cdb1-8706-4207-b0ee-792349916511") });

            migrationBuilder.InsertData(
                table: "PostLikes",
                columns: new[] { "Id", "AuthorId", "CreatedAt", "PostId", "TargetId" },
                values: new object[] { new Guid("c7e7774a-ff56-4de5-83ae-9fc5742ca05b"), new Guid("a1234567-89ab-4cde-9012-3456789abcde"), new DateTime(2024, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("4e0e9111-9e6b-4e20-8b40-b5fd06fb9069"), new Guid("4e0e9111-9e6b-4e20-8b40-b5fd06fb9069") });

            migrationBuilder.CreateIndex(
                name: "IX_CommentLike_CommentId_AuthorId",
                table: "CommentLikes",
                columns: new[] { "CommentId", "AuthorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommentLikes_AuthorId_CommentId",
                table: "CommentLikes",
                columns: new[] { "AuthorId", "CommentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostLike_PostId_AuthorId",
                table: "PostLikes",
                columns: new[] { "PostId", "AuthorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostLikes_AuthorId_PostId",
                table: "PostLikes",
                columns: new[] { "AuthorId", "PostId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentLikes");

            migrationBuilder.DropTable(
                name: "PostLikes");
        }
    }
}
