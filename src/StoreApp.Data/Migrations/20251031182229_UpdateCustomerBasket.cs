using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCustomerBasket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "CustomerBaskets",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CustomerBaskets");
        }
    }
}
