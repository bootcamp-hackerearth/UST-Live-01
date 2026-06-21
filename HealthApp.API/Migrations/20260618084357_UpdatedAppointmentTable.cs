using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthApp.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedAppointmentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HealthRecords_AppointmentId",
                table: "HealthRecords");

            migrationBuilder.AlterColumn<string>(
                name: "CancellationReason",
                table: "Appointments",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Appointments",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.CreateIndex(
                name: "IX_HealthRecords_AppointmentId",
                table: "HealthRecords",
                column: "AppointmentId",
                unique: true,
                filter: "[AppointmentId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HealthRecords_AppointmentId",
                table: "HealthRecords");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Appointments");

            migrationBuilder.AlterColumn<string>(
                name: "CancellationReason",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.CreateIndex(
                name: "IX_HealthRecords_AppointmentId",
                table: "HealthRecords",
                column: "AppointmentId");
        }
    }
}
