using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentStarLab.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPayments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DoctorId1",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Payments",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Works_DoctorId_WorkDate",
                table: "Works",
                columns: new[] { "DoctorId", "WorkDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Works_WorkDate",
                table: "Works",
                column: "WorkDate");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_DoctorId1",
                table: "Payments",
                column: "DoctorId1");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentDate",
                table: "Payments",
                column: "PaymentDate");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Doctors_DoctorId1",
                table: "Payments",
                column: "DoctorId1",
                principalTable: "Doctors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Doctors_DoctorId1",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Works_DoctorId_WorkDate",
                table: "Works");

            migrationBuilder.DropIndex(
                name: "IX_Works_WorkDate",
                table: "Works");

            migrationBuilder.DropIndex(
                name: "IX_Payments_DoctorId1",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_PaymentDate",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DoctorId1",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Payments");
        }
    }
}
