using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileManagementService.Migrations
{
    /// <inheritdoc />
    public partial class extensions2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsConverted",
                table: "Files");

            migrationBuilder.RenameColumn(
                name: "Format",
                table: "Extensions",
                newName: "MediaType");

            migrationBuilder.RenameColumn(
                name: "ExtensionName",
                table: "Extensions",
                newName: "FilenameExtension");

            migrationBuilder.CreateTable(
                name: "ExtensionConversion",
                columns: table => new
                {
                    SourceExtensionId = table.Column<int>(type: "integer", nullable: false),
                    TargetExtensionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtensionConversion", x => new { x.SourceExtensionId, x.TargetExtensionId });
                    table.ForeignKey(
                        name: "FK_ExtensionConversion_Extensions_SourceExtensionId",
                        column: x => x.SourceExtensionId,
                        principalTable: "Extensions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExtensionConversion_Extensions_TargetExtensionId",
                        column: x => x.TargetExtensionId,
                        principalTable: "Extensions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExtensionConversion_TargetExtensionId",
                table: "ExtensionConversion",
                column: "TargetExtensionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExtensionConversion");

            migrationBuilder.RenameColumn(
                name: "MediaType",
                table: "Extensions",
                newName: "Format");

            migrationBuilder.RenameColumn(
                name: "FilenameExtension",
                table: "Extensions",
                newName: "ExtensionName");

            migrationBuilder.AddColumn<bool>(
                name: "IsConverted",
                table: "Files",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
