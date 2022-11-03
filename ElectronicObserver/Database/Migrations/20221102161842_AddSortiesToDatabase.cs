using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronicObserver.Database.Migrations
{
    public partial class AddSortiesToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SortieRecordId",
                table: "ApiFiles",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Sorties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    World = table.Column<int>(type: "INTEGER", nullable: false),
                    Map = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sorties", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiFiles_SortieRecordId",
                table: "ApiFiles",
                column: "SortieRecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApiFiles_Sorties_SortieRecordId",
                table: "ApiFiles",
                column: "SortieRecordId",
                principalTable: "Sorties",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApiFiles_Sorties_SortieRecordId",
                table: "ApiFiles");

            migrationBuilder.DropTable(
                name: "Sorties");

            migrationBuilder.DropIndex(
                name: "IX_ApiFiles_SortieRecordId",
                table: "ApiFiles");

            migrationBuilder.DropColumn(
                name: "SortieRecordId",
                table: "ApiFiles");
        }
    }
}
