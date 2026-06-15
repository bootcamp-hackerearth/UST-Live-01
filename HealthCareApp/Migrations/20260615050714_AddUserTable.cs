using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HealthCareApp.Migrations
{
    /// <inheritdoc />
    public partial class AddUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedDate", "Email", "PasswordHash", "PasswordSalt", "ReferenceId", "Role" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@healthaxis.com", "AdminPasswordHashDemo", new byte[] { 1, 2, 3, 4, 5 }, null, 0 },
                    { 2, new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "ravi.kumar@example.com", "PatientPasswordHashDemo", new byte[] { 6, 7, 8, 9, 10 }, 1, 1 },
                    { 3, new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "arun.menon@example.com", "DoctorPasswordHashDemo", new byte[] { 11, 12, 13, 14, 15 }, 1, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
