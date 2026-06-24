using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HealthCareApp.Migrations
{
    /// <inheritdoc />
    public partial class SeedDoctorData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "DoctorId", "ConsultationFee", "CreatedDate", "DoctorName", "IsActive", "Specialisation", "YearsOfExperience" },
                values: new object[,]
                {
                    { 1, 500, new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Arun Menon", true, 9, 10 },
                    { 2, 1000, new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Meera Nair", true, 8, 15 },
                    { 3, 700, new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vikram Das", true, 7, 8 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 3);
        }
    }
}
