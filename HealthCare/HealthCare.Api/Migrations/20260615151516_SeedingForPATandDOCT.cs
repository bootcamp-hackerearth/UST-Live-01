using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthCare.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedingForPATandDOCT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "DoctorId", "ConsultationFee", "FullName", "IsActive", "Specialisation", "UserId", "YearsOfExperience" },
                values: new object[] { 1, 500m, "Dr. Arun Kumar", true, "Cardiology", 3, 3 });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "PatientId", "DateOfBirth", "FullName", "Gender", "InsuranceId", "IsActived", "PhoneNumber", "UserId" },
                values: new object[] { 1, new DateOnly(2003, 4, 7), "Abishek", "Male", null, true, "9876543210", 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "PatientId",
                keyValue: 1);
        }
    }
}
