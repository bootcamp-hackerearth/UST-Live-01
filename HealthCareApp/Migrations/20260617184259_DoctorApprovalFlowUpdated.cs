using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthCareApp.Migrations
{
    /// <inheritdoc />
    public partial class DoctorApprovalFlowUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 1,
                columns: new[] { "ConsultationFee", "Email", "VerificationStatus" },
                values: new object[] { 5000, "arun.menon@example.com", 2 });

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 2,
                columns: new[] { "Email", "VerificationStatus" },
                values: new object[] { "meera.nair@example.com", 2 });

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 3,
                columns: new[] { "Email", "VerificationStatus" },
                values: new object[] { "vikram.das@example.com", 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 1,
                columns: new[] { "ConsultationFee", "Email", "VerificationStatus" },
                values: new object[] { 500, "", 1 });

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 2,
                columns: new[] { "Email", "VerificationStatus" },
                values: new object[] { "", 1 });

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 3,
                columns: new[] { "Email", "VerificationStatus" },
                values: new object[] { "", 1 });
        }
    }
}
