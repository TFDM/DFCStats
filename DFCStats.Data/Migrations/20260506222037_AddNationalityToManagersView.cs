using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DFCStats.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNationalityToManagersView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER VIEW View_Managers AS
            SELECT
                M.Id,
                M.PersonId,
                P.FirstName,
                P.LastName,
                P.LastName + ', ' + P.FirstName AS LastNameFirstName,
                P.FirstName + ' ' + P.LastName AS FirstNameLastName,
                P.DateofBirth,
                P.NationalityId,
                N.Nationality,
                N.Icon,
                M.StartDate,
                M.EndDate,
                
                --Alternative end date. This is either the end date or the current date (if the manager is still in post)
                CASE WHEN M.EndDate IS NULL
                    THEN CONVERT(Date, GETDATE())
                    ELSE M.EndDate
                END AS AltEndDate,
                
                M.IsCaretaker,

                --Calculates the days in charge. If the end date is null the current date is used
                CASE WHEN M.EndDate IS NULL
                    THEN DATEDIFF(day, M.StartDate, GETDATE())
                    ELSE DATEDIFF(day, M.StartDate, M.EndDate)
                END AS DaysInCharge,

                --Calculates the number of games managed. If the end date is null the current date is used
                CASE WHEN M.EndDate IS NULL
                    THEN (SELECT COUNT(*) FROM Fixtures F WHERE F.Date >= M.StartDate AND F.Date <= GETDATE())
                    ELSE (SELECT COUNT(*) FROM Fixtures F WHERE F.Date >= M.StartDate AND F.Date <= M.EndDate)
                END AS GamesManaged,

                --Calculates the number of games won. If the end date is null the current date is used
                CASE WHEN M.EndDate IS NULL
                    THEN (SELECT COUNT(*) FROM Fixtures F WHERE (F.Date >= M.StartDate AND F.Date <= GETDATE()) AND F.Outcome = 'W')
                    ELSE (SELECT COUNT(*) FROM Fixtures F WHERE (F.Date >= M.StartDate AND F.Date <= M.EndDate) AND F.Outcome = 'W')
                END AS Won,

                --Calculates the number of games drawn. If the end date is null the current date is used
                CASE WHEN M.EndDate IS NULL
                    THEN (SELECT COUNT(*) FROM Fixtures F WHERE (F.Date >= M.StartDate AND F.Date <= GETDATE()) AND F.Outcome = 'D')
                    ELSE (SELECT COUNT(*) FROM Fixtures F WHERE (F.Date >= M.StartDate AND F.Date <= M.EndDate) AND F.Outcome = 'D')
                END AS Drawn,

                --Calculates the number of games lost. If the end date is null the current date is used
                CASE WHEN M.EndDate IS NULL
                    THEN (SELECT COUNT(*) FROM Fixtures F WHERE (F.Date >= M.StartDate AND F.Date <= GETDATE()) AND F.Outcome = 'L')
                    ELSE (SELECT COUNT(*) FROM Fixtures F WHERE (F.Date >= M.StartDate AND F.Date <= M.EndDate) AND F.Outcome = 'L')
                END AS Lost,

                --Sums the total goals scored
                ISNULL(
                CASE WHEN M.EndDate IS NULL
                    THEN (SELECT SUM(F.DarlingtonScore) FROM Fixtures F WHERE F.Date >= M.StartDate AND F.Date <= GETDATE())
                    ELSE (SELECT SUM(F.DarlingtonScore) FROM Fixtures F WHERE F.Date >= M.StartDate AND F.Date <= M.EndDate)
                END, 0) AS GoalsFor,

                --Sums the total goals conceded
                ISNULL(
                CASE WHEN M.EndDate IS NULL
                    THEN (SELECT SUM(F.OppositionScore) FROM Fixtures F WHERE F.Date >= M.StartDate AND F.Date <= GETDATE())
                    ELSE (SELECT SUM(F.OppositionScore) FROM Fixtures F WHERE F.Date >= M.StartDate AND F.Date <= M.EndDate)
                END, 0) AS GoalsAg,

                --Calculates the win percentage. If the end date is null the current date is used
                CASE WHEN M.EndDate IS NULL
                    --End date is null (they are still in charge)
                    THEN
                        --Check to see if there were any games played during their time in charge
                        CASE (SELECT COUNT(*) FROM Fixtures WHERE Date >= StartDate AND Date <= GETDATE())
                            WHEN 0 THEN 0
                            ELSE (SELECT CAST((SELECT COUNT(*) FROM Fixtures WHERE (Date >= StartDate AND Date <= GETDATE()) AND Outcome = 'W') AS decimal) / CAST((SELECT COUNT(*) FROM Fixtures WHERE Date >= StartDate AND Date <= GETDATE()) AS decimal) * 100)
                        END
                    --End date is not null
                    ELSE
                        --Check to see if there were any games played during their time in charge
                        CASE (SELECT COUNT(*) FROM Fixtures WHERE Date >= StartDate AND Date <= EndDate)
                            WHEN 0 THEN 0
                            ELSE (SELECT CAST((SELECT COUNT(*) FROM Fixtures WHERE (Date >= StartDate AND Date <= EndDate) AND Outcome = 'W') AS decimal) / CAST((SELECT COUNT(*) FROM Fixtures WHERE Date >= StartDate AND Date <= EndDate) AS decimal) * 100)
                        END
                END As WinPercentage

            FROM Managers M
            JOIN People P ON M.PersonId = P.Id
            JOIN Nationalities N ON P.NationalityId = N.Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER VIEW View_Managers AS
            SELECT
                M.Id,
                M.PersonId,
                P.FirstName,
                P.LastName,
                P.LastName + ', ' + P.FirstName AS LastNameFirstName,
                P.FirstName + ' ' + P.LastName AS FirstNameLastName,
                P.DateofBirth,
                P.NationalityId,
                N.Icon,
                M.StartDate,
                M.EndDate,
                
                --Alternative end date. This is either the end date or the current date (if the manager is still in post)
                CASE WHEN M.EndDate IS NULL
                    THEN CONVERT(Date, GETDATE())
                    ELSE M.EndDate
                END AS AltEndDate,
                
                M.IsCaretaker,

                --Calculates the days in charge. If the end date is null the current date is used
                CASE WHEN M.EndDate IS NULL
                    THEN DATEDIFF(day, M.StartDate, GETDATE())
                    ELSE DATEDIFF(day, M.StartDate, M.EndDate)
                END AS DaysInCharge,

                --Calculates the number of games managed. If the end date is null the current date is used
                CASE WHEN M.EndDate IS NULL
                    THEN (SELECT COUNT(*) FROM Fixtures F WHERE F.Date >= M.StartDate AND F.Date <= GETDATE())
                    ELSE (SELECT COUNT(*) FROM Fixtures F WHERE F.Date >= M.StartDate AND F.Date <= M.EndDate)
                END AS GamesManaged,

                --Calculates the number of games won. If the end date is null the current date is used
                CASE WHEN M.EndDate IS NULL
                    THEN (SELECT COUNT(*) FROM Fixtures F WHERE (F.Date >= M.StartDate AND F.Date <= GETDATE()) AND F.Outcome = 'W')
                    ELSE (SELECT COUNT(*) FROM Fixtures F WHERE (F.Date >= M.StartDate AND F.Date <= M.EndDate) AND F.Outcome = 'W')
                END AS Won,

                --Calculates the number of games drawn. If the end date is null the current date is used
                CASE WHEN M.EndDate IS NULL
                    THEN (SELECT COUNT(*) FROM Fixtures F WHERE (F.Date >= M.StartDate AND F.Date <= GETDATE()) AND F.Outcome = 'D')
                    ELSE (SELECT COUNT(*) FROM Fixtures F WHERE (F.Date >= M.StartDate AND F.Date <= M.EndDate) AND F.Outcome = 'D')
                END AS Drawn,

                --Calculates the number of games lost. If the end date is null the current date is used
                CASE WHEN M.EndDate IS NULL
                    THEN (SELECT COUNT(*) FROM Fixtures F WHERE (F.Date >= M.StartDate AND F.Date <= GETDATE()) AND F.Outcome = 'L')
                    ELSE (SELECT COUNT(*) FROM Fixtures F WHERE (F.Date >= M.StartDate AND F.Date <= M.EndDate) AND F.Outcome = 'L')
                END AS Lost,

                --Sums the total goals scored
                ISNULL(
                CASE WHEN M.EndDate IS NULL
                    THEN (SELECT SUM(F.DarlingtonScore) FROM Fixtures F WHERE F.Date >= M.StartDate AND F.Date <= GETDATE())
                    ELSE (SELECT SUM(F.DarlingtonScore) FROM Fixtures F WHERE F.Date >= M.StartDate AND F.Date <= M.EndDate)
                END, 0) AS GoalsFor,

                --Sums the total goals conceded
                ISNULL(
                CASE WHEN M.EndDate IS NULL
                    THEN (SELECT SUM(F.OppositionScore) FROM Fixtures F WHERE F.Date >= M.StartDate AND F.Date <= GETDATE())
                    ELSE (SELECT SUM(F.OppositionScore) FROM Fixtures F WHERE F.Date >= M.StartDate AND F.Date <= M.EndDate)
                END, 0) AS GoalsAg,

                --Calculates the win percentage. If the end date is null the current date is used
                CASE WHEN M.EndDate IS NULL
                    --End date is null (they are still in charge)
                    THEN
                        --Check to see if there were any games played during their time in charge
                        CASE (SELECT COUNT(*) FROM Fixtures WHERE Date >= StartDate AND Date <= GETDATE())
                            WHEN 0 THEN 0
                            ELSE (SELECT CAST((SELECT COUNT(*) FROM Fixtures WHERE (Date >= StartDate AND Date <= GETDATE()) AND Outcome = 'W') AS decimal) / CAST((SELECT COUNT(*) FROM Fixtures WHERE Date >= StartDate AND Date <= GETDATE()) AS decimal) * 100)
                        END
                    --End date is not null
                    ELSE
                        --Check to see if there were any games played during their time in charge
                        CASE (SELECT COUNT(*) FROM Fixtures WHERE Date >= StartDate AND Date <= EndDate)
                            WHEN 0 THEN 0
                            ELSE (SELECT CAST((SELECT COUNT(*) FROM Fixtures WHERE (Date >= StartDate AND Date <= EndDate) AND Outcome = 'W') AS decimal) / CAST((SELECT COUNT(*) FROM Fixtures WHERE Date >= StartDate AND Date <= EndDate) AS decimal) * 100)
                        END
                END As WinPercentage

            FROM Managers M
            JOIN People P ON M.PersonId = P.Id
            JOIN Nationalities N ON P.NationalityId = N.Id");
        }
    }
}
