using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class RenameIconPathToIconKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconPath",
                table: "languages_of_aggregator");

            migrationBuilder.AddColumn<string>(
                name: "IconKey",
                table: "languages_of_aggregator",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconKey",
                table: "languages_of_aggregator");

            migrationBuilder.AddColumn<string>(
                name: "IconPath",
                table: "languages_of_aggregator",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: true);
        }
    }
}
