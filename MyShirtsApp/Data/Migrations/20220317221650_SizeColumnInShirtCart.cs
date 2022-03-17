using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyShirtsApp.Data.Migrations
{
    public partial class SizeColumnInShirtCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "ShirtCarts",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "ShirtCarts");
        }
    }
}
