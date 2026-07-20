using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthAxis.API.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationIdempotency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppointmentId",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "UX_Notifications_Appointment_Doctor_Type",
                table: "Notifications",
                columns: new[] { "AppointmentId", "DoctorId", "NotificationType" },
                unique: true,
                filter: "[AppointmentId] IS NOT NULL AND [DoctorId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_Notifications_Appointment_Doctor_Type",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "Notifications");
        }
    }
}
