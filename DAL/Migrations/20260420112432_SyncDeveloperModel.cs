using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class SyncDeveloperModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_developer_of_aggregator_localizations_developer_of_aggregat~",
                table: "developer_of_aggregator_localizations");

            migrationBuilder.DropForeignKey(
                name: "FK_programs_of_aggregator_developer_of_aggregators_DeveloperOf~",
                table: "programs_of_aggregator");

            migrationBuilder.DropPrimaryKey(
                name: "PK_developer_of_aggregators",
                table: "developer_of_aggregators");

            migrationBuilder.RenameTable(
                name: "developer_of_aggregators",
                newName: "developers_of_aggregator");

            migrationBuilder.RenameColumn(
                name: "Slug",
                table: "developers_of_aggregator",
                newName: "SystemCode");

            migrationBuilder.RenameColumn(
                name: "CanonicalName",
                table: "developers_of_aggregator",
                newName: "Name");

            migrationBuilder.RenameIndex(
                name: "IX_developer_of_aggregators_UpdatedAt",
                table: "developers_of_aggregator",
                newName: "IX_developers_of_aggregator_UpdatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_developer_of_aggregators_Slug",
                table: "developers_of_aggregator",
                newName: "IX_developers_of_aggregator_SystemCode");

            migrationBuilder.RenameIndex(
                name: "IX_developer_of_aggregators_IsDeleted",
                table: "developers_of_aggregator",
                newName: "IX_developers_of_aggregator_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_developer_of_aggregators_CreatedAt",
                table: "developers_of_aggregator",
                newName: "IX_developers_of_aggregator_CreatedAt");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "developers_of_aggregator",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "developers_of_aggregator",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_developers_of_aggregator",
                table: "developers_of_aggregator",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_developer_of_aggregator_localizations_developers_of_aggrega~",
                table: "developer_of_aggregator_localizations",
                column: "DeveloperOfAggregatorId",
                principalTable: "developers_of_aggregator",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_programs_of_aggregator_developers_of_aggregator_DeveloperOf~",
                table: "programs_of_aggregator",
                column: "DeveloperOfAggregatorId",
                principalTable: "developers_of_aggregator",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_developer_of_aggregator_localizations_developers_of_aggrega~",
                table: "developer_of_aggregator_localizations");

            migrationBuilder.DropForeignKey(
                name: "FK_programs_of_aggregator_developers_of_aggregator_DeveloperOf~",
                table: "programs_of_aggregator");

            migrationBuilder.DropPrimaryKey(
                name: "PK_developers_of_aggregator",
                table: "developers_of_aggregator");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "developers_of_aggregator");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "developers_of_aggregator");

            migrationBuilder.RenameTable(
                name: "developers_of_aggregator",
                newName: "developer_of_aggregators");

            migrationBuilder.RenameColumn(
                name: "SystemCode",
                table: "developer_of_aggregators",
                newName: "Slug");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "developer_of_aggregators",
                newName: "CanonicalName");

            migrationBuilder.RenameIndex(
                name: "IX_developers_of_aggregator_UpdatedAt",
                table: "developer_of_aggregators",
                newName: "IX_developer_of_aggregators_UpdatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_developers_of_aggregator_SystemCode",
                table: "developer_of_aggregators",
                newName: "IX_developer_of_aggregators_Slug");

            migrationBuilder.RenameIndex(
                name: "IX_developers_of_aggregator_IsDeleted",
                table: "developer_of_aggregators",
                newName: "IX_developer_of_aggregators_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_developers_of_aggregator_CreatedAt",
                table: "developer_of_aggregators",
                newName: "IX_developer_of_aggregators_CreatedAt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_developer_of_aggregators",
                table: "developer_of_aggregators",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_developer_of_aggregator_localizations_developer_of_aggregat~",
                table: "developer_of_aggregator_localizations",
                column: "DeveloperOfAggregatorId",
                principalTable: "developer_of_aggregators",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_programs_of_aggregator_developer_of_aggregators_DeveloperOf~",
                table: "programs_of_aggregator",
                column: "DeveloperOfAggregatorId",
                principalTable: "developer_of_aggregators",
                principalColumn: "Id");
        }
    }
}
