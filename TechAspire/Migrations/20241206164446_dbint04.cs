using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechAspire.Migrations
{
    /// <inheritdoc />
    public partial class dbint04 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_productModels_Brands_BrandId",
                table: "productModels");

            migrationBuilder.DropForeignKey(
                name: "FK_productModels_Categories_CategoryId",
                table: "productModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_productModels",
                table: "productModels");

            migrationBuilder.RenameTable(
                name: "productModels",
                newName: "ProductModels");

            migrationBuilder.RenameIndex(
                name: "IX_productModels_CategoryId",
                table: "ProductModels",
                newName: "IX_ProductModels_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_productModels_BrandId",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                newName: "productModels");

            migrationBuilder.RenameIndex(
                name: "IX_ProductModels_CategoryId",
                table: "productModels",
                newName: "IX_productModels_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductModels_BrandId",
                table: "productModels",
                newName: "IX_productModels_BrandId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_productModels",
                table: "productModels",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_productModels_Brands_BrandId",
                table: "productModels",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_productModels_Categories_CategoryId",
                table: "productModels",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
