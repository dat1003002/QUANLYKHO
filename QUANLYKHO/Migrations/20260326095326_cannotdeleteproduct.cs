using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QUANLYKHO.Migrations
{
    /// <inheritdoc />
    public partial class cannotdeleteproduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryCheckDetails_Products_ProductId",
                table: "InventoryCheckDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Users_LastImportById",
                table: "Products");

            migrationBuilder.AddColumn<bool>(
                name: "CanDeleteProduct",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryCheckDetails_Products_ProductId",
                table: "InventoryCheckDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Users_LastImportById",
                table: "Products",
                column: "LastImportById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryCheckDetails_Products_ProductId",
                table: "InventoryCheckDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Users_LastImportById",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CanDeleteProduct",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryCheckDetails_Products_ProductId",
                table: "InventoryCheckDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Users_LastImportById",
                table: "Products",
                column: "LastImportById",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
