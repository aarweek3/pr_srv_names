using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddSeoAndHtmlToLicenseTypeLocalizations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "license_type_of_aggregator_localizations",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "HtmlContent",
                table: "license_type_of_aggregator_localizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SeoDataId",
                table: "license_type_of_aggregator_localizations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlPicture",
                table: "license_type_of_aggregator_localizations",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_license_type_of_aggregator_localizations_SeoDataId",
                table: "license_type_of_aggregator_localizations",
                column: "SeoDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_license_type_of_aggregator_localizations_SeoData_SeoDataId",
                table: "license_type_of_aggregator_localizations",
                column: "SeoDataId",
                principalTable: "SeoData",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_license_type_of_aggregator_localizations_SeoData_SeoDataId",
                table: "license_type_of_aggregator_localizations");

            migrationBuilder.DropIndex(
                name: "IX_license_type_of_aggregator_localizations_SeoDataId",
                table: "license_type_of_aggregator_localizations");

            migrationBuilder.DropColumn(
                name: "HtmlContent",
                table: "license_type_of_aggregator_localizations");

            migrationBuilder.DropColumn(
                name: "SeoDataId",
                table: "license_type_of_aggregator_localizations");

            migrationBuilder.DropColumn(
                name: "UrlPicture",
                table: "license_type_of_aggregator_localizations");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "license_type_of_aggregator_localizations",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);
        }
    }
}
