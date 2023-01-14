using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronicObserver.Database.Migrations
{
    public partial class RemoveShipPlanRemodelNotifyBoolean : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotifyOnRemodelReady",
                table: "ShipTrainingPlans");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NotifyOnRemodelReady",
                table: "ShipTrainingPlans",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
