using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autopark.API.Migrations
{
    /// <inheritdoc />
    public partial class VehicleDriverOneToOneCorrect : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Drivers_CurrentDriverId",
                schema: "identity",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_CurrentDriverId",
                schema: "identity",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "CurrentDriverId",
                schema: "identity",
                table: "Vehicles");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_CurrentVehicleId",
                schema: "identity",
                table: "Drivers",
                column: "CurrentVehicleId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Vehicles_CurrentVehicleId",
                schema: "identity",
                table: "Drivers",
                column: "CurrentVehicleId",
                principalSchema: "identity",
                principalTable: "Vehicles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Vehicles_CurrentVehicleId",
                schema: "identity",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_CurrentVehicleId",
                schema: "identity",
                table: "Drivers");

            migrationBuilder.AddColumn<Guid>(
                name: "CurrentDriverId",
                schema: "identity",
                table: "Vehicles",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_CurrentDriverId",
                schema: "identity",
                table: "Vehicles",
                column: "CurrentDriverId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Drivers_CurrentDriverId",
                schema: "identity",
                table: "Vehicles",
                column: "CurrentDriverId",
                principalSchema: "identity",
                principalTable: "Drivers",
                principalColumn: "Id");
        }
    }
}
