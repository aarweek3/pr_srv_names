using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class initSettingUseк : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Theme = table.Column<int>(type: "integer", nullable: false),
                    Density = table.Column<int>(type: "integer", nullable: false),
                    PrimaryColor = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    SidebarState = table.Column<int>(type: "integer", nullable: false),
                    NavigationBehavior = table.Column<int>(type: "integer", nullable: false),
                    TableDensity = table.Column<int>(type: "integer", nullable: false),
                    DefaultPageSize = table.Column<int>(type: "integer", nullable: false),
                    ShowAdvancedFilters = table.Column<bool>(type: "boolean", nullable: false),
                    Language = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    TimeZone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AccessibilityLevel = table.Column<int>(type: "integer", nullable: false),
                    NotificationLevel = table.Column<int>(type: "integer", nullable: false),
                    NotificationChannels = table.Column<int>(type: "integer", nullable: false),
                    SessionTerminationMode = table.Column<int>(type: "integer", nullable: false),
                    LoginNotificationMode = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSettings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_UserId",
                table: "UserSettings",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSettings");
        }
    }
}
