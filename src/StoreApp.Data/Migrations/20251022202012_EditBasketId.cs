using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditBasketId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1️⃣ ستون جدید اضافه کنید
            migrationBuilder.AddColumn<string>(
                name: "NewId",
                table: "CustomerBaskets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            // 2️⃣ حذف Foreign Key
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerBasketItems_CustomerBaskets_BasketId",
                table: "CustomerBasketItems");

            // 3️⃣ حذف Primary Key قدیمی
            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerBaskets",
                table: "CustomerBaskets");

            // 4️⃣ حذف ستون قدیمی Id
            migrationBuilder.DropColumn(
                name: "Id",
                table: "CustomerBaskets");

            // 5️⃣ Rename ستون جدید به Id
            migrationBuilder.RenameColumn(
                name: "NewId",
                table: "CustomerBaskets",
                newName: "Id");

            // 6️⃣ اضافه کردن Primary Key جدید
            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerBaskets",
                table: "CustomerBaskets",
                column: "Id");

            // 7️⃣ تغییر نوع BasketId در CustomerBasketItems
            migrationBuilder.AlterColumn<string>(
                name: "BasketId",
                table: "CustomerBasketItems",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            // 8️⃣ اضافه کردن Foreign Key جدید
            migrationBuilder.AddForeignKey(
                name: "FK_CustomerBasketItems_CustomerBaskets_BasketId",
                table: "CustomerBasketItems",
                column: "BasketId",
                principalTable: "CustomerBaskets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // معکوس تغییرات برای Rollback
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerBasketItems_CustomerBaskets_BasketId",
                table: "CustomerBasketItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerBaskets",
                table: "CustomerBaskets");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CustomerBaskets");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CustomerBaskets",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "BasketId",
                table: "CustomerBasketItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerBaskets",
                table: "CustomerBaskets",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerBasketItems_CustomerBaskets_BasketId",
                table: "CustomerBasketItems",
                column: "BasketId",
                principalTable: "CustomerBaskets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
