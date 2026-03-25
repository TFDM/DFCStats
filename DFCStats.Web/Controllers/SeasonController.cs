using DFCStats.Business.Interfaces;
using DFCStats.Web.Models.Seasons;
using Microsoft.AspNetCore.Mvc;
using DFCStats.Domain.Exceptions;
using DFCStats.Domain.DTOs.Seasons;

namespace DFCStats.Web.Controllers;

public class SeasonController : Controller
{
    private readonly ISeasonService _seasonService;

    public SeasonController(ISeasonService seasonService)
    {
        _seasonService = seasonService;
    }

    public async Task<IActionResult> Index()
    {
        var x = await _seasonService.GetSeasonByIdAsync(Guid.Parse("DDB2A4F0-5888-4D1C-B9F5-FF3858ADBA29"), SeasonIncludes.All);

        //Set the page heading and the page title
		ViewData["PageHeading"] = "Seasons";
		ViewData["Title"] = "Seasons";

        return View();
    }

    public async Task<IActionResult> Details(string id)
    {
        // Validate that the id parameter is a valid GUID format
        // the seasonId is set to the guid if the parsing is successful
        if (!Guid.TryParse(id, out var seasonId))
            // If the id is not a valid GUID, return a 400 Bad Request HTTP response
            return BadRequest("Invalid ID format");

        // Get the season from the database including all the appearance and fixture data
        var season = await _seasonService.GetSeasonByIdAsync(seasonId, SeasonIncludes.All);

        // If the season record is not found, return a 404 Not Found HTTP response
        if (season == null)
            return NotFound("Season not found");

        // Set the page heading and the page title
		@ViewData["PageHeading"] = season.Description;
		ViewData["Title"] = string.Format("Season {0}", season.Description);

        // Convert the seasonDTO to an EditPerson model
        var seasonToDisplay = new Seasons
        { 
            Id = season.Id,
            Season = season.Description,
            GamesPlayed = season.GamesPlayed,
            Wins = season.GamesWon,
            Draws = season.GamesDrawn,
            Loses = season.GamesLost,
            TotalPlayersUsed = season.TotalPlayersUed,
            AverageHomeAttendance = string.Format("{0:n0}", season.AverageHomeAttendance),
            HighestHomeAttendance = string.Format("{0:n0}", season.HighestHomeAttendance),
            Fixtures = season.Fixtures!.Select(f => new SeasonFixtures
            {
                Id = f.Id,
                Date = f.Date,
                Attendance = string.Format("{0:n0}", f.Attendance),
                Competition = f.Competition,
                Outcome = f.Outcome,
                Scoreline = f.Scoreline,
                Teams = f.Teams,
                TeamsWithScore = f.TeamsAndScores,
                Venue = f.Venue,
                PenaltiesRequired = f.PenaltiesRequired,
                PenaltyScoreWithOutcome = f.PenaltyScoreWithOutcome
            }).ToList(),
            Appearances = season.Appearances!.Select(a => new SeasonalAppearances
            {
                PersonId = a.PersonId,
                FirstName = a.FirstName,
                LastName = a.LastName,
                TotalAppearances = a.TotalAppearances,
                Starts = a.Starts,
                Subs = a.Subs,
                Goals = a.Goals,
                RedCards = a.RedCards,
                LeagueStarts = a.LeagueStarts,
                LeagueSubs = a.LeagueSubs,
                LeagueGoals = a.LeagueGoals,
                CupStarts = a.CupStarts,
                CupSubs = a.CupSubs,
                CupGoals = a.CupGoals,
                PlayOffStarts = a.PlayOffStarts,
                PlayOffSubs = a.PlayOffSubs,
                PlayOffGoals = a.PlayOffGoals
            }).OrderBy(a => a.LastName).ThenBy(a => a.FirstName).ToList()
        };

        return View(seasonToDisplay);
    }

    public async Task<IActionResult> New()
    {
        //Set the page heading and the page title
		ViewData["PageHeading"] = "Create Season";
		ViewData["Title"] = "Create Season";

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> New(NewSeason newSeason)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // Convert the NewSeason model to a SeasonDTO
                var seasonDTO = new SeasonDTO { Description = newSeason.Description! };

                // Add the new season to the database
                await _seasonService.AddSeasonAsync(seasonDTO);
                
                // Add a success message to TempData
                TempData["Success"] = $"{newSeason.Description} has been added successfully";

                // Redirect to the index action
                return RedirectToAction("Index");
            } catch (DFCStatsException ex)
            {
                // Add a failure message to TempData
                TempData["Failure"] = ex.Message;
            }
        }

        //Set the page heading and the page title
		ViewData["PageHeading"] = "Create Season";
		ViewData["Title"] = "Create Season";

        return View(newSeason);
    }
}