using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthAxis.API.Migrations
{
    /// <inheritdoc />
    public partial class AddAppointmentConcurrencyProtection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointments_DoctorId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_PatientId",
                table: "Appointments");

            migrationBuilder.CreateIndex(
                name: "UX_Appointments_Doctor_Slot_Active",
                table: "Appointments",
                columns: new[] { "DoctorId", "ScheduledDate", "TimeSlot" },
                unique: true,
                filter: "[Status] IN ('Pending', 'Confirmed')");

            migrationBuilder.CreateIndex(
                name: "UX_Appointments_Patient_Slot_Active",
                table: "Appointments",
                columns: new[] { "PatientId", "ScheduledDate", "TimeSlot" },
                unique: true,
                filter: "[Status] IN ('Pending', 'Confirmed')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_Appointments_Doctor_Slot_Active",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "UX_Appointments_Patient_Slot_Active",
                table: "Appointments");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId",
                table: "Appointments",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientId",
                table: "Appointments",
                column: "PatientId");
        }
    }
}
