using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Salvo.Migrations
{
    public partial class addGamePlayerEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "Games",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateTable(
                name: "GamePlayers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JoinDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GameId = table.Column<long>(type: "bigint", nullable: false),
                    PlayerId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePlayers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GamePlayers_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GamePlayers_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GamePlayers_GameId",
                table: "GamePlayers",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GamePlayers_PlayerId",
                table: "GamePlayers",
                column: "PlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GamePlayers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "Games",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
