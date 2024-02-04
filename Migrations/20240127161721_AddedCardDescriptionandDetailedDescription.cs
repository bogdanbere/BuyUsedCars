using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuyUsedCars.Migrations
{
    /// <inheritdoc />
    public partial class AddedCardDescriptionandDetailedDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Cars",
                newName: "DetailedDescription");

            migrationBuilder.AddColumn<string>(
                name: "CardDescription",
                table: "Cars",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardDescription",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "DetailedDescription",
                table: "Cars",
                newName: "Description");
        }
    }
}
