using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HealthApp.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataToPatientTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "PatientId", "DateOfBirth", "Email", "Gender", "InsuranceId", "PatientName", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "johnmathew@gmail.com", "Male", "INS001", "John Mathew", "8796534521" },
                    { 2, new DateTime(1985, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "rameshk@gmail.com", "Male", "INS1002", "Ramesh Kumar", "9876543210" },
                    { 3, new DateTime(1992, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "priya@gmail.com", "Female", "INS1003", "Priya Sharma", "8765432109" },
                    { 4, new DateTime(2000, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "arunraj@gmail.com", "Male", "INS1004", "Arun Raj", "7654321098" },
                    { 5, new DateTime(2001, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "meenu@gmail.com", "Female", "INS1005", "Meena Devi", "6543210987" }
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

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "PatientId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "PatientId",
                keyValue: 5);
        }
    }
}
