using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthApp.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedDoctorTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Doctors",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "Specialisation" },
                values: new object[] { new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cardiologist" });

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 2,
                columns: new[] { "CreatedDate", "Specialisation" },
                values: new object[] { new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dermatologist" });

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 3,
                columns: new[] { "CreatedDate", "Specialisation" },
                values: new object[] { new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Neurologist" });

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 4,
                columns: new[] { "CreatedDate", "Specialisation" },
                values: new object[] { new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dermatologist" });

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 5,
                columns: new[] { "ConsultationFee", "CreatedDate", "Specialisation" },
                values: new object[] { 1000, new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "GeneralPractitioner" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Doctors");

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 1,
                column: "Specialisation",
                value: "Orthopedic");

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 2,
                column: "Specialisation",
                value: "Gynecologist");

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 3,
                column: "Specialisation",
                value: "ENT");

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 4,
                column: "Specialisation",
                value: "Psychiatrist");

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 5,
                columns: new[] { "ConsultationFee", "Specialisation" },
                values: new object[] { 3000, "Cardiologist" });
        }
    }
}
