using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DFCStats.Business.Interfaces;
using DFCStats.Web.Models.Tables;

namespace DFCStats.Web.Controllers;

public class TableController : Controller
{
    private readonly ITableService _tableService;
    private readonly ISeasonService _seasonService;
    private readonly IClubService _clubService;

    public TableController(ITableService tableService, ISeasonService seasonService, IClubService clubService)
    {
        _tableService = tableService;
        _seasonService = seasonService;
        _clubService = clubService;
    }

    public async Task<IActionResult> Manage(string id)
    {
        // Validate that the id parameter is a valid GUID format
        // the seasonId is set to the guid if the parsing is successful
        if (!Guid.TryParse(id, out var seasonId))
            // If the id is not a valid GUID, return a 400 Bad Request HTTP response
            return BadRequest("Invalid ID format");

        // Get the season from the database - including the table
        var season = await _seasonService.GetSeasonByIdAsync(seasonId, SeasonIncludes.Tables);

        // If the season record is not found, return a 404 Not Found HTTP response
        if (season == null)
            return NotFound("Season not found");

        // Set the page heading and the page title
        var pageHeadingAndTitle = string.Format($"Manage Table for Season {season.Description}");
		@ViewData["PageHeading"] =  pageHeadingAndTitle;
		ViewData["Title"] = pageHeadingAndTitle;

        // Get all the clubs from the database and convert them to a selectListItem
        ViewBag.clubs = (await _clubService.GetAllClubsAsync())
            .OrderBy(c => c.Name)
            .Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();

        // Convert the season's table into a model
        ViewBag.table = season.Table?.Select(t => new Tables()
        {
            Id = t.Id,
            ClubName = t.ClubName,
            Position = t.Position,
            GamesPlayed = t.GamesPlayed,
            HomeGamesWon = t.HomeGamesWon,
            HomeGamesDrawn = t.HomeGamesDrawn,
            HomeGamesLost = t.HomeGamesLost,
            HomeGoalsFor = t.HomeGoalsFor,
            HomeGoalsAgainst = t.HomeGoalsAgainst,
            AwayGamesWon = t.AwayGamesWon,
            AwayGamesDrawn = t.AwayGamesDrawn,
            AwayGamesLost = t.AwayGamesLost,
            AwayGoalsFor = t.AwayGoalsFor,
            AwayGoalsAgainst = t.AwayGoalsAgainst,
            GoalDifference = t.GoalDifference,
            Points = t.Points,
            IsChampion = t.IsChampion,
            IsPromotion = t.IsPromotion,
            IsRelegation = t.IsRelegation,
            IsPlayOff = t.IsPlayOff,
            IsDarlington = t.IsDarlington,
            Notes = t.Notes
        }).ToList();

        return View();
    }
}