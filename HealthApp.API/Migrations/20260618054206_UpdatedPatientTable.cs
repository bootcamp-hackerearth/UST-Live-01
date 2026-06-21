using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthApp.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedPatientTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Patients",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "Prescription",
                table: "HealthRecords",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "HealthRecords",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Diagnosis",
                table: "HealthRecords",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "PatientId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "InsuranceId" },
                values: new object[] { new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "INS1001" });

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "PatientId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "PatientId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "PatientId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "PatientId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Patients");

            migrationBuilder.AlterColumn<string>(
                name: "Prescription",
                table: "HealthRecords",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "HealthRecords",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Diagnosis",
                table: "HealthRecords",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "PatientId",
                keyValue: 1,
                column: "InsuranceId",
                value: "INS001");
        }
    }
}
