using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthCareApp.Migrations
{
    /// <inheritdoc />
    public partial class DoctorApprovalFlowAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VerificationStatus",
                table: "Doctors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 1,
                columns: new[] { "Email", "IdentityUserId", "VerificationStatus" },
                values: new object[] { "", null, 1 });

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 2,
                columns: new[] { "Email", "IdentityUserId", "VerificationStatus" },
                values: new object[] { "", null, 1 });

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 3,
                columns: new[] { "Email", "IdentityUserId", "VerificationStatus" },
                values: new object[] { "", null, 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "VerificationStatus",
                table: "Doctors");
        }
    }
}
