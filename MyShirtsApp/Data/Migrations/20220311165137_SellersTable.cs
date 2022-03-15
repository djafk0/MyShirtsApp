namespace MyShirtsApp.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class SellersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shirts_Sizes_SizeId",
                table: "Shirts");

            migrationBuilder.AddColumn<int>(
                name: "SellerId",
                table: "Shirts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Sellers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sellers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sellers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shirts_SellerId",
                table: "Shirts",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sellers_UserId",
                table: "Sellers",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Shirts_Sellers_SellerId",
                table: "Shirts",
                column: "SellerId",
                principalTable: "Sellers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Shirts_Sizes_SizeId",
                table: "Shirts",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shirts_Sellers_SellerId",
                table: "Shirts");

            migrationBuilder.DropForeignKey(
                name: "FK_Shirts_Sizes_SizeId",
                table: "Shirts");

            migrationBuilder.DropTable(
                name: "Sellers");

            migrationBuilder.DropIndex(
                name: "IX_Shirts_SellerId",
                table: "Shirts");

            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "Shirts");

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
