using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronicObserver.Database.Migrations
{
	/// <inheritdoc />
	public partial class RemoveAutoRefresh : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "AutoRefresh");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "AutoRefresh",
				columns: table => new
				{
					Id = table.Column<int>(type: "INTEGER", nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					IsSingleMapMode = table.Column<bool>(type: "INTEGER", nullable: false),
					Rules = table.Column<string>(type: "TEXT", nullable: false),
					SingleMapModeMap = table.Column<string>(type: "TEXT", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AutoRefresh", x => x.Id);
				});
		}
	}
}
