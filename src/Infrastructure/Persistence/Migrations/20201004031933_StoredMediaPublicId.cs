using Microsoft.EntityFrameworkCore.Migrations;

namespace SharedPhotoAlbum.Infrastructure.Persistence.Migrations
{
    public partial class StoredMediaPublicId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "File_PublicId",
                table: "StoredMedia",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "File_PublicId",
                table: "StoredMedia");
        }
    }
}
