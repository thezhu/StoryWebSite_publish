using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StoryWebSite.Migrations
{
    public partial class post : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "ImageBlock");

            migrationBuilder.AddColumn<string>(
                name: "ImageCaption",
                table: "ImageBlock",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "ImageBlock",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "ImageBlock",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "MyImage",
                table: "ImageBlock",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageCaption",
                table: "ImageBlock");

            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "ImageBlock");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "ImageBlock");

            migrationBuilder.DropColumn(
                name: "MyImage",
                table: "ImageBlock");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "ImageBlock",
                nullable: false,
                defaultValue: "");
        }
    }
}
