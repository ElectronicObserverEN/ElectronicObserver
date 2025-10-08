using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronicObserver.TestData.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEquipmentData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AircraftCost",
                table: "MasterEquipment",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CardType",
                table: "MasterEquipment",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AircraftCost",
                table: "MasterEquipment");

            migrationBuilder.DropColumn(
                name: "CardType",
                table: "MasterEquipment");
        }
    }
}
