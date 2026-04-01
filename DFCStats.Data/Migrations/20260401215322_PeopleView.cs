using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DFCStats.Data.Migrations
{
    /// <inheritdoc />
    public partial class PeopleView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE VIEW View_People AS
                SELECT
                P.Id,
                P.FirstName,
                P.LastName,
                P.LastName + ', ' + P.FirstName AS LastNameFirstName,
                P.FirstName + ' ' + P.LastName AS FirstNameLastName,
                P.DateofBirth,
                P.NationalityID,
                N.Nationality,
                N.Icon,
                P.Biography,
                P.IsManager,
                --Total Appearances in League, Cup or Play-offs (Starts and Subs)
                (
                    SELECT COUNT(*) FROM Participants PA
                    JOIN Fixtures F ON PA.FixtureID = F.Id
                    JOIN Categories C ON F.CategoryID = C.Id
                    WHERE (C.League = 1 OR C.Cup = 1 OR C.PlayOff = 1) AND PA.PersonID = P.Id
                ) AS TotalApps,
                --Total Starts in League, Cup or Play-offs (Starts only)
                (
                    SELECT COUNT(*) FROM Participants PA
                    JOIN Fixtures F ON PA.FixtureID = F.Id
                    JOIN Categories C ON F.CategoryID = C.Id
                    WHERE (C.League = 1 OR C.Cup = 1 OR C.PlayOff = 1) AND PA.PersonID = P.Id AND PA.Started = 1
                ) AS TotalStartApps,
                --Total Subs in League, Cup or Play-offs (Subs only)
                (
                    SELECT COUNT(*) FROM Participants PA
                    JOIN Fixtures F ON PA.FixtureID = F.Id
                    JOIN Categories C ON F.CategoryID = C.Id
                    WHERE (C.League = 1 OR C.Cup = 1 OR C.PlayOff = 1) AND PA.PersonID = P.Id AND PA.Started = 0
                ) AS TotalSubApps,
                --Total Goals in League, Cup or Play-offs
                (
                    ISNULL(
                        (SELECT SUM(Goals) FROM Participants PA
                        JOIN Fixtures F ON PA.FixtureID = F.Id
                        JOIN Categories C ON F.CategoryID = C.Id
                        WHERE (C.League = 1 OR C.Cup = 1 OR C.PlayOff = 1) AND PA.PersonID = P.Id),
                        0)
                ) AS TotalGoals,
                -- Goals per Game (rounded up to 2 decimal places)
                CAST(
                    CEILING(
                        (
                            CAST(
                                ISNULL((
                                    SELECT SUM(Goals)
                                    FROM Participants PA
                                    JOIN Fixtures F ON PA.FixtureID = F.Id
                                    JOIN Categories C ON F.CategoryID = C.Id
                                    WHERE (C.League = 1 OR C.Cup = 1 OR C.PlayOff = 1)
                                    AND PA.PersonID = P.Id
                                ), 0) AS FLOAT
                            )
                            /
                            NULLIF((
                                SELECT COUNT(*)
                                FROM Participants PA
                                JOIN Fixtures F ON PA.FixtureID = F.Id
                                JOIN Categories C ON F.CategoryID = C.Id
                                WHERE (C.League = 1 OR C.Cup = 1 OR C.PlayOff = 1)
                                AND PA.PersonID = P.Id
                            ), 0)
                        ) * 100.0
                    ) / 100.0 AS DECIMAL(10, 2)
                ) AS GoalsPerGame
                FROM People P
                LEFT JOIN Nationalities N ON P.NationalityID = N.Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW View_People;");
        }
    }
}
