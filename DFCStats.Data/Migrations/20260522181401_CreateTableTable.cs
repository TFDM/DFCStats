using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DFCStats.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateTableTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tables",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SeasonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Position = table.Column<int>(type: "int", nullable: false),
                    Played = table.Column<int>(type: "int", nullable: false),
                    HomeWon = table.Column<int>(type: "int", nullable: false),
                    HomeDrawn = table.Column<int>(type: "int", nullable: false),
                    HomeLost = table.Column<int>(type: "int", nullable: false),
                    HomeGoalsFor = table.Column<int>(type: "int", nullable: false),
                    HomeGoalsAgainst = table.Column<int>(type: "int", nullable: false),
                    AwayWon = table.Column<int>(type: "int", nullable: false),
                    AwayDrawn = table.Column<int>(type: "int", nullable: false),
                    AwayLost = table.Column<int>(type: "int", nullable: false),
                    AwayGoalsFor = table.Column<int>(type: "int", nullable: false),
                    AwayGoalsAgainst = table.Column<int>(type: "int", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    IsChampion = table.Column<bool>(type: "bit", nullable: false),
                    IsPromotion = table.Column<bool>(type: "bit", nullable: false),
                    IsPlayOffs = table.Column<bool>(type: "bit", nullable: false),
                    IsRelegated = table.Column<bool>(type: "bit", nullable: false),
                    IsDarlington = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tables_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tables_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tables_ClubId",
                table: "Tables",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_Tables_SeasonId",
                table: "Tables",
                column: "SeasonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tables");
        }
    }
}
