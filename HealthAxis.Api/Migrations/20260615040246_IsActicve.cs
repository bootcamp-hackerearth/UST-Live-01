using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HealthAxisCore_Api.Migrations
{
    /// <inheritdoc />
    public partial class IsActicve : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthRecords_Appointments_AppointmentId",
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
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 10);

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

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "PatientId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "PatientId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "PatientId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "PatientId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "PatientId",
                keyValue: 10);

            migrationBuilder.AlterColumn<string>(
                name: "InsuranceID",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Patients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "HealthRecords",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "AppointmentId",
                table: "HealthRecords",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthRecords_Appointments_AppointmentId",
                table: "HealthRecords",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "AppointmentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthRecords_Appointments_AppointmentId",
                table: "HealthRecords");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Patients");

            migrationBuilder.AlterColumn<string>(
                name: "InsuranceID",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "HealthRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AppointmentId",
                table: "HealthRecords",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "DoctorId", "ConsultationFee", "DoctorName", "IsActive", "Specialisation", "YearsOfExperience" },
                values: new object[,]
                {
                    { 1, 800, "Arjun Nair", true, "Cardiologist", 12 },
                    { 2, 600, "Meera Pillai", true, "Dermatologist", 8 },
                    { 3, 1200, "Rahul Menon", true, "Neurologist", 15 },
                    { 4, 700, "Anjali Nair", true, "Gynecologist", 10 },
                    { 5, 1500, "Vivek Kumar", false, "OrthopedicSurgeon", 18 },
                    { 6, 500, "Sneha Reddy", true, "Pediatrician", 6 },
                    { 7, 900, "Amit Sharma", true, "Psychiatrist", 9 },
                    { 8, 1100, "Priya Iyer", true, "Endocrinologist", 11 },
                    { 9, 1300, "Karthik Das", false, "Oncologist", 14 },
                    { 10, 300, "Rohit Verma", true, "GeneralPractitioner", 5 }
                });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "PatientId", "DateOfBirth", "Email", "Gender", "InsuranceID", "PatientName", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, new DateTime(1998, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "anu.nair@example.com", "Female", "INS1001", "Anu Nair", "9876543210" },
                    { 2, new DateTime(1992, 9, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "rahul.menon@example.com", "Male", "INS1002", "Rahul Menon", "9876543211" },
                    { 3, new DateTime(2000, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "meera.pillai@example.com", "Female", "INS1003", "Meera Pillai", "9876543212" },
                    { 4, new DateTime(1985, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "arjun.kumar@example.com", "Male", "INS1004", "Arjun Kumar", "9876543213" },
                    { 5, new DateTime(1995, 7, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "sneha.reddy@example.com", "Female", "INS1005", "Sneha Reddy", "9876543214" },
                    { 6, new DateTime(1988, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "vivek.sharma@example.com", "Male", "INS1006", "Vivek Sharma", "9876543215" },
                    { 7, new DateTime(1999, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "priya.iyer@example.com", "Female", "INS1007", "Priya Iyer", "9876543216" },
                    { 8, new DateTime(1993, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "kiran.das@example.com", "Male", "INS1008", "Kiran Das", "9876543217" },
                    { 9, new DateTime(1990, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "alex.joseph@example.com", "Other", "INS1009", "Alex Joseph", "9876543218" },
                    { 10, new DateTime(1997, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "sam.thomas@example.com", "Transgender", "INS1010", "Sam Thomas", "9876543219" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_HealthRecords_Appointments_AppointmentId",
                table: "HealthRecords",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "AppointmentId");
        }
    }
}
