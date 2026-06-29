using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthAxisApplicn.Migrations
{
    /// <inheritdoc />
    public partial class HealthRecordLinkToAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppointmentId",
                table: "HealthRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_HealthRecords_AppointmentId",
                table: "HealthRecords",
                column: "AppointmentId");

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

            migrationBuilder.DropIndex(
                name: "IX_HealthRecords_AppointmentId",
                table: "HealthRecords");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "HealthRecords");
        }
    }
}
