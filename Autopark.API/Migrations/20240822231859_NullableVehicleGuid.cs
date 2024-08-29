using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autopark.API.Migrations
{
    /// <inheritdoc />
    public partial class NullableVehicleGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vehicles_Drivers_CurrentDriverId",
                table: "vehicles");

            migrationBuilder.AlterColumn<Guid>(
                name: "CurrentDriverId",
                table: "vehicles",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CurrentVehicleId",
                table: "Drivers",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_vehicles_Drivers_CurrentDriverId",
                table: "vehicles",
                column: "CurrentDriverId",
                principalTable: "Drivers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vehicles_Drivers_CurrentDriverId",
                table: "vehicles");

            migrationBuilder.AlterColumn<Guid>(
                name: "CurrentDriverId",
                table: "vehicles",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CurrentVehicleId",
                table: "Drivers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_vehicles_Drivers_CurrentDriverId",
                table: "vehicles",
                column: "CurrentDriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
