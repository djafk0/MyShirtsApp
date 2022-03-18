namespace MyShirtsApp.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ModifiedCompositeKeyShirtCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ShirtCarts",
                table: "ShirtCarts");

            migrationBuilder.RenameColumn(
                name: "Size",
                table: "ShirtCarts",
                newName: "SizeName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShirtCarts",
                table: "ShirtCarts",
                columns: new[] { "ShirtId", "CartId", "SizeName" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ShirtCarts",
                table: "ShirtCarts");

            migrationBuilder.RenameColumn(
                name: "SizeName",
                table: "ShirtCarts",
                newName: "Size");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShirtCarts",
                table: "ShirtCarts",
                columns: new[] { "ShirtId", "CartId" });
        }
    }
}
