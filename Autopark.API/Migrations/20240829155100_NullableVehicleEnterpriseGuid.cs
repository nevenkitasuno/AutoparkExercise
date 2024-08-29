using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autopark.API.Migrations
{
    /// <inheritdoc />
    public partial class NullableVehicleEnterpriseGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vehicles_Enterprises_EnterpriseId",
                table: "vehicles");

            migrationBuilder.AlterColumn<Guid>(
                name: "EnterpriseId",
                table: "vehicles",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_vehicles_Enterprises_EnterpriseId",
                table: "vehicles",
                column: "EnterpriseId",
                principalTable: "Enterprises",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vehicles_Enterprises_EnterpriseId",
                table: "vehicles");

            migrationBuilder.AlterColumn<Guid>(
                name: "EnterpriseId",
                table: "vehicles",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_vehicles_Enterprises_EnterpriseId",
                table: "vehicles",
                column: "EnterpriseId",
                principalTable: "Enterprises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
