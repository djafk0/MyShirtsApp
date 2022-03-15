using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyShirtsApp.Data.Migrations
{
    public partial class addedShirtSizesAndShirtCartsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shirts_Sizes_SizeId",
                table: "Shirts");

            migrationBuilder.DropTable(
                name: "CartShirt");

            migrationBuilder.DropIndex(
                name: "IX_Shirts_SizeId",
                table: "Shirts");

            migrationBuilder.DropColumn(
                name: "SizeId",
                table: "Shirts");

            migrationBuilder.AddColumn<int>(
                name: "CartId",
                table: "Shirts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ShirtCarts",
                columns: table => new
                {
                    ShirtId = table.Column<int>(type: "int", nullable: false),
                    CartId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShirtCarts", x => new { x.ShirtId, x.CartId });
                    table.ForeignKey(
                        name: "FK_ShirtCarts_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShirtCarts_Shirts_ShirtId",
                        column: x => x.ShirtId,
                        principalTable: "Shirts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShirtSizes",
                columns: table => new
                {
                    ShirtId = table.Column<int>(type: "int", nullable: false),
                    SizeId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShirtSizes", x => new { x.ShirtId, x.SizeId });
                    table.ForeignKey(
                        name: "FK_ShirtSizes_Shirts_ShirtId",
                        column: x => x.ShirtId,
                        principalTable: "Shirts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShirtSizes_Sizes_SizeId",
                        column: x => x.SizeId,
                        principalTable: "Sizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shirts_CartId",
                table: "Shirts",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_ShirtCarts_CartId",
                table: "ShirtCarts",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_ShirtSizes_SizeId",
                table: "ShirtSizes",
                column: "SizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shirts_Carts_CartId",
                table: "Shirts",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shirts_Carts_CartId",
                table: "Shirts");

            migrationBuilder.DropTable(
                name: "ShirtCarts");

            migrationBuilder.DropTable(
                name: "ShirtSizes");

            migrationBuilder.DropIndex(
                name: "IX_Shirts_CartId",
                table: "Shirts");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "Shirts");

            migrationBuilder.AddColumn<int>(
                name: "SizeId",
                table: "Shirts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CartShirt",
                columns: table => new
                {
                    CartsId = table.Column<int>(type: "int", nullable: false),
                    ShirtsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartShirt", x => new { x.CartsId, x.ShirtsId });
                    table.ForeignKey(
                        name: "FK_CartShirt_Carts_CartsId",
                        column: x => x.CartsId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartShirt_Shirts_ShirtsId",
                        column: x => x.ShirtsId,
                        principalTable: "Shirts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shirts_SizeId",
                table: "Shirts",
                column: "SizeId");

            migrationBuilder.CreateIndex(
                name: "IX_CartShirt_ShirtsId",
                table: "CartShirt",
                column: "ShirtsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shirts_Sizes_SizeId",
                table: "Shirts",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
