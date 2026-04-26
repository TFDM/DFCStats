using DFCStats.Business.Interfaces;
using DFCStats.Web.Models.Seasons;
using Microsoft.AspNetCore.Mvc;
using DFCStats.Domain.Exceptions;
using DFCStats.Domain.DTOs.Seasons;
using X.PagedList;

namespace DFCStats.Web.Controllers;

public class SeasonController : Controller
{
    private readonly ISeasonService _seasonService;

    public SeasonController(ISeasonService seasonService)
    {
        _seasonService = seasonService;
    }

    public async Task<IActionResult> Index(string sort, int page = 1, int pageSize = 50)
    {
        //Set the page heading and the page title
		ViewData["PageHeading"] = "Seasons";
		ViewData["Title"] = "Seasons";

        // Ensure the page and page size are above not zero or negative
        page = (page < 1) ? 1 : page;
        pageSize = (pageSize < 1) ? 50 : pageSize;

        // Search for seasons
        var (seasons, totalCount) = await _seasonService.GetAllSeasonsWithPaginationAsync(
            page: page, 
            pageSize: pageSize, 
            sort: sort);

        // Convert the seasons from a DTO to a model
        var listOfSeasons = seasons.Select(dto => new Seasons
        {
            Id = dto.Id,
            Season = dto.Description,
            GamesPlayed = dto.GamesPlayed,
            Wins = dto.GamesWon,
            Draws = dto.GamesDrawn,
            Loses = dto.GamesLost,
            WinPercentage = (dto.WinPercentage != null) ? string.Format("{0:0}%", dto.WinPercentage) : null,
            AverageHomeAttendance = string.Format("{0:n0}", dto.AverageHomeAttendance),
            HighestHomeAttendance = string.Format("{0:n0}", dto.HighestHomeAttendance),
            TotalPlayersUsed = dto.TotalPlayersUed
        }).ToList();
        
        // Convert to a static list
		var seasonsAsIPagedList = new StaticPagedList<Seasons>(listOfSeasons, page, pageSize, totalCount);

        // If the sort parameter is null or empty then we are initializing the value as descending  
        ViewBag.SortByDescription = string.IsNullOrEmpty(sort) ? "description_desc" : "";
        ViewBag.SortByGamesPlayed = sort == "gamesplayed" ? "gamesplayed_desc" : "gamesplayed";
        ViewBag.SortByWins = sort == "wins" ? "wins_desc" : "wins";
        ViewBag.SortByDraws = sort == "draws" ? "draws_desc" : "draws";
        ViewBag.SortByLoses = sort == "loses" ? "loses_desc" : "loses";
        ViewBag.SortByWinPercentage = sort == "winpercentage" ? "winpercentage_desc" : "winpercentage";
        ViewBag.SortByAvgHomeAtt = sort == "averagehomeatt" ? "averagehomeatt_desc" : "averagehomeatt";
        ViewBag.SortByHighestHomeAtt = sort == "highesthomeatt" ? "highesthomeatt_desc" : "highesthomeatt";
        ViewBag.SortByPlayersUsed = sort == "playersused" ? "playersused_desc" : "playersused";
        ViewBag.Sort = sort;

        return View(seasonsAsIPagedList);
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

        // Convert the seasonDTO to an seasons model
        var seasonToDisplay = new Seasons
        { 
            Id = season.Id,
            Season = season.Description,
            GamesPlayed = season.GamesPlayed,
            Wins = season.GamesWon,
            Draws = season.GamesDrawn,
            Loses = season.GamesLost,
            WinPercentage = (season.WinPercentage != null) ? string.Format("{0:0}%", season.WinPercentage) : null,
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

    public async Task<IActionResult> Edit(string id)
    {
        //Set the page heading and the page title
		ViewData["PageHeading"] = "Edit Season";
		ViewData["Title"] = "Edit Season";

        // Validate that the id parameter is a valid GUID format
        // the seasonId is set to the guid if the parsing is successful
        if (!Guid.TryParse(id, out var seasonId))
            // If the id is not a valid GUID, return a 400 Bad Request HTTP response
            return BadRequest("Invalid ID format");

        // Retrieve the season record from the database using the validated GUID
        var season = await _seasonService.GetSeasonByIdAsync(seasonId);

        // If the season record is not found, return a 404 Not Found HTTP response
        if (season == null)
            return NotFound("Season not found");

        // Convert the seasonDTO to an EditSeason model
        var seasonToEdit = new EditSeason
        { 
            Id = season.Id,
            Description = season.Description
        };

        return View(seasonToEdit);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditSeason editSeason)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // Convert the EditSeason model to a SeasonDTO
                var seasonDTO = new SeasonDTO
                { 
                    Id = editSeason.Id,
                    Description = editSeason.Description 
                };

                // Update the season in the database
                await _seasonService.UpdateSeasonAsync(seasonDTO);

                // Add a success message to TempData
                TempData["Success"] = $"Season {seasonDTO.Description}  has been updated successfully";

                // Redirect to the index action
                return RedirectToAction("Index");
            } catch (DFCStatsException ex)
            {
                // Add a failure message to TempData
                TempData["Failure"] = ex.Message;
            }
        }

        //Set the page heading and the page title
		ViewData["PageHeading"] = "Edit Season";
		ViewData["Title"] = "Edit Season";

        return View(editSeason);
    }
}