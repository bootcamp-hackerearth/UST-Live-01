using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthAxisApplicn.Migrations
{
    /// <inheritdoc />
    public partial class AddingIsActiveToPatient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Patients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "PatientId",
                keyValue: 1,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "PatientId",
                keyValue: 2,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "PatientId",
                keyValue: 3,
                column: "IsActive",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Patients");
        }
    }
}
