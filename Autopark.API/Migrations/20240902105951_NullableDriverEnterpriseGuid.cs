using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autopark.API.Migrations
{
    /// <inheritdoc />
    public partial class NullableDriverEnterpriseGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Enterprises_EnterpriseId",
                table: "Drivers");

            migrationBuilder.AlterColumn<Guid>(
                name: "EnterpriseId",
                table: "Drivers",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Enterprises_EnterpriseId",
                table: "Drivers",
                column: "EnterpriseId",
                principalTable: "Enterprises",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Enterprises_EnterpriseId",
                table: "Drivers");

            migrationBuilder.AlterColumn<Guid>(
                name: "EnterpriseId",
                table: "Drivers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Enterprises_EnterpriseId",
                table: "Drivers",
                column: "EnterpriseId",
                principalTable: "Enterprises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
