using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPlaceToAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Place",
                table: "Addresss",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Place",
                table: "Addresss");
        }
    }
}
