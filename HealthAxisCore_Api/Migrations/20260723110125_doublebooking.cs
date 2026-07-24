using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthAxisCore_Api.Migrations
{
    /// <inheritdoc />
    public partial class doublebooking : Migration
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

            migrationBuilder.AlterColumn<string>(
                name: "TimeSlot",
                table: "Appointments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "UX_Appointments_DoctorId_ScheduledDate_TimeSlot",
                table: "Appointments",
                columns: new[] { "DoctorId", "ScheduledDate", "TimeSlot" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_Appointments_PatientId_ScheduledDate_TimeSlot",
                table: "Appointments",
                columns: new[] { "PatientId", "ScheduledDate", "TimeSlot" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_Appointments_DoctorId_ScheduledDate_TimeSlot",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "UX_Appointments_PatientId_ScheduledDate_TimeSlot",
                table: "Appointments");

            migrationBuilder.AlterColumn<string>(
                name: "TimeSlot",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

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
