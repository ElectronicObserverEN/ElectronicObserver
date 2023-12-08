using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronicObserver.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddedPropertyWillBeUsedForCOnversion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "WillBeUsedForConversion",
                table: "EquipmentAssignmentItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WillBeUsedForConversion",
                table: "EquipmentAssignmentItems");
        }
    }
}
