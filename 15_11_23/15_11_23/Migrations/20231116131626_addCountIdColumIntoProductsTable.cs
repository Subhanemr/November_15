using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _15_11_23.Migrations
{
    public partial class addCountIdColumIntoProductsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountId",
                table: "Products");
        }
    }
}
