using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class _1151 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_platforms_of_aggregator_Slug",
                table: "platforms_of_aggregator");

            migrationBuilder.DropColumn(
                name: "CanonicalName",
                table: "platforms_of_aggregator");

            migrationBuilder.RenameColumn(
                name: "Slug",
                table: "platforms_of_aggregator",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "SystemCode",
                table: "platforms_of_aggregator",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HtmlContent",
                table: "platform_of_aggregator_localizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SeoDataId",
                table: "platform_of_aggregator_localizations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlPicture",
                table: "platform_of_aggregator_localizations",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_platforms_of_aggregator_SystemCode",
                table: "platforms_of_aggregator",
                column: "SystemCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_platform_of_aggregator_localizations_SeoDataId",
                table: "platform_of_aggregator_localizations",
                column: "SeoDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_platform_of_aggregator_localizations_SeoData_SeoDataId",
                table: "platform_of_aggregator_localizations",
                column: "SeoDataId",
                principalTable: "SeoData",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_platform_of_aggregator_localizations_SeoData_SeoDataId",
                table: "platform_of_aggregator_localizations");

            migrationBuilder.DropIndex(
                name: "IX_platforms_of_aggregator_SystemCode",
                table: "platforms_of_aggregator");

            migrationBuilder.DropIndex(
                name: "IX_platform_of_aggregator_localizations_SeoDataId",
                table: "platform_of_aggregator_localizations");

            migrationBuilder.DropColumn(
                name: "SystemCode",
                table: "platforms_of_aggregator");

            migrationBuilder.DropColumn(
                name: "HtmlContent",
                table: "platform_of_aggregator_localizations");

            migrationBuilder.DropColumn(
                name: "SeoDataId",
                table: "platform_of_aggregator_localizations");

            migrationBuilder.DropColumn(
                name: "UrlPicture",
                table: "platform_of_aggregator_localizations");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "platforms_of_aggregator",
                newName: "Slug");

            migrationBuilder.AddColumn<string>(
                name: "CanonicalName",
                table: "platforms_of_aggregator",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_platforms_of_aggregator_Slug",
                table: "platforms_of_aggregator",
                column: "Slug",
                unique: true);
        }
    }
}
