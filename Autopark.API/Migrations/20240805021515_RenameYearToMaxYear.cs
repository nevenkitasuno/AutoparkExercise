using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autopark.API.Migrations
{
    /// <inheritdoc />
    public partial class RenameYearToMaxYear : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Year",
                table: "Vehicles",
                newName: "ManufactureYear");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ManufactureYear",
                table: "Vehicles",
                newName: "Year");
        }
    }
}
