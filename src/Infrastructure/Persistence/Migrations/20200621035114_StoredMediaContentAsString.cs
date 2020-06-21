using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SharedPhotoAlbum.Infrastructure.Persistence.Migrations
{
    public partial class StoredMediaContentAsString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "StoredMedia",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "bytea",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Content",
                table: "StoredMedia",
                type: "bytea",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
