using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronicObserver.Database.Migrations
{
	public partial class SaveSortieFleetAndMapData : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			// all the old data doesn't contain enough info
			migrationBuilder.Sql("""
				DELETE FROM ApiFiles;
				DELETE FROM Sorties;
				VACUUM;
				""", true);

			migrationBuilder.AddColumn<string>(
				name: "FleetData",
				table: "Sorties",
				type: "TEXT",
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<string>(
				name: "MapData",
				table: "Sorties",
				type: "TEXT",
				nullable: false,
				defaultValue: "");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "FleetData",
				table: "Sorties");

			migrationBuilder.DropColumn(
				name: "MapData",
				table: "Sorties");
		}
	}
}
