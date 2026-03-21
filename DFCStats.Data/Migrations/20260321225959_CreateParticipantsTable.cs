using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DFCStats.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateParticipantsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FixtureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderNo = table.Column<int>(type: "int", nullable: false),
                    Started = table.Column<bool>(type: "bit", nullable: false),
                    Sub = table.Column<bool>(type: "bit", nullable: false),
                    Goals = table.Column<int>(type: "int", nullable: false),
                    YellowCard = table.Column<bool>(type: "bit", nullable: false),
                    RedCard = table.Column<bool>(type: "bit", nullable: false),
                    ReplacedByPersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReplacedTime = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Participants_Fixtures_FixtureId",
                        column: x => x.FixtureId,
                        principalTable: "Fixtures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Participants_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Participants_People_ReplacedByPersonId",
                        column: x => x.ReplacedByPersonId,
                        principalTable: "People",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Participants_FixtureId",
                table: "Participants",
                column: "FixtureId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_PersonId",
                table: "Participants",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_ReplacedByPersonId",
                table: "Participants",
                column: "ReplacedByPersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Participants");
        }
    }
}
