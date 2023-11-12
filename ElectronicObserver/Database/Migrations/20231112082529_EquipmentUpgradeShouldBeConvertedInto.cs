using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronicObserver.Database.Migrations
{
    public partial class EquipmentUpgradeShouldBeConvertedInto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShouldBeConvertedInto",
                table: "EquipmentUpgradePlanItems",
                type: "INTEGER",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShouldBeConvertedInto",
                table: "EquipmentUpgradePlanItems");
        }
    }
}
