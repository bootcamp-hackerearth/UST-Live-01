using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HealthApp.API.Migrations
{
    /// <inheritdoc />
    public partial class finaldb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Doctors_DoctorId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Patients_PatientId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthRecords_Appointments_AppointmentId",
                table: "HealthRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthRecords_Doctors_DoctorId",
                table: "HealthRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthRecords_Patients_PatientId",
                table: "HealthRecords");

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
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 5);

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

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Patients",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "HealthRecords",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Doctors",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "MustChangePassword",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Appointments",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "CancellationReason",
                table: "Appointments",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    RefreshTokenId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JwtId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.RefreshTokenId);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Doctors_DoctorId",
                table: "Appointments",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "DoctorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Patients_PatientId",
                table: "Appointments",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthRecords_Appointments_AppointmentId",
                table: "HealthRecords",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "AppointmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthRecords_Doctors_DoctorId",
                table: "HealthRecords",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "DoctorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthRecords_Patients_PatientId",
                table: "HealthRecords",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Doctors_DoctorId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Patients_PatientId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthRecords_Appointments_AppointmentId",
                table: "HealthRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthRecords_Doctors_DoctorId",
                table: "HealthRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthRecords_Patients_PatientId",
                table: "HealthRecords");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MustChangePassword",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Patients",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "HealthRecords",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Doctors",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Appointments",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "CancellationReason",
                table: "Appointments",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "DoctorId", "ConsultationFee", "CreatedDate", "DoctorName", "IsActive", "Specialisation", "YearsOfExperience" },
                values: new object[,]
                {
                    { 1, 1500, new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vignesh Kumar", true, "Cardiologist", 14 },
                    { 2, 1800, new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sneha Paul", true, "Dermatologist", 20 },
                    { 3, 900, new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hari Narayanan", true, "Neurologist", 9 },
                    { 4, 2000, new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Martin Smith", false, "Dermatologist", 11 },
                    { 5, 1000, new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bharath Raj", true, "GeneralPractitioner", 25 }
                });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "PatientId", "CreatedDate", "DateOfBirth", "Email", "Gender", "InsuranceId", "PatientName", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "johnmathew@gmail.com", "Male", "INS1001", "John Mathew", "8796534521" },
                    { 2, new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1985, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "rameshk@gmail.com", "Male", "INS1002", "Ramesh Kumar", "9876543210" },
                    { 3, new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1992, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "priya@gmail.com", "Female", "INS1003", "Priya Sharma", "8765432109" },
                    { 4, new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2000, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "arunraj@gmail.com", "Male", "INS1004", "Arun Raj", "7654321098" },
                    { 5, new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2001, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "meenu@gmail.com", "Female", "INS1005", "Meena Devi", "6543210987" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Doctors_DoctorId",
                table: "Appointments",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "DoctorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Patients_PatientId",
                table: "Appointments",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthRecords_Appointments_AppointmentId",
                table: "HealthRecords",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "AppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthRecords_Doctors_DoctorId",
                table: "HealthRecords",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthRecords_Patients_PatientId",
                table: "HealthRecords",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId");
        }
    }
}
