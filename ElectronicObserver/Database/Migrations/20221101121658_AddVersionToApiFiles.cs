using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronicObserver.Database.Migrations
{
	public partial class AddVersionToApiFiles : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<int>(
				name: "Version",
				table: "ApiFiles",
				type: "INTEGER",
				nullable: false,
				defaultValue: 0);

			migrationBuilder.Sql("""
				DELETE FROM ApiFiles
				WHERE Name in ("api_start2/getData",
						"api_get_member/questlist",
						"api_get_member/slot_item",
						"api_get_member/unsetslot",
						"api_get_member/ship3",
						"api_get_member/furniture",
						"api_get_member/require_info")
				""");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "Version",
				table: "ApiFiles");
		}
	}
}
