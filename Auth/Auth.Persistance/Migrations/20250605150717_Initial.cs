using System;
using Core;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auth.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_Auth_AuthId",
                table: "RefreshToken");

            migrationBuilder.DropTable(
                name: "Auth");

            migrationBuilder.RenameColumn(
                name: "AuthId",
                table: "RefreshToken",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshToken_AuthId",
                table: "RefreshToken",
                newName: "IX_RefreshToken_UserId");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Login = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<UserRole>(type: "role", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Login", "PasswordHash", "Role" },
                values: new object[] { new Guid("a1234567-89ab-4cde-9012-3456789abcde"), "admin", "$2a$10$4cOi70lokMvP3bIsu4cWweIMaq9O9C53UelbFm3HSOkIgxxxc.d4W", UserRole.Admin });

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_User_UserId",
                table: "RefreshToken",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_User_UserId",
                table: "RefreshToken");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "RefreshToken",
                newName: "AuthId");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshToken",
                newName: "IX_RefreshToken_AuthId");

            migrationBuilder.CreateTable(
                name: "Auth",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Login = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<UserRole>(type: "role", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Auth",
                columns: new[] { "Id", "Login", "PasswordHash", "Role" },
                values: new object[] { new Guid("a1234567-89ab-4cde-9012-3456789abcde"), "admin", "$2a$10$4cOi70lokMvP3bIsu4cWweIMaq9O9C53UelbFm3HSOkIgxxxc.d4W", UserRole.Admin });

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_Auth_AuthId",
                table: "RefreshToken",
                column: "AuthId",
                principalTable: "Auth",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
