using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechAspire.Migrations
{
    /// <inheritdoc />
    public partial class dbint05 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Decription",
                table: "ProductModels");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ProductModels");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "ProductModels");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Decription",
                table: "ProductModels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ProductModels",
                type: "NUMERIC(15,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "ProductModels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
