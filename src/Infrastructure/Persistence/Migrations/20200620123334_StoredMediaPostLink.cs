using Microsoft.EntityFrameworkCore.Migrations;

namespace SharedPhotoAlbum.Infrastructure.Persistence.Migrations
{
    public partial class StoredMediaPostLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoredMedia_Posts_PostId",
                table: "StoredMedia");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "StoredMedia",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StoredMedia_Posts_PostId",
                table: "StoredMedia",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoredMedia_Posts_PostId",
                table: "StoredMedia");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "StoredMedia",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_StoredMedia_Posts_PostId",
                table: "StoredMedia",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
