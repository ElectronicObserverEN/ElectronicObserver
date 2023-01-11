using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronicObserver.Database.Migrations
{
    public partial class AddShipPlanRemodelBooleans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NotifyOnAnyRemodelReady",
                table: "ShipTrainingPlans",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NotifyOnRemodelReady",
                table: "ShipTrainingPlans",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotifyOnAnyRemodelReady",
                table: "ShipTrainingPlans");

            migrationBuilder.DropColumn(
                name: "NotifyOnRemodelReady",
                table: "ShipTrainingPlans");
        }
    }
}
