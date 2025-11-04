using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAddressTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Addresss",
                newName: "FirstName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Addresss",
                newName: "FirstName");
        }
    }
}
