using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronicObserver.Database.Migrations
{
    public partial class AddExpeditionsToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExpeditionRecordId",
                table: "ApiFiles",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Expeditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Expedition = table.Column<int>(type: "INTEGER", nullable: false),
                    Fleet = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expeditions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiFiles_ExpeditionRecordId",
                table: "ApiFiles",
                column: "ExpeditionRecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApiFiles_Expeditions_ExpeditionRecordId",
                table: "ApiFiles",
                column: "ExpeditionRecordId",
                principalTable: "Expeditions",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApiFiles_Expeditions_ExpeditionRecordId",
                table: "ApiFiles");

            migrationBuilder.DropTable(
                name: "Expeditions");

            migrationBuilder.DropIndex(
                name: "IX_ApiFiles_ExpeditionRecordId",
                table: "ApiFiles");

            migrationBuilder.DropColumn(
                name: "ExpeditionRecordId",
                table: "ApiFiles");
        }
    }
}
