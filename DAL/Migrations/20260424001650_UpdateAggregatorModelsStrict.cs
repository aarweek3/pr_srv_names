using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAggregatorModelsStrict : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Cleanup: Delete orphans (NULLs or non-existent parents) and duplicates
            
            // Platform Localizations
            migrationBuilder.Sql("DELETE FROM platform_of_aggregator_localizations WHERE \"PlatformOfAggregatorId\" IS NULL OR \"PlatformOfAggregatorId\" NOT IN (SELECT \"Id\" FROM platforms_of_aggregator);");
            migrationBuilder.Sql("DELETE FROM platform_of_aggregator_localizations WHERE \"Id\" NOT IN (SELECT MIN(\"Id\") FROM platform_of_aggregator_localizations GROUP BY \"PlatformOfAggregatorId\", \"LanguageOfAggregatorId\");");
            
            // Category Localizations
            migrationBuilder.Sql("DELETE FROM category_of_aggregator_localizations WHERE \"CategoryOfAggregatorId\" IS NULL OR \"CategoryOfAggregatorId\" NOT IN (SELECT \"Id\" FROM categories_of_aggregator);");
            migrationBuilder.Sql("DELETE FROM category_of_aggregator_localizations WHERE \"Id\" NOT IN (SELECT MIN(\"Id\") FROM category_of_aggregator_localizations GROUP BY \"CategoryOfAggregatorId\", \"LanguageOfAggregatorId\");");

            // Developer Localizations
            migrationBuilder.Sql("DELETE FROM developer_of_aggregator_localizations WHERE \"DeveloperOfAggregatorId\" IS NULL OR \"DeveloperOfAggregatorId\" NOT IN (SELECT \"Id\" FROM developers_of_aggregator);");
            migrationBuilder.Sql("DELETE FROM developer_of_aggregator_localizations WHERE \"Id\" NOT IN (SELECT MIN(\"Id\") FROM developer_of_aggregator_localizations GROUP BY \"DeveloperOfAggregatorId\", \"LanguageOfAggregatorId\");");

            // Tag Localizations
            migrationBuilder.Sql("DELETE FROM tag_of_aggregator_localizations WHERE \"TagOfAggregatorId\" IS NULL OR \"TagOfAggregatorId\" NOT IN (SELECT \"Id\" FROM tags_of_aggregator);");
            migrationBuilder.Sql("DELETE FROM tag_of_aggregator_localizations WHERE \"Id\" NOT IN (SELECT MIN(\"Id\") FROM tag_of_aggregator_localizations GROUP BY \"TagOfAggregatorId\", \"LanguageOfAggregatorId\");");

            // Program Localizations
            migrationBuilder.Sql("DELETE FROM programs_of_aggregator_localizations WHERE \"ProgramOfAggregatorId\" IS NULL OR \"ProgramOfAggregatorId\" NOT IN (SELECT \"Id\" FROM programs_of_aggregator);");
            migrationBuilder.Sql("DELETE FROM programs_of_aggregator_localizations WHERE \"Id\" NOT IN (SELECT MIN(\"Id\") FROM programs_of_aggregator_localizations GROUP BY \"ProgramOfAggregatorId\", \"LanguageOfAggregatorId\");");

            // Version Localizations
            migrationBuilder.Sql("DELETE FROM version_of_aggregator_localizations WHERE \"VersionOfAggregatorId\" IS NULL OR \"VersionOfAggregatorId\" NOT IN (SELECT \"Id\" FROM versions_of_aggregator);");
            migrationBuilder.Sql("DELETE FROM version_of_aggregator_localizations WHERE \"Id\" NOT IN (SELECT MIN(\"Id\") FROM version_of_aggregator_localizations GROUP BY \"VersionOfAggregatorId\", \"LanguageOfAggregatorId\");");

            // Screenshot Localizations
            migrationBuilder.Sql("DELETE FROM screenshot_of_aggregator_localizations WHERE \"ScreenshotOfAggregatorId\" IS NULL OR \"ScreenshotOfAggregatorId\" NOT IN (SELECT \"Id\" FROM screenshots_of_aggregator);");
            migrationBuilder.Sql("DELETE FROM screenshot_of_aggregator_localizations WHERE \"Id\" NOT IN (SELECT MIN(\"Id\") FROM screenshot_of_aggregator_localizations GROUP BY \"ScreenshotOfAggregatorId\", \"LanguageOfAggregatorId\");");

            // Video Localizations
            migrationBuilder.Sql("DELETE FROM video_of_aggregator_localizations WHERE \"VideoOfAggregatorId\" IS NULL OR \"VideoOfAggregatorId\" NOT IN (SELECT \"Id\" FROM videos_of_aggregator);");
            migrationBuilder.Sql("DELETE FROM video_of_aggregator_localizations WHERE \"Id\" NOT IN (SELECT MIN(\"Id\") FROM video_of_aggregator_localizations GROUP BY \"VideoOfAggregatorId\", \"LanguageOfAggregatorId\");");

            // DownloadLink Localizations
            migrationBuilder.Sql("DELETE FROM download_link_of_aggregator_localizations WHERE \"DownloadLinkOfAggregatorId\" IS NULL OR \"DownloadLinkOfAggregatorId\" NOT IN (SELECT \"Id\" FROM download_links_of_aggregator);");
            migrationBuilder.Sql("DELETE FROM download_link_of_aggregator_localizations WHERE \"Id\" NOT IN (SELECT MIN(\"Id\") FROM download_link_of_aggregator_localizations GROUP BY \"DownloadLinkOfAggregatorId\", \"LanguageOfAggregatorId\");");

            // Relationship tables (N:M)
            migrationBuilder.Sql("DELETE FROM program_tags_of_aggregator WHERE \"ProgramOfAggregatorId\" IS NULL OR \"ProgramOfAggregatorId\" NOT IN (SELECT \"Id\" FROM programs_of_aggregator);");
            migrationBuilder.Sql("DELETE FROM program_tags_of_aggregator WHERE \"TagOfAggregatorId\" IS NULL OR \"TagOfAggregatorId\" NOT IN (SELECT \"Id\" FROM tags_of_aggregator);");
            migrationBuilder.Sql("DELETE FROM program_tags_of_aggregator WHERE \"Id\" NOT IN (SELECT MIN(\"Id\") FROM program_tags_of_aggregator GROUP BY \"ProgramOfAggregatorId\", \"TagOfAggregatorId\");");

            migrationBuilder.Sql("DELETE FROM program_platforms_of_aggregator WHERE \"ProgramOfAggregatorId\" IS NULL OR \"ProgramOfAggregatorId\" NOT IN (SELECT \"Id\" FROM programs_of_aggregator);");
            migrationBuilder.Sql("DELETE FROM program_platforms_of_aggregator WHERE \"PlatformOfAggregatorId\" IS NULL OR \"PlatformOfAggregatorId\" NOT IN (SELECT \"Id\" FROM platforms_of_aggregator);");
            migrationBuilder.Sql("DELETE FROM program_platforms_of_aggregator WHERE \"Id\" NOT IN (SELECT MIN(\"Id\") FROM program_platforms_of_aggregator GROUP BY \"ProgramOfAggregatorId\", \"PlatformOfAggregatorId\");");

            // Versions
            migrationBuilder.Sql("DELETE FROM versions_of_aggregator WHERE \"ProgramOfAggregatorId\" IS NULL OR \"ProgramOfAggregatorId\" NOT IN (SELECT \"Id\" FROM programs_of_aggregator);");
            migrationBuilder.Sql("DELETE FROM versions_of_aggregator WHERE \"Id\" NOT IN (SELECT MIN(\"Id\") FROM versions_of_aggregator GROUP BY \"ProgramOfAggregatorId\", \"VersionNumber\");");

            // Market Data
            migrationBuilder.Sql("DELETE FROM program_market_data_of_aggregator WHERE \"ProgramOfAggregatorId\" IS NULL OR \"ProgramOfAggregatorId\" NOT IN (SELECT \"Id\" FROM programs_of_aggregator);");
            migrationBuilder.Sql("DELETE FROM program_market_data_of_aggregator WHERE \"Id\" NOT IN (SELECT MIN(\"Id\") FROM program_market_data_of_aggregator GROUP BY \"ProgramOfAggregatorId\", \"LanguageOfAggregatorId\", \"AggregatorSourceId\");");

            // Drop foreign keys before altering columns
            migrationBuilder.DropForeignKey(
                name: "FK_category_of_aggregator_localizations_categories_of_aggregat~",
                table: "category_of_aggregator_localizations");

            migrationBuilder.DropForeignKey(
                name: "FK_developer_of_aggregator_localizations_developers_of_aggrega~",
                table: "developer_of_aggregator_localizations");

            migrationBuilder.DropForeignKey(
                name: "FK_download_link_of_aggregator_localizations_download_links_of~",
                table: "download_link_of_aggregator_localizations");

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

            migrationBuilder.AddColumn<bool>(
                name: "IsSystem",
                table: "programs_of_aggregator",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NeedsReview",
                table: "programs_of_aggregator",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "programs_of_aggregator",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "programs_of_aggregator",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "TagOfAggregatorId",
                table: "program_tags_of_aggregator",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProgramOfAggregatorId",
                table: "program_tags_of_aggregator",
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

            migrationBuilder.AddColumn<string>(
                name: "ExternalIdentifier",
                table: "program_market_data_of_aggregator",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

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

            migrationBuilder.AddColumn<string>(
                name: "HierarchyPath",
                table: "categories_of_aggregator",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "categories_of_aggregator",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "program_slug_redirects_of_aggregator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OldSlug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NewSlug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ProgramOfAggregatorId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_program_slug_redirects_of_aggregator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_program_slug_redirects_of_aggregator_programs_of_aggregator~",
                        column: x => x.ProgramOfAggregatorId,
                        principalTable: "programs_of_aggregator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "program_snapshots_of_aggregator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProgramOfAggregatorId = table.Column<int>(type: "integer", nullable: false),
                    AggregatorSourceId = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RatingValue = table.Column<double>(type: "double precision", nullable: true),
                    DownloadCount = table.Column<long>(type: "bigint", nullable: true),
                    SnapshotDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_program_snapshots_of_aggregator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_program_snapshots_of_aggregator_aggregator_sources_Aggregat~",
                        column: x => x.AggregatorSourceId,
                        principalTable: "aggregator_sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_program_snapshots_of_aggregator_programs_of_aggregator_Prog~",
                        column: x => x.ProgramOfAggregatorId,
                        principalTable: "programs_of_aggregator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_languages_of_aggregator_Code",
                table: "languages_of_aggregator",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_languages_of_aggregator_ShortCode",
                table: "languages_of_aggregator",
                column: "ShortCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_categories_of_aggregator_HierarchyPath",
                table: "categories_of_aggregator",
                column: "HierarchyPath");

            migrationBuilder.CreateIndex(
                name: "IX_program_slug_redirects_of_aggregator_CreatedAt",
                table: "program_slug_redirects_of_aggregator",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_program_slug_redirects_of_aggregator_OldSlug",
                table: "program_slug_redirects_of_aggregator",
                column: "OldSlug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_program_slug_redirects_of_aggregator_ProgramOfAggregatorId",
                table: "program_slug_redirects_of_aggregator",
                column: "ProgramOfAggregatorId");

            migrationBuilder.CreateIndex(
                name: "IX_program_slug_redirects_of_aggregator_UpdatedAt",
                table: "program_slug_redirects_of_aggregator",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_program_snapshots_of_aggregator_AggregatorSourceId",
                table: "program_snapshots_of_aggregator",
                column: "AggregatorSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_program_snapshots_of_aggregator_CreatedAt",
                table: "program_snapshots_of_aggregator",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_program_snapshots_of_aggregator_ProgramOfAggregatorId_Snaps~",
                table: "program_snapshots_of_aggregator",
                columns: new[] { "ProgramOfAggregatorId", "SnapshotDate" });

            migrationBuilder.CreateIndex(
                name: "IX_program_snapshots_of_aggregator_UpdatedAt",
                table: "program_snapshots_of_aggregator",
                column: "UpdatedAt");

            migrationBuilder.AddForeignKey(
                name: "FK_category_of_aggregator_localizations_categories_of_aggregat~",
                table: "category_of_aggregator_localizations",
                column: "CategoryOfAggregatorId",
                principalTable: "categories_of_aggregator",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_developer_of_aggregator_localizations_developers_of_aggrega~",
                table: "developer_of_aggregator_localizations",
                column: "DeveloperOfAggregatorId",
                principalTable: "developers_of_aggregator",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_category_of_aggregator_localizations_categories_of_aggregat~",
                table: "category_of_aggregator_localizations");

            migrationBuilder.DropForeignKey(
                name: "FK_developer_of_aggregator_localizations_developers_of_aggrega~",
                table: "developer_of_aggregator_localizations");

            migrationBuilder.DropForeignKey(
                name: "FK_download_link_of_aggregator_localizations_download_links_of~",
                table: "download_link_of_aggregator_localizations");

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
                name: "program_slug_redirects_of_aggregator");

            migrationBuilder.DropTable(
                name: "program_snapshots_of_aggregator");

            migrationBuilder.DropIndex(
                name: "IX_languages_of_aggregator_Code",
                table: "languages_of_aggregator");

            migrationBuilder.DropIndex(
                name: "IX_languages_of_aggregator_ShortCode",
                table: "languages_of_aggregator");

            migrationBuilder.DropIndex(
                name: "IX_categories_of_aggregator_HierarchyPath",
                table: "categories_of_aggregator");

            migrationBuilder.DropColumn(
                name: "IsSystem",
                table: "programs_of_aggregator");

            migrationBuilder.DropColumn(
                name: "NeedsReview",
                table: "programs_of_aggregator");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "programs_of_aggregator");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "programs_of_aggregator");

            migrationBuilder.DropColumn(
                name: "ExternalIdentifier",
                table: "program_market_data_of_aggregator");

            migrationBuilder.DropColumn(
                name: "HierarchyPath",
                table: "categories_of_aggregator");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "categories_of_aggregator");

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
                name: "TagOfAggregatorId",
                table: "program_tags_of_aggregator",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "ProgramOfAggregatorId",
                table: "program_tags_of_aggregator",
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

            migrationBuilder.AddForeignKey(
                name: "FK_category_of_aggregator_localizations_categories_of_aggregat~",
                table: "category_of_aggregator_localizations",
                column: "CategoryOfAggregatorId",
                principalTable: "categories_of_aggregator",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_developer_of_aggregator_localizations_developers_of_aggrega~",
                table: "developer_of_aggregator_localizations",
                column: "DeveloperOfAggregatorId",
                principalTable: "developers_of_aggregator",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_download_link_of_aggregator_localizations_download_links_of~",
                table: "download_link_of_aggregator_localizations",
                column: "DownloadLinkOfAggregatorId",
                principalTable: "download_links_of_aggregator",
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
    }
}
