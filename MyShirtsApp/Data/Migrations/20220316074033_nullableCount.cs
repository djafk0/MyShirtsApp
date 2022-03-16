using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyShirtsApp.Data.Migrations
{
    public partial class nullableCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shirts_Carts_CartId",
                table: "Shirts");

            migrationBuilder.DropIndex(
                name: "IX_Shirts_CartId",
                table: "Shirts");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "Shirts");

            migrationBuilder.AlterColumn<int>(
                name: "Count",
                table: "ShirtSizes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Count",
                table: "ShirtSizes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CartId",
                table: "Shirts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shirts_CartId",
                table: "Shirts",
                column: "CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shirts_Carts_CartId",
                table: "Shirts",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id");
        }
    }
}
