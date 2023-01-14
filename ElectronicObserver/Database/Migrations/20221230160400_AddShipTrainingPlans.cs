using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronicObserver.Database.Migrations
{
    public partial class AddShipTrainingPlans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShipTrainingPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ShipId = table.Column<int>(type: "INTEGER", nullable: false),
                    TargetLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    TargetHPBonus = table.Column<int>(type: "INTEGER", nullable: false),
                    TargetASWBonus = table.Column<int>(type: "INTEGER", nullable: false),
                    TargetLuck = table.Column<int>(type: "INTEGER", nullable: false),
                    TargetRemodel = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipTrainingPlans", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShipTrainingPlans");
        }
    }
}
