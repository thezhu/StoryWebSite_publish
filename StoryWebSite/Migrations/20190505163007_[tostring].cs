using Microsoft.EntityFrameworkCore.Migrations;

namespace StoryWebSite.Migrations
{
    public partial class tostring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Order",
                table: "TextBlock",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Order",
                table: "ImageBlock",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Order",
                table: "TextBlock",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Order",
                table: "ImageBlock",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
