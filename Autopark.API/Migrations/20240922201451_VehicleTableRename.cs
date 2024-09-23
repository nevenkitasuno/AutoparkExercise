using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autopark.API.Migrations
{
    /// <inheritdoc />
    public partial class VehicleTableRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DriverVehicle_vehicles_VehiclesId",
                schema: "identity",
                table: "DriverVehicle");

            migrationBuilder.DropForeignKey(
                name: "FK_vehicles_Brands_BrandId",
                schema: "identity",
                table: "vehicles");

            migrationBuilder.DropForeignKey(
                name: "FK_vehicles_Drivers_CurrentDriverId",
                schema: "identity",
                table: "vehicles");

            migrationBuilder.DropForeignKey(
                name: "FK_vehicles_Enterprises_EnterpriseId",
                schema: "identity",
                table: "vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_vehicles",
                schema: "identity",
                table: "vehicles");

            migrationBuilder.RenameTable(
                name: "vehicles",
                schema: "identity",
                newName: "Vehicles",
                newSchema: "identity");

            migrationBuilder.RenameIndex(
                name: "IX_vehicles_EnterpriseId",
                schema: "identity",
                table: "Vehicles",
                newName: "IX_Vehicles_EnterpriseId");

            migrationBuilder.RenameIndex(
                name: "IX_vehicles_CurrentDriverId",
                schema: "identity",
                table: "Vehicles",
                newName: "IX_Vehicles_CurrentDriverId");

            migrationBuilder.RenameIndex(
                name: "IX_vehicles_BrandId",
                schema: "identity",
                table: "Vehicles",
                newName: "IX_Vehicles_BrandId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehicles",
                schema: "identity",
                table: "Vehicles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DriverVehicle_Vehicles_VehiclesId",
                schema: "identity",
                table: "DriverVehicle",
                column: "VehiclesId",
                principalSchema: "identity",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Brands_BrandId",
                schema: "identity",
                table: "Vehicles",
                column: "BrandId",
                principalSchema: "identity",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Drivers_CurrentDriverId",
                schema: "identity",
                table: "Vehicles",
                column: "CurrentDriverId",
                principalSchema: "identity",
                principalTable: "Drivers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Enterprises_EnterpriseId",
                schema: "identity",
                table: "Vehicles",
                column: "EnterpriseId",
                principalSchema: "identity",
                principalTable: "Enterprises",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DriverVehicle_Vehicles_VehiclesId",
                schema: "identity",
                table: "DriverVehicle");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Brands_BrandId",
                schema: "identity",
                table: "Vehicles");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Drivers_CurrentDriverId",
                schema: "identity",
                table: "Vehicles");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Enterprises_EnterpriseId",
                schema: "identity",
                table: "Vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehicles",
                schema: "identity",
                table: "Vehicles");

            migrationBuilder.RenameTable(
                name: "Vehicles",
                schema: "identity",
                newName: "vehicles",
                newSchema: "identity");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicles_EnterpriseId",
                schema: "identity",
                table: "vehicles",
                newName: "IX_vehicles_EnterpriseId");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicles_CurrentDriverId",
                schema: "identity",
                table: "vehicles",
                newName: "IX_vehicles_CurrentDriverId");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicles_BrandId",
                schema: "identity",
                table: "vehicles",
                newName: "IX_vehicles_BrandId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_vehicles",
                schema: "identity",
                table: "vehicles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DriverVehicle_vehicles_VehiclesId",
                schema: "identity",
                table: "DriverVehicle",
                column: "VehiclesId",
                principalSchema: "identity",
                principalTable: "vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_vehicles_Brands_BrandId",
                schema: "identity",
                table: "vehicles",
                column: "BrandId",
                principalSchema: "identity",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_vehicles_Drivers_CurrentDriverId",
                schema: "identity",
                table: "vehicles",
                column: "CurrentDriverId",
                principalSchema: "identity",
                principalTable: "Drivers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_vehicles_Enterprises_EnterpriseId",
                schema: "identity",
                table: "vehicles",
                column: "EnterpriseId",
                principalSchema: "identity",
                principalTable: "Enterprises",
                principalColumn: "Id");
        }
    }
}
