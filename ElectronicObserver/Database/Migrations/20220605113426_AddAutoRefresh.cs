using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronicObserver.Database.Migrations
{
    public partial class AddAutoRefresh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AutoRefresh",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsSingleMapMode = table.Column<bool>(type: "INTEGER", nullable: false),
                    Rules = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoRefresh", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutoRefresh");
        }
    }
}
