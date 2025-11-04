using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditCustomerBasket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "CustomerBaskets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CustomerBaskets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "CustomerBaskets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "CustomerBaskets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "CustomerBaskets",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerBaskets_LastModifiedBy",
                table: "CustomerBaskets",
                column: "LastModifiedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerBaskets_AspNetUsers_LastModifiedBy",
                table: "CustomerBaskets",
                column: "LastModifiedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerBaskets_AspNetUsers_LastModifiedBy",
                table: "CustomerBaskets");

            migrationBuilder.DropIndex(
                name: "IX_CustomerBaskets_LastModifiedBy",
                table: "CustomerBaskets");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "CustomerBaskets");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CustomerBaskets");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "CustomerBaskets");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "CustomerBaskets");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "CustomerBaskets");
        }
    }
}
