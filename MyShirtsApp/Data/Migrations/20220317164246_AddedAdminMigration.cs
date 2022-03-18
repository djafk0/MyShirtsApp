namespace MyShirtsApp.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddedAdminMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Shirts",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Shirts_UserId",
                table: "Shirts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shirts_AspNetUsers_UserId",
                table: "Shirts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shirts_AspNetUsers_UserId",
                table: "Shirts");

            migrationBuilder.DropIndex(
                name: "IX_Shirts_UserId",
                table: "Shirts");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Shirts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
