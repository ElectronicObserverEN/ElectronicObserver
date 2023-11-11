using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronicObserver.Database.Migrations
{
    public partial class AddedParentToUpgradePlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "EquipmentUpgradePlanItems",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUpgradePlanItems_ParentId",
                table: "EquipmentUpgradePlanItems",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentUpgradePlanItems_EquipmentUpgradePlanItems_ParentId",
                table: "EquipmentUpgradePlanItems",
                column: "ParentId",
                principalTable: "EquipmentUpgradePlanItems",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentUpgradePlanItems_EquipmentUpgradePlanItems_ParentId",
                table: "EquipmentUpgradePlanItems");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentUpgradePlanItems_ParentId",
                table: "EquipmentUpgradePlanItems");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "EquipmentUpgradePlanItems");
        }
    }
}
