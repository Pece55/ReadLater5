using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class ClientPermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookmark_Categories_CategoryId",
                table: "Bookmark");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Bookmark",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookmark_Categories_CategoryId",
                table: "Bookmark",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookmark_Categories_CategoryId",
                table: "Bookmark");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Bookmark",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookmark_Categories_CategoryId",
                table: "Bookmark",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
