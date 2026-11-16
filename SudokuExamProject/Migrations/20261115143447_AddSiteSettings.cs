using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SudokuExamProject.Migrations
{
    public partial class AddSiteSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SiteSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SiteName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SiteDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ContactEmail = table.Column<string>(type: "text", nullable: true),
                    IsGameEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AllowRegistration = table.Column<bool>(type: "boolean", nullable: false),
                    AllowHints = table.Column<bool>(type: "boolean", nullable: false),
                    DefaultDifficulty = table.Column<string>(type: "text", nullable: true),
                    MinimumPasswordLength = table.Column<int>(type: "integer", nullable: false),
                    MaxFailedLoginAttempts = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteSettings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SiteSettings");
        }
    }
}
