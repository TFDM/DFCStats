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

        // Get all the clubs from the database and sort by their name
        var clubs = await _clubService.GetAllClubsAsync(sort: "name");

        // Sets up the true/false options for the dropdowns
        var trueFalseOptions = new List<SelectListItem>
        {
            new SelectListItem { Text = "Yes", Value = "true" },
            new SelectListItem { Text = "No", Value = "false" }
        };

        // Sets up the new table form - this includes all the options for the dropdowns and the season id
        var viewModel = new NewTable
        {
            SeasonId = season.Id,
            ClubOptions = clubs.Select(dto => new SelectListItem
            {
                Text = dto.Name,
                Value = dto.Id.ToString()
            }),
            IsChampionOptions = trueFalseOptions,
            IsPromotionOptions = trueFalseOptions,  
            IsPlayOffsOptions = trueFalseOptions,
            IsRelegatedOptions = trueFalseOptions,
            IsDarlingtonOptions = trueFalseOptions
        };

        // Convert the season's table into a model
        ViewBag.table = ConvertToTableModel(season.Table!);

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Manage(NewTable newTable)
    {
        // Get the season from the database - including the table
        var season = await _seasonService.GetSeasonByIdAsync(newTable.SeasonId, SeasonIncludes.Tables);

        // If the season record is not found, return a 404 Not Found HTTP response
        // The season should be found as its passed in the form as a hidden field 
        if (season == null)
            return NotFound("Season not found");

        // Set the page heading and the page title
        var pageHeadingAndTitle = string.Format($"Manage Table for Season {season.Description}");
		@ViewData["PageHeading"] =  pageHeadingAndTitle;
		ViewData["Title"] = pageHeadingAndTitle;

        // Get all the clubs from the database and sort by their name
        var clubs = await _clubService.GetAllClubsAsync(sort: "name");

        // Populate the ClubOptions property of the model with the list of clubs for the dropdown
        newTable.ClubOptions = clubs.Select(dto => new SelectListItem
        {
            Text = dto.Name,
            Value = dto.Id.ToString()
        });

        // Sets up the true/false options for the dropdowns
        var trueFalseOptions = new List<SelectListItem>
        {
            new SelectListItem { Text = "Yes", Value = "true" },
            new SelectListItem { Text = "No", Value = "false" }
        };

        // Populate the true/false options for the dropdowns in the model
        newTable.IsChampionOptions = trueFalseOptions;
        newTable.IsPromotionOptions = trueFalseOptions;
        newTable.IsPlayOffsOptions = trueFalseOptions;
        newTable.IsRelegatedOptions = trueFalseOptions;
        newTable.IsDarlingtonOptions = trueFalseOptions;

        // Convert the season's table into a model
        ViewBag.table = ConvertToTableModel(season.Table!);

        if (ModelState.IsValid)
        {
            // Process the form submission and save the new table entry to the database

            // Return the user to the manage page and maintain the form input values
            return View(newTable);
        }

        return View(newTable);
    }

    /// <summary>
    /// Converts a list of TableDTO objects to a list of Tables model objects, ordered by position
    /// </summary>
    /// <param name="tableDtos"></param>
    /// <returns></returns>
    private List<Tables> ConvertToTableModel(List<DFCStats.Domain.DTOs.Tables.TableDTO> tableDtos)
    {
        return tableDtos.Select(t => new Tables()
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
        }).OrderBy(t => t.Position).ToList();
    }
}