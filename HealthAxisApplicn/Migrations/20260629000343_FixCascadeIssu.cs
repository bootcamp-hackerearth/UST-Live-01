using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HealthAxisApplicn.Migrations
{
    /// <inheritdoc />
    public partial class FixCascadeIssu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Patients",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Doctors",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_UserId",
                table: "Patients",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_UserId",
                table: "Doctors",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_AspNetUsers_UserId",
                table: "Doctors",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_AspNetUsers_UserId",
                table: "Patients",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_AspNetUsers_UserId",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_AspNetUsers_UserId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_UserId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_UserId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Doctors");

            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "DoctorId", "ConsultationFee", "DoctorName", "Email", "IsActive", "Specialisation", "YearsOfExperience" },
                values: new object[,]
                {
                    { 1, 1000m, "Ranjith Govind", "ranjith.govind@example.com", true, "Cardiologist", 3 },
                    { 2, 1200m, "Rahul Nambiar", "rahul.nambiar@example.com", true, "Endocrinologist", 4 },
                    { 3, 1100m, "Atif Aslam", "atif.aslam@example.com", true, "Dermatologist", 2 }
                });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "PatientId", "DateOfBirth", "Email", "Gender", "InsuranceID", "IsActive", "PatientName", "PhoneNo" },
                values: new object[,]
                {
                    { 1, new DateTime(1990, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "nakul@gmail.com", "Male", null, true, "Nakul", "9887756746" },
                    { 2, new DateTime(1995, 8, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "nelwyn@gmail.com", "Male", null, true, "Nelwyn", "9778864535" },
                    { 3, new DateTime(1978, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "vikram@gmail.com", "Male", null, true, "Vikram", "9335252312" }
                });
        }
    }
}
