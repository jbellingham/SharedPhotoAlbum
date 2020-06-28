using Microsoft.EntityFrameworkCore.Migrations;

namespace SharedPhotoAlbum.Infrastructure.Persistence.Migrations
{
    public partial class FeedOwnerNonNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feeds_AspNetUsers_OwnerId",
                table: "Feeds");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Feeds",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Feeds_AspNetUsers_OwnerId",
                table: "Feeds",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feeds_AspNetUsers_OwnerId",
                table: "Feeds");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Feeds",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_Feeds_AspNetUsers_OwnerId",
                table: "Feeds",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
