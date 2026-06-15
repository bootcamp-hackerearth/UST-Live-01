using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HealthCareApp.Migrations
{
    /// <inheritdoc />
    public partial class AddAppointmentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    AppointmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeSlot = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CancellationReason = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.AppointmentId);
                    table.ForeignKey(
                        name: "FK_Appointments_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "DoctorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointments_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "AppointmentId", "CancellationReason", "CreatedDate", "DoctorId", "PatientId", "ScheduledDate", "Status", "TimeSlot" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "10:00 AM - 10:30 AM" },
                    { 2, null, new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 2, new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "11:00 AM - 11:30 AM" },
                    { 3, "Patient requested cancellation", new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 3, new DateTime(2026, 6, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "02:00 PM - 02:30 PM" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId",
                table: "Appointments",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientId",
                table: "Appointments",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");
        }
    }
}
