using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class SyncTagAndCategoryTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tag_of_aggregator_localizations_tags_of_aggregator_TagOfAgg~",
                table: "tag_of_aggregator_localizations");

            migrationBuilder.AddColumn<int>(
                name: "CategoryTagId",
                table: "tags_of_aggregator",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "tags_of_aggregator",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IconPath",
                table: "tags_of_aggregator",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFeature",
                table: "tags_of_aggregator",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "tags_of_aggregator",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "TagOfAggregatorId",
                table: "tag_of_aggregator_localizations",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "tag_of_aggregator_localizations",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "H1Title",
                table: "tag_of_aggregator_localizations",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetaDescription",
                table: "tag_of_aggregator_localizations",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetaTitle",
                table: "tag_of_aggregator_localizations",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "category_tags_of_aggregator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IconPath = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Color = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category_tags_of_aggregator", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "category_tag_of_aggregator_localizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryTagOfAggregatorId = table.Column<int>(type: "integer", nullable: false),
                    LanguageOfAggregatorId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category_tag_of_aggregator_localizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_category_tag_of_aggregator_localizations_category_tags_of_a~",
                        column: x => x.CategoryTagOfAggregatorId,
                        principalTable: "category_tags_of_aggregator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_category_tag_of_aggregator_localizations_languages_of_aggre~",
                        column: x => x.LanguageOfAggregatorId,
                        principalTable: "languages_of_aggregator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tags_of_aggregator_CategoryTagId_IsActive_IsDeleted",
                table: "tags_of_aggregator",
                columns: new[] { "CategoryTagId", "IsActive", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_category_tag_of_aggregator_localizations_CategoryTagOfAggre~",
                table: "category_tag_of_aggregator_localizations",
                columns: new[] { "CategoryTagOfAggregatorId", "LanguageOfAggregatorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_category_tag_of_aggregator_localizations_CreatedAt",
                table: "category_tag_of_aggregator_localizations",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_category_tag_of_aggregator_localizations_LanguageOfAggregat~",
                table: "category_tag_of_aggregator_localizations",
                column: "LanguageOfAggregatorId");

            migrationBuilder.CreateIndex(
                name: "IX_category_tag_of_aggregator_localizations_UpdatedAt",
                table: "category_tag_of_aggregator_localizations",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_category_tags_of_aggregator_CreatedAt",
                table: "category_tags_of_aggregator",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_category_tags_of_aggregator_IsDeleted",
                table: "category_tags_of_aggregator",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_category_tags_of_aggregator_Slug",
                table: "category_tags_of_aggregator",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_category_tags_of_aggregator_UpdatedAt",
                table: "category_tags_of_aggregator",
                column: "UpdatedAt");

            migrationBuilder.AddForeignKey(
                name: "FK_tag_of_aggregator_localizations_tags_of_aggregator_TagOfAgg~",
                table: "tag_of_aggregator_localizations",
                column: "TagOfAggregatorId",
                principalTable: "tags_of_aggregator",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tags_of_aggregator_category_tags_of_aggregator_CategoryTagId",
                table: "tags_of_aggregator",
                column: "CategoryTagId",
                principalTable: "category_tags_of_aggregator",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tag_of_aggregator_localizations_tags_of_aggregator_TagOfAgg~",
                table: "tag_of_aggregator_localizations");

            migrationBuilder.DropForeignKey(
                name: "FK_tags_of_aggregator_category_tags_of_aggregator_CategoryTagId",
                table: "tags_of_aggregator");

            migrationBuilder.DropTable(
                name: "category_tag_of_aggregator_localizations");

            migrationBuilder.DropTable(
                name: "category_tags_of_aggregator");

            migrationBuilder.DropIndex(
                name: "IX_tags_of_aggregator_CategoryTagId_IsActive_IsDeleted",
                table: "tags_of_aggregator");

            migrationBuilder.DropColumn(
                name: "CategoryTagId",
                table: "tags_of_aggregator");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "tags_of_aggregator");

            migrationBuilder.DropColumn(
                name: "IconPath",
                table: "tags_of_aggregator");

            migrationBuilder.DropColumn(
                name: "IsFeature",
                table: "tags_of_aggregator");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "tags_of_aggregator");

            migrationBuilder.DropColumn(
                name: "H1Title",
                table: "tag_of_aggregator_localizations");

            migrationBuilder.DropColumn(
                name: "MetaDescription",
                table: "tag_of_aggregator_localizations");

            migrationBuilder.DropColumn(
                name: "MetaTitle",
                table: "tag_of_aggregator_localizations");

            migrationBuilder.AlterColumn<int>(
                name: "TagOfAggregatorId",
                table: "tag_of_aggregator_localizations",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "tag_of_aggregator_localizations",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tag_of_aggregator_localizations_tags_of_aggregator_TagOfAgg~",
                table: "tag_of_aggregator_localizations",
                column: "TagOfAggregatorId",
                principalTable: "tags_of_aggregator",
                principalColumn: "Id");
        }
    }
}
