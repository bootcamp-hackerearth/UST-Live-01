using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HealthAxisApplicn.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "DoctorId", "ConsultationFee", "DoctorName", "IsActive", "Specialisation", "YearsOfExperience" },
                values: new object[,]
                {
                    { 1, 1000m, "Ranjith Govind", true, "Cardiologist", 3 },
                    { 2, 1200m, "Rahul Nambiar", true, "Endocrinologist", 4 },
                    { 3, 1100m, "Atif Aslam", true, "Dermatologist", 2 }
                });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "PatientId", "DateOfBirth", "Email", "Gender", "InsuranceID", "PatientName", "PhoneNo" },
                values: new object[,]
                {
                    { 1, new DateTime(1990, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "nakul@gmail.com", "Male", null, "Nakul", "9887756746" },
                    { 2, new DateTime(1995, 8, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "nelwyn@gmail.com", "Male", null, "Nelwyn", "9778864535" },
                    { 3, new DateTime(1978, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "vikram@gmail.com", "Male", null, "Vikram", "9335252312" }
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
