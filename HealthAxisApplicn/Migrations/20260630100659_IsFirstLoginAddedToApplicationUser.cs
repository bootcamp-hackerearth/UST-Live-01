using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthAxisApplicn.Migrations
{
    /// <inheritdoc />
    public partial class IsFirstLoginAddedToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HealthRecords_AppointmentId",
                table: "HealthRecords");

            migrationBuilder.AddColumn<bool>(
                name: "IsFirstLogin",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_HealthRecords_AppointmentId",
                table: "HealthRecords",
                column: "AppointmentId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HealthRecords_AppointmentId",
                table: "HealthRecords");

            migrationBuilder.DropColumn(
                name: "IsFirstLogin",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_HealthRecords_AppointmentId",
                table: "HealthRecords",
                column: "AppointmentId");
        }
    }
}
