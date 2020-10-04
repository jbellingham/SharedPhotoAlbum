using Microsoft.EntityFrameworkCore.Migrations;

namespace SharedPhotoAlbum.Infrastructure.Persistence.Migrations
{
    public partial class StoredMediaFileValueObject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "StoredMedia");

            migrationBuilder.DropColumn(
                name: "MediaType",
                table: "StoredMedia");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "StoredMedia");

            migrationBuilder.AddColumn<string>(
                name: "File_DataUrl",
                table: "StoredMedia",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "File_FileType",
                table: "StoredMedia",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "File_MimeType",
                table: "StoredMedia",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "File_DataUrl",
                table: "StoredMedia");

            migrationBuilder.DropColumn(
                name: "File_FileType",
                table: "StoredMedia");

            migrationBuilder.DropColumn(
                name: "File_MimeType",
                table: "StoredMedia");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "StoredMedia",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MediaType",
                table: "StoredMedia",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "StoredMedia",
                type: "text",
                nullable: true);
        }
    }
}
