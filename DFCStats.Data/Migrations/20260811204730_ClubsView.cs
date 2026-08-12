using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DFCStats.Data.Migrations
{
    /// <inheritdoc />
    public partial class ClubsView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE VIEW View_Clubs AS
            SELECT
                C.Id,
                C.Name,

                -- Number of competitive fixtures played.
                -- COUNT(F.Id) ignores NULL values, so clubs with no fixtures return 0.
                COUNT(F.Id) AS Played,

                -- Number of games won.
                -- The CASE expression contributes 1 for each win, otherwise 0.
                COALESCE(
                    SUM(CASE WHEN F.Outcome = 'W' THEN 1 ELSE 0 END),
                    0
                ) AS Won,

                -- Number of games drawn.
                COALESCE(
                    SUM(CASE WHEN F.Outcome = 'D' THEN 1 ELSE 0 END),
                    0
                ) AS Drawn,

                -- Number of games lost.
                COALESCE(
                    SUM(CASE WHEN F.Outcome = 'L' THEN 1 ELSE 0 END),
                    0
                ) AS Lost,

                -- Total goals scored by Darlington.
                -- COALESCE converts NULL to 0 for clubs with no fixtures.
                COALESCE(
                    SUM(F.DarlingtonScore),
                    0
                ) AS GoalsFor,

                -- Total goals scored by the opposition.
                COALESCE(
                    SUM(F.OppositionScore),
                    0
                ) AS GoalsAgainst

            FROM Clubs C

            -- Start with all clubs so that clubs with no fixtures are still included.
            LEFT JOIN Fixtures F
                ON F.ClubId = C.Id

            -- Only include fixtures whose category is a League, Cup or Play-Off.
            -- The LEFT JOIN ensures clubs with no qualifying fixtures are retained.
            LEFT JOIN Categories CAT
                ON CAT.Id = F.CategoryId
                AND (
                    CAT.League = 1
                    OR CAT.Cup = 1
                    OR CAT.PlayOff = 1
                )

            -- Aggregate the fixture statistics for each club.
            GROUP BY
                C.Id,
                C.Name;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW View_Clubs;");
        }
    }
}
