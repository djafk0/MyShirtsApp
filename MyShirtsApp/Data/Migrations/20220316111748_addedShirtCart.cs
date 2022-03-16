using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyShirtsApp.Data.Migrations
{
    public partial class addedShirtCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShirtCarts_Carts_CartId",
                table: "ShirtCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShirtCarts_Shirts_ShirtId",
                table: "ShirtCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShirtSizes_Shirts_ShirtId",
                table: "ShirtSizes");

            migrationBuilder.DropForeignKey(
                name: "FK_ShirtSizes_Sizes_SizeId",
                table: "ShirtSizes");

            migrationBuilder.AddForeignKey(
                name: "FK_ShirtCarts_Carts_CartId",
                table: "ShirtCarts",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShirtCarts_Shirts_ShirtId",
                table: "ShirtCarts",
                column: "ShirtId",
                principalTable: "Shirts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShirtSizes_Shirts_ShirtId",
                table: "ShirtSizes",
                column: "ShirtId",
                principalTable: "Shirts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShirtSizes_Sizes_SizeId",
                table: "ShirtSizes",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShirtCarts_Carts_CartId",
                table: "ShirtCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShirtCarts_Shirts_ShirtId",
                table: "ShirtCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShirtSizes_Shirts_ShirtId",
                table: "ShirtSizes");

            migrationBuilder.DropForeignKey(
                name: "FK_ShirtSizes_Sizes_SizeId",
                table: "ShirtSizes");

            migrationBuilder.AddForeignKey(
                name: "FK_ShirtCarts_Carts_CartId",
                table: "ShirtCarts",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShirtCarts_Shirts_ShirtId",
                table: "ShirtCarts",
                column: "ShirtId",
                principalTable: "Shirts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShirtSizes_Shirts_ShirtId",
                table: "ShirtSizes",
                column: "ShirtId",
                principalTable: "Shirts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShirtSizes_Sizes_SizeId",
                table: "ShirtSizes",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
