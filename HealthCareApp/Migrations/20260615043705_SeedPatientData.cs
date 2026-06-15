using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HealthCareApp.Migrations
{
    /// <inheritdoc />
    public partial class SeedPatientData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "PatientId", "CreatedDate", "DateOfBirth", "Email", "Gender", "InsuranceID", "PatientName", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1998, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "ravi.kumar@example.com", 0, "INS1001", "Ravi Kumar", "9876543210" },
                    { 2, new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2001, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "anjali.nair@example.com", 1, "INS1002", "Anjali Nair", "8765432109" },
                    { 3, new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1995, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "kiran.das@example.com", 3, null, "Kiran Das", "7654321098" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "PatientId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "PatientId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "PatientId",
                keyValue: 3);
        }
    }
}
