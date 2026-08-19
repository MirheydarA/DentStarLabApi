using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentStarLab.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDoctorAccessToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Doctors_DoctorId1",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_DoctorId1",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DoctorId1",
                table: "Payments");

            migrationBuilder.AddColumn<Guid>(
                name: "AccessToken",
                table: "Doctors",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.Empty);

            migrationBuilder.Sql("""
                UPDATE Doctors
                SET AccessToken = NEWID()
                WHERE AccessToken = '00000000-0000-0000-0000-000000000000';
                """);

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_AccessToken",
                table: "Doctors",
                column: "AccessToken",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Doctors_AccessToken",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "AccessToken",
                table: "Doctors");

            migrationBuilder.AddColumn<int>(
                name: "DoctorId1",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_DoctorId1",
                table: "Payments",
                column: "DoctorId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Doctors_DoctorId1",
                table: "Payments",
                column: "DoctorId1",
                principalTable: "Doctors",
                principalColumn: "Id");
        }
    }
}
