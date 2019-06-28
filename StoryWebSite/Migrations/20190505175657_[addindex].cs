using Microsoft.EntityFrameworkCore.Migrations;

namespace StoryWebSite.Migrations
{
    public partial class addindex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "ImageBlock");

            migrationBuilder.AddColumn<int>(
                name: "TextBlockIndex",
                table: "TextBlock",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ImageBlockIndex",
                table: "ImageBlock",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TextBlockIndex",
                table: "TextBlock");

            migrationBuilder.DropColumn(
                name: "ImageBlockIndex",
                table: "ImageBlock");

            migrationBuilder.AddColumn<string>(
                name: "Order",
                table: "ImageBlock",
                nullable: true);
        }
    }
}
