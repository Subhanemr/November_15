using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _15_11_23.Migrations
{
    public partial class ImageurlTableAddColum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_Slides_SlideId",
                table: "ProductImages");

            migrationBuilder.DropIndex(
                name: "IX_ProductImages_SlideId",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "SlideId",
                table: "ProductImages");

            migrationBuilder.AddColumn<string>(
                name: "ImgUrl",
                table: "Slides",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgUrl",
                table: "Slides");

            migrationBuilder.AddColumn<int>(
                name: "SlideId",
                table: "ProductImages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_SlideId",
                table: "ProductImages",
                column: "SlideId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_Slides_SlideId",
                table: "ProductImages",
                column: "SlideId",
                principalTable: "Slides",
                principalColumn: "Id");
        }
    }
}