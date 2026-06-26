using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HealthApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "DoctorId", "ConsultationFee", "DoctorEmail", "DoctorPhoneNo", "FullName", "IsActive", "Specialisation", "YearsOfExperience" },
                values: new object[,]
                {
                    { 1, 600.00m, "gregory.house@healthapp.com", "9000000101", "Dr. Gregory House", true, 0, 15 },
                    { 2, 500.00m, "lisa.cuddy@healthapp.com", "9000000102", "Dr. Lisa Cuddy", true, 8, 12 },
                    { 3, 1000.00m, "derek.shepherd@healthapp.com", "9000000103", "Dr. Derek Shepherd", true, 3, 18 },
                    { 4, 450.00m, "meredith.grey@healthapp.com", "9000000104", "Dr. Meredith Grey", true, 0, 8 },
                    { 5, 750.00m, "cristina.yang@healthapp.com", "9000000105", "Dr. Cristina Yang", true, 1, 10 },
                    { 6, 800.00m, "richard.webber@healthapp.com", "9000000106", "Dr. Richard Webber", true, 0, 30 },
                    { 7, 550.00m, "miranda.bailey@healthapp.com", "9000000107", "Dr. Miranda Bailey", true, 0, 14 },
                    { 8, 400.00m, "alex.karev@healthapp.com", "9000000108", "Dr. Alex Karev", true, 5, 7 },
                    { 9, 650.00m, "jackson.avery@healthapp.com", "9000000109", "Dr. Jackson Avery", true, 7, 9 },
                    { 10, 400.00m, "april.kepner@healthapp.com", "9000000110", "Dr. April Kepner", true, 4, 6 }
                });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "PatientId", "CreatedDate", "DateOfBirth", "Email", "FullName", "Gender", "InsuranceId", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(1985, 4, 12), "michael.j@gmail.com", "Michael Johnson", "Male", "INS-1001", "8000000501" },
                    { 2, new DateTime(2025, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(1990, 8, 25), "sarah.w@gmail.com", "Sarah Williams", "Female", "INS-1002", "8000000502" },
                    { 3, new DateTime(2025, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(1978, 2, 14), "david.b@gmail.com", "David Brown", "Male", "INS-1003", "8000000503" },
                    { 4, new DateTime(2025, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(1995, 11, 30), "emily.d@gmail.com", "Emily Davis", "Female", "INS-1004", "8000000504" },
                    { 5, new DateTime(2025, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(1982, 7, 19), "james.m@gmail.com", "James Miller", "Male", "INS-1005", "8000000505" },
                    { 6, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(1988, 9, 5), "jessica.w@gmail.com", "Jessica Wilson", "Female", "INS-1006", "8000000506" },
                    { 7, new DateTime(2025, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(1970, 12, 1), "robert.m@gmail.com", "Robert Moore", "Male", "INS-1007", "8000000507" },
                    { 8, new DateTime(2025, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(1992, 3, 22), "amanda.t@gmail.com", "Amanda Taylor", "Female", "INS-1008", "8000000508" },
                    { 9, new DateTime(2025, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(1980, 5, 17), "william.a@gmail.com", "William Anderson", "Male", "INS-1009", "8000000509" },
                    { 10, new DateTime(2025, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(1998, 10, 8), "ashley.t@gmail.com", "Ashley Thomas", "Female", "INS-1010", "8000000510" }
                });

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "AppointmentId", "CancellationReason", "DoctorId", "PatientId", "ScheduledDate", "Status", "TimeSlot" },
                values: new object[,]
                {
                    { 1001, null, 1, 1, new DateOnly(2025, 10, 15), 3, "09:00 AM - 09:30 AM" },
                    { 1002, null, 2, 2, new DateOnly(2025, 11, 20), 3, "10:00 AM - 10:30 AM" },
                    { 1003, null, 3, 3, new DateOnly(2026, 1, 10), 3, "11:30 AM - 12:00 PM" },
                    { 1004, null, 4, 4, new DateOnly(2026, 2, 14), 3, "02:00 PM - 02:30 PM" },
                    { 1005, "Patient had a sudden family emergency.", 5, 5, new DateOnly(2026, 4, 10), 2, "04:00 PM - 04:30 PM" },
                    { 1006, null, 1, 6, new DateOnly(2026, 8, 15), 1, "09:00 AM - 09:30 AM" },
                    { 1007, null, 2, 7, new DateOnly(2026, 9, 16), 1, "10:00 AM - 10:30 AM" },
                    { 1008, null, 3, 8, new DateOnly(2026, 10, 17), 1, "11:30 AM - 12:00 PM" },
                    { 1009, null, 4, 9, new DateOnly(2026, 11, 20), 0, "02:00 PM - 02:30 PM" },
                    { 1010, null, 5, 10, new DateOnly(2026, 12, 5), 0, "04:00 PM - 04:30 PM" }
                });

            migrationBuilder.InsertData(
                table: "HealthRecords",
                columns: new[] { "RecordId", "AppointmentId", "Diagnosis", "DoctorId", "Notes", "PatientId", "Prescription", "VisitDate" },
                values: new object[,]
                {
                    { 1001, 1001, "Essential Hypertension", 1, "Patient educated on low sodium diet.", 1, "Lisinopril 10mg daily", new DateOnly(2025, 10, 15) },
                    { 1002, 1002, "Routine Pregnancy Checkup", 2, "Fetal heart rate normal. Continue regular checkups.", 2, "Prenatal Vitamins", new DateOnly(2025, 11, 20) },
                    { 1003, 1003, "Migraine with Aura", 3, "Advised to keep a headache diary to identify triggers.", 3, "Sumatriptan 50mg as needed", new DateOnly(2026, 1, 10) },
                    { 1004, 1004, "Acute Bronchitis", 4, "Rest and hydration recommended.", 4, "Azithromycin 500mg daily", new DateOnly(2026, 2, 14) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentId",
                keyValue: 1005);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentId",
                keyValue: 1006);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentId",
                keyValue: 1007);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentId",
                keyValue: 1008);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentId",
                keyValue: 1009);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentId",
                keyValue: 1010);

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
                table: "HealthRecords",
                keyColumn: "RecordId",
                keyValue: 1001);

            migrationBuilder.DeleteData(
                table: "HealthRecords",
                keyColumn: "RecordId",
                keyValue: 1002);

            migrationBuilder.DeleteData(
                table: "HealthRecords",
                keyColumn: "RecordId",
                keyValue: 1003);

            migrationBuilder.DeleteData(
                table: "HealthRecords",
                keyColumn: "RecordId",
                keyValue: 1004);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentId",
                keyValue: 1001);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentId",
                keyValue: 1002);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentId",
                keyValue: 1003);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentId",
                keyValue: 1004);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 5);

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
        }
    }
}
