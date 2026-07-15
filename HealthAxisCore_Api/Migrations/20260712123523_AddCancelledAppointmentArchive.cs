using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthAxisCore_Api.Migrations
{
    /// <inheritdoc />
    public partial class AddCancelledAppointmentArchive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CancelledAppointmentArchives",
                columns: table => new
                {
                    CancelledAppointmentArchiveId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OriginalAppointmentId = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    PatientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    DoctorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeSlot = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CancellationReason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CancelledByRole = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CancelledByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CancelledAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WasAutoCancelled = table.Column<bool>(type: "bit", nullable: false),
                    ArchivedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LegalHold = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CancelledAppointmentArchives", x => x.CancelledAppointmentArchiveId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CancelledAppointmentArchives_DoctorId",
                table: "CancelledAppointmentArchives",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_CancelledAppointmentArchives_OriginalAppointmentId",
                table: "CancelledAppointmentArchives",
                column: "OriginalAppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_CancelledAppointmentArchives_PatientId",
                table: "CancelledAppointmentArchives",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_CancelledAppointmentArchives_ScheduledDate",
                table: "CancelledAppointmentArchives",
                column: "ScheduledDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CancelledAppointmentArchives");
        }
    }
}
