using Microsoft.EntityFrameworkCore.Migrations;

namespace web.Migrations
{
    public partial class CategorisUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Type",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Type_UserId",
                table: "Type",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Type_AspNetUsers_UserId",
                table: "Type",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Type_AspNetUsers_UserId",
                table: "Type");

            migrationBuilder.DropIndex(
                name: "IX_Type_UserId",
                table: "Type");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Type");
        }
    }
}
