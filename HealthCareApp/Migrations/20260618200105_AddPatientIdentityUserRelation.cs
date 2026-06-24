using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthCareApp.Migrations
{
    /// <inheritdoc />
    public partial class AddPatientIdentityUserRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Patients",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "PatientId",
                keyValue: 1,
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "PatientId",
                keyValue: 2,
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "PatientId",
                keyValue: 3,
                column: "IdentityUserId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_IdentityUserId",
                table: "Patients",
                column: "IdentityUserId",
                unique: true,
                filter: "[IdentityUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_AspNetUsers_IdentityUserId",
                table: "Patients",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_AspNetUsers_IdentityUserId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_IdentityUserId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Patients");
        }
    }
}
