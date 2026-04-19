using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class _1129 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_category_of_aggregator_localizations_categories_of_aggregat~",
                table: "category_of_aggregator_localizations");

            migrationBuilder.DropForeignKey(
                name: "FK_developer_of_aggregator_localizations_developer_of_aggregat~",
                table: "developer_of_aggregator_localizations");

            migrationBuilder.DropForeignKey(
                name: "FK_download_link_of_aggregator_localizations_download_links_of~",
                table: "download_link_of_aggregator_localizations");

            migrationBuilder.DropForeignKey(
                name: "FK_download_logs_of_aggregator_versions_of_aggregator_VersionO~",
                table: "download_logs_of_aggregator");

            migrationBuilder.DropForeignKey(
                name: "FK_license_type_of_aggregator_localizations_license_types_of_a~",
                table: "license_type_of_aggregator_localizations");

            migrationBuilder.DropForeignKey(
                name: "FK_platform_of_aggregator_localizations_platforms_of_aggregato~",
                table: "platform_of_aggregator_localizations");

            migrationBuilder.DropForeignKey(
                name: "FK_programs_of_aggregator_localizations_programs_of_aggregator~",
                table: "programs_of_aggregator_localizations");

            migrationBuilder.DropForeignKey(
                name: "FK_screenshot_of_aggregator_localizations_screenshots_of_aggre~",
                table: "screenshot_of_aggregator_localizations");

            migrationBuilder.DropForeignKey(
                name: "FK_version_of_aggregator_localizations_versions_of_aggregator_~",
                table: "version_of_aggregator_localizations");

            migrationBuilder.DropForeignKey(
                name: "FK_video_of_aggregator_localizations_videos_of_aggregator_Vide~",
                table: "video_of_aggregator_localizations");

            migrationBuilder.AlterColumn<int>(
                name: "VideoOfAggregatorId",
                table: "video_of_aggregator_localizations",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "VersionOfAggregatorId",
                table: "version_of_aggregator_localizations",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "ScreenshotOfAggregatorId",
                table: "screenshot_of_aggregator_localizations",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "ProgramOfAggregatorId",
                table: "programs_of_aggregator_localizations",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "ProgramOfAggregatorId",
                table: "program_platforms_of_aggregator",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "PlatformOfAggregatorId",
                table: "program_platforms_of_aggregator",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "PlatformOfAggregatorId",
                table: "platform_of_aggregator_localizations",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "LicenseTypeOfAggregatorId",
                table: "license_type_of_aggregator_localizations",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "IconPath",
                table: "languages_of_aggregator",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "VersionOfAggregatorId",
                table: "download_logs_of_aggregator",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "DownloadLinkOfAggregatorId",
                table: "download_link_of_aggregator_localizations",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "DeveloperOfAggregatorId",
                table: "developer_of_aggregator_localizations",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryOfAggregatorId",
                table: "category_of_aggregator_localizations",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "tags_of_aggregator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags_of_aggregator", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "program_tags_of_aggregator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProgramOfAggregatorId = table.Column<int>(type: "integer", nullable: true),
                    TagOfAggregatorId = table.Column<int>(type: "integer", nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsMain = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_program_tags_of_aggregator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_program_tags_of_aggregator_programs_of_aggregator_ProgramOf~",
                        column: x => x.ProgramOfAggregatorId,
                        principalTable: "programs_of_aggregator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_program_tags_of_aggregator_tags_of_aggregator_TagOfAggregat~",
                        column: x => x.TagOfAggregatorId,
                        principalTable: "tags_of_aggregator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tag_of_aggregator_localizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TagOfAggregatorId = table.Column<int>(type: "integer", nullable: true),
                    LanguageOfAggregatorId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tag_of_aggregator_localizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tag_of_aggregator_localizations_languages_of_aggregator_Lan~",
                        column: x => x.LanguageOfAggregatorId,
                        principalTable: "languages_of_aggregator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tag_of_aggregator_localizations_tags_of_aggregator_TagOfAgg~",
                        column: x => x.TagOfAggregatorId,
                        principalTable: "tags_of_aggregator",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_program_tags_of_aggregator_CreatedAt",
                table: "program_tags_of_aggregator",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_program_tags_of_aggregator_ProgramOfAggregatorId_TagOfAggre~",
                table: "program_tags_of_aggregator",
                columns: new[] { "ProgramOfAggregatorId", "TagOfAggregatorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_program_tags_of_aggregator_TagOfAggregatorId",
                table: "program_tags_of_aggregator",
                column: "TagOfAggregatorId");

            migrationBuilder.CreateIndex(
                name: "IX_program_tags_of_aggregator_UpdatedAt",
                table: "program_tags_of_aggregator",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_tag_of_aggregator_localizations_CreatedAt",
                table: "tag_of_aggregator_localizations",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_tag_of_aggregator_localizations_LanguageOfAggregatorId",
                table: "tag_of_aggregator_localizations",
                column: "LanguageOfAggregatorId");

            migrationBuilder.CreateIndex(
                name: "IX_tag_of_aggregator_localizations_TagOfAggregatorId_LanguageO~",
                table: "tag_of_aggregator_localizations",
                columns: new[] { "TagOfAggregatorId", "LanguageOfAggregatorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tag_of_aggregator_localizations_UpdatedAt",
                table: "tag_of_aggregator_localizations",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_tags_of_aggregator_CreatedAt",
                table: "tags_of_aggregator",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_tags_of_aggregator_IsDeleted",
                table: "tags_of_aggregator",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_tags_of_aggregator_Slug",
                table: "tags_of_aggregator",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tags_of_aggregator_UpdatedAt",
                table: "tags_of_aggregator",
                column: "UpdatedAt");

            migrationBuilder.AddForeignKey(
                name: "FK_category_of_aggregator_localizations_categories_of_aggregat~",
                table: "category_of_aggregator_localizations",
                column: "CategoryOfAggregatorId",
                principalTable: "categories_of_aggregator",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_developer_of_aggregator_localizations_developer_of_aggregat~",
                table: "developer_of_aggregator_localizations",
                column: "DeveloperOfAggregatorId",
                principalTable: "developer_of_aggregators",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_download_link_of_aggregator_localizations_download_links_of~",
                table: "download_link_of_aggregator_localizations",
                column: "DownloadLinkOfAggregatorId",
                principalTable: "download_links_of_aggregator",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_download_logs_of_aggregator_versions_of_aggregator_VersionO~",
                table: "download_logs_of_aggregator",
                column: "VersionOfAggregatorId",
                principalTable: "versions_of_aggregator",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_license_type_of_aggregator_localizations_license_types_of_a~",
                table: "license_type_of_aggregator_localizations",
                column: "LicenseTypeOfAggregatorId",
                principalTable: "license_types_of_aggregator",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_platform_of_aggregator_localizations_platforms_of_aggregato~",
                table: "platform_of_aggregator_localizations",
                column: "PlatformOfAggregatorId",
                principalTable: "platforms_of_aggregator",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_programs_of_aggregator_localizations_programs_of_aggregator~",
                table: "programs_of_aggregator_localizations",
                column: "ProgramOfAggregatorId",
                principalTable: "programs_of_aggregator",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_screenshot_of_aggregator_localizations_screenshots_of_aggre~",
                table: "screenshot_of_aggregator_localizations",
                column: "ScreenshotOfAggregatorId",
                principalTable: "screenshots_of_aggregator",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_version_of_aggregator_localizations_versions_of_aggregator_~",
                table: "version_of_aggregator_localizations",
                column: "VersionOfAggregatorId",
                principalTable: "versions_of_aggregator",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_video_of_aggregator_localizations_videos_of_aggregator_Vide~",
                table: "video_of_aggregator_localizations",
                column: "VideoOfAggregatorId",
                principalTable: "videos_of_aggregator",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_category_of_aggregator_localizations_categories_of_aggregat~",
                table: "category_of_aggregator_localizations");

            migrationBuilder.DropForeignKey(
                name: "FK_developer_of_aggregator_localizations_developer_of_aggregat~",
                table: "developer_of_aggregator_localizations");

            migrationBuilder.DropForeignKey(
                name: "FK_download_link_of_aggregator_localizations_download_links_of~",
                table: "download_link_of_aggregator_localizations");

            migrationBuilder.DropForeignKey(
                name: "FK_download_logs_of_aggregator_versions_of_aggregator_VersionO~",
                table: "download_logs_of_aggregator");

            migrationBuilder.DropForeignKey(
                name: "FK_license_type_of_aggregator_localizations_license_types_of_a~",
                table: "license_type_of_aggregator_localizations");

            migrationBuilder.DropForeignKey(
                name: "FK_platform_of_aggregator_localizations_platforms_of_aggregato~",
                table: "platform_of_aggregator_localizations");

            migrationBuilder.DropForeignKey(
                name: "FK_programs_of_aggregator_localizations_programs_of_aggregator~",
                table: "programs_of_aggregator_localizations");

            migrationBuilder.DropForeignKey(
                name: "FK_screenshot_of_aggregator_localizations_screenshots_of_aggre~",
                table: "screenshot_of_aggregator_localizations");

            migrationBuilder.DropForeignKey(
                name: "FK_version_of_aggregator_localizations_versions_of_aggregator_~",
                table: "version_of_aggregator_localizations");

            migrationBuilder.DropForeignKey(
                name: "FK_video_of_aggregator_localizations_videos_of_aggregator_Vide~",
                table: "video_of_aggregator_localizations");

            migrationBuilder.DropTable(
                name: "program_tags_of_aggregator");

            migrationBuilder.DropTable(
                name: "tag_of_aggregator_localizations");

            migrationBuilder.DropTable(
                name: "tags_of_aggregator");

            migrationBuilder.DropColumn(
                name: "IconPath",
                table: "languages_of_aggregator");

            migrationBuilder.AlterColumn<int>(
                name: "VideoOfAggregatorId",
                table: "video_of_aggregator_localizations",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "VersionOfAggregatorId",
                table: "version_of_aggregator_localizations",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ScreenshotOfAggregatorId",
                table: "screenshot_of_aggregator_localizations",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProgramOfAggregatorId",
                table: "programs_of_aggregator_localizations",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProgramOfAggregatorId",
                table: "program_platforms_of_aggregator",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PlatformOfAggregatorId",
                table: "program_platforms_of_aggregator",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PlatformOfAggregatorId",
                table: "platform_of_aggregator_localizations",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LicenseTypeOfAggregatorId",
                table: "license_type_of_aggregator_localizations",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "VersionOfAggregatorId",
                table: "download_logs_of_aggregator",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DownloadLinkOfAggregatorId",
                table: "download_link_of_aggregator_localizations",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DeveloperOfAggregatorId",
                table: "developer_of_aggregator_localizations",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryOfAggregatorId",
                table: "category_of_aggregator_localizations",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_category_of_aggregator_localizations_categories_of_aggregat~",
                table: "category_of_aggregator_localizations",
                column: "CategoryOfAggregatorId",
                principalTable: "categories_of_aggregator",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_developer_of_aggregator_localizations_developer_of_aggregat~",
                table: "developer_of_aggregator_localizations",
                column: "DeveloperOfAggregatorId",
                principalTable: "developer_of_aggregators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_download_link_of_aggregator_localizations_download_links_of~",
                table: "download_link_of_aggregator_localizations",
                column: "DownloadLinkOfAggregatorId",
                principalTable: "download_links_of_aggregator",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_download_logs_of_aggregator_versions_of_aggregator_VersionO~",
                table: "download_logs_of_aggregator",
                column: "VersionOfAggregatorId",
                principalTable: "versions_of_aggregator",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_license_type_of_aggregator_localizations_license_types_of_a~",
                table: "license_type_of_aggregator_localizations",
                column: "LicenseTypeOfAggregatorId",
                principalTable: "license_types_of_aggregator",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_platform_of_aggregator_localizations_platforms_of_aggregato~",
                table: "platform_of_aggregator_localizations",
                column: "PlatformOfAggregatorId",
                principalTable: "platforms_of_aggregator",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_programs_of_aggregator_localizations_programs_of_aggregator~",
                table: "programs_of_aggregator_localizations",
                column: "ProgramOfAggregatorId",
                principalTable: "programs_of_aggregator",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_screenshot_of_aggregator_localizations_screenshots_of_aggre~",
                table: "screenshot_of_aggregator_localizations",
                column: "ScreenshotOfAggregatorId",
                principalTable: "screenshots_of_aggregator",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_version_of_aggregator_localizations_versions_of_aggregator_~",
                table: "version_of_aggregator_localizations",
                column: "VersionOfAggregatorId",
                principalTable: "versions_of_aggregator",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_video_of_aggregator_localizations_videos_of_aggregator_Vide~",
                table: "video_of_aggregator_localizations",
                column: "VideoOfAggregatorId",
                principalTable: "videos_of_aggregator",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
