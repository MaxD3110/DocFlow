using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FileManagementService.Migrations
{
    /// <inheritdoc />
    public partial class SubtleChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Files");

            migrationBuilder.RenameColumn(
                name: "FileType",
                table: "Files",
                newName: "Name");

            migrationBuilder.AddColumn<int>(
                name: "ExtensionId",
                table: "Files",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Extensions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExtensionName = table.Column<string>(type: "text", nullable: false),
                    Format = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Extensions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Files_ExtensionId",
                table: "Files",
                column: "ExtensionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Extensions_ExtensionId",
                table: "Files",
                column: "ExtensionId",
                principalTable: "Extensions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Extensions_ExtensionId",
                table: "Files");

            migrationBuilder.DropTable(
                name: "Extensions");

            migrationBuilder.DropIndex(
                name: "IX_Files_ExtensionId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "ExtensionId",
                table: "Files");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Files",
                newName: "FileType");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Files",
                type: "text",
                nullable: true);
        }
    }
}
