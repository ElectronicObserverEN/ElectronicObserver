using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronicObserver.Database.Migrations
{
    public partial class EquipmentUpgradePlanItemModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SliderLevel",
                table: "EquipmentUpgradePlanItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SliderLevel",
                table: "EquipmentUpgradePlanItems");
        }
    }
}
