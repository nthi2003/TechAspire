using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechAspire.Migrations
{
    /// <inheritdoc />
    public partial class db02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductModels_Brands_BrandId",
                table: "ProductModels");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductModels_Categories_CategoryId",
                table: "ProductModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductModels",
                table: "ProductModels");

            migrationBuilder.RenameTable(
                name: "ProductModels",
                newName: "Products");

            migrationBuilder.RenameIndex(
                name: "IX_ProductModels_CategoryId",
                table: "Products",
                newName: "IX_Products_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductModels_BrandId",
                table: "Products",
                newName: "IX_Products_BrandId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "ProductModels");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CategoryId",
                table: "ProductModels",
                newName: "IX_ProductModels_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_BrandId",
                table: "ProductModels",
                newName: "IX_ProductModels_BrandId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductModels",
                table: "ProductModels",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductModels_Brands_BrandId",
                table: "ProductModels",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductModels_Categories_CategoryId",
                table: "ProductModels",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
