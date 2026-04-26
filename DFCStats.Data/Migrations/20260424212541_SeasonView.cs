using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DFCStats.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeasonView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE VIEW View_Seasons AS
            SELECT
                Id,
                Description,

                --Count Games Played
                (SELECT COUNT(*)
                FROM Fixtures F
                JOIN Categories C ON F.CategoryID = C.Id
                WHERE C.Description != 'Friendly'
                AND F.SeasonID = S.ID) AS GamesPlayed,

                --Count Wins
                (SELECT COUNT(*)
                FROM Fixtures F
                JOIN Categories C ON F.CategoryID = C.Id
                WHERE C.Description != 'Friendly'
                AND F.SeasonID = S.ID
                AND F.Outcome = 'W') AS Wins,

                --Count Draws
                (SELECT COUNT(*)
                FROM Fixtures F
                JOIN Categories C ON F.CategoryID = C.Id
                WHERE C.Description != 'Friendly'
                AND F.SeasonID = S.ID
                AND F.Outcome = 'D') AS Draws,

                --Count Loses
                (SELECT COUNT(*)
                FROM Fixtures F
                JOIN Categories C ON F.CategoryID = C.Id
                WHERE C.Description != 'Friendly'
                AND F.SeasonID = S.ID
                AND F.Outcome = 'L') AS Loses,

                -- Win Percentage Calculation
                (SELECT (COUNT(*) * 100.0) / NULLIF((SELECT COUNT(*) FROM Fixtures F2 JOIN Categories C2 ON F2.CategoryID = C2.Id WHERE C2.Description != 'Friendly' AND F2.SeasonID = S.ID), 0)
                FROM Fixtures F
                JOIN Categories C ON F.CategoryID = C.Id
                WHERE C.Description != 'Friendly'
                AND F.SeasonID = S.ID
                AND F.Outcome = 'W') AS WinPercentage,

                --Average home attendance
                (SELECT
                    AVG(F.Attendance)
                FROM Fixtures F
                JOIN Venues V ON F.VenueID = V.Id
                JOIN Categories C ON F.CategoryID = C.Id
                WHERE F.SeasonID = S.Id
                AND (V.ShortDescription = 'H')
                AND (C.League = 1 OR C.PlayOff = 1 OR C.Cup = 1)) AS AverageHomeAttendance,

                --Highest home attendance
                (SELECT TOP 1
                    F.Attendance
                FROM Fixtures F
                JOIN Venues V ON F.VenueID = V.Id
                JOIN Categories C ON F.CategoryID = C.Id
                WHERE F.SeasonID = S.Id
                AND (V.ShortDescription = 'H')
                AND (C.League = 1 OR C.PlayOff = 1 OR C.Cup = 1)
                ORDER BY F.Attendance DESC
                ) AS HighestHomeAttendance,

                --Count players used
                (SELECT
                    COUNT(DISTINCT p.PersonID)
                FROM Fixtures F
                JOIN Categories C ON F.CategoryID = C.Id
                JOIN Participants p ON f.Id = p.FixtureID
                WHERE F.SeasonID = S.Id AND C.Description != 'Friendly'
                ) AS PlayersUsed

            FROM Seasons S");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW View_Seasons;");
        }
    }
}
