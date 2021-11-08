using Microsoft.EntityFrameworkCore.Migrations;

namespace Salvo.Migrations
{
    public partial class addSalvoEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Salvos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GamePlayerID = table.Column<long>(type: "bigint", nullable: false),
                    Turn = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salvos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Salvos_GamePlayers_GamePlayerID",
                        column: x => x.GamePlayerID,
                        principalTable: "GamePlayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalvoLocations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    salvoID = table.Column<long>(type: "bigint", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalvoLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalvoLocations_Salvos_salvoID",
                        column: x => x.salvoID,
                        principalTable: "Salvos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalvoLocations_salvoID",
                table: "SalvoLocations",
                column: "salvoID");

            migrationBuilder.CreateIndex(
                name: "IX_Salvos_GamePlayerID",
                table: "Salvos",
                column: "GamePlayerID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalvoLocations");

            migrationBuilder.DropTable(
                name: "Salvos");
        }
    }
}
