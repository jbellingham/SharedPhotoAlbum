using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SharedPhotoAlbum.Infrastructure.Persistence.Migrations
{
    public partial class NullableLastModififiedBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "LastModifiedBy",
                table: "StoredMedia",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "LastModifiedBy",
                table: "Posts",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "LastModifiedBy",
                table: "Feeds",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "LastModifiedBy",
                table: "Comments",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "LastModifiedBy",
                table: "StoredMedia",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "LastModifiedBy",
                table: "Posts",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "LastModifiedBy",
                table: "Feeds",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "LastModifiedBy",
                table: "Comments",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);
        }
    }
}
