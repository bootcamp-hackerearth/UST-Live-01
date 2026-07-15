using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthAxisCore_Api.Migrations
{
    /// <inheritdoc />
    public partial class AddDoctorLeaveFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DoctorId",
                table: "Notifications",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOnLeave",
                table: "Doctors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "DoctorLeaves",
                columns: table => new
                {
                    LeaveId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorLeaves", x => x.LeaveId);
                    table.ForeignKey(
                        name: "FK_DoctorLeaves_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "DoctorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorLeaves_DoctorId",
                table: "DoctorLeaves",
                column: "DoctorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoctorLeaves");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "IsOnLeave",
                table: "Doctors");

            migrationBuilder.AlterColumn<int>(
                name: "DoctorId",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
