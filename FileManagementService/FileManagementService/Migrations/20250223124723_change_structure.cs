using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileManagementService.Migrations
{
    /// <inheritdoc />
    public partial class change_structure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Files");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Files",
                newName: "StoragePath");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Files",
                newName: "UploadedAt");

            migrationBuilder.AlterColumn<long>(
                name: "FileSize",
                table: "Files",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Files",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileType",
                table: "Files",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsConverted",
                table: "Files",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "FileType",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "IsConverted",
                table: "Files");

            migrationBuilder.RenameColumn(
                name: "UploadedAt",
                table: "Files",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "StoragePath",
                table: "Files",
                newName: "Name");

            migrationBuilder.AlterColumn<int>(
                name: "FileSize",
                table: "Files",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<byte[]>(
                name: "Content",
                table: "Files",
                type: "bytea",
                nullable: true);
        }
    }
}
