using Microsoft.EntityFrameworkCore.Migrations;

namespace Epsic.Rpg.Migrations
{
    public partial class AddTeamsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "Characters",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Characters_TeamId",
                table: "Characters",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Teams_TeamId",
                table: "Characters",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Teams_TeamId",
                table: "Characters");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Characters_TeamId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Characters");
        }
    }
}
