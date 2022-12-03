using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronicObserver.Database.Migrations
{
    public partial class AddCostCalculationDataToEquipmentUpgrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SelectedHelper",
                table: "EquipmentUpgradePlanItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SliderLevel",
                table: "EquipmentUpgradePlanItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

			migrationBuilder.Sql($"""
				UPDATE EquipmentUpgradePlanItems
				SET DesiredUpgradeLevel=255
				WHERE DesiredUpgradeLevel=-1
				""");
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectedHelper",
                table: "EquipmentUpgradePlanItems");

            migrationBuilder.DropColumn(
                name: "SliderLevel",
                table: "EquipmentUpgradePlanItems");
        }
    }
}
