namespace MyShirtsApp.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class CountInShirtCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "ShirtCarts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "ShirtCarts");
        }
    }
}
