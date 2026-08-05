using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DFCStats.Business.Interfaces;
using DFCStats.Web.Models.Tables;
using DFCStats.Domain.Exceptions;

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

    public async Task<IActionResult> Add(string id)
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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(NewTable newTable)
    {
        if (ModelState.IsValid)
        {
            // Create the table DTO - validation should ensure the values and valid and not null
            var tableDTO = new DFCStats.Domain.DTOs.Tables.TableDTO
            {
                SeasonId = newTable.SeasonId,
                ClubId = newTable.ClubId,
                GamesPlayed = newTable.Played ?? 0,
                HomeGamesWon = newTable.HomeWon ?? 0,
                HomeGamesDrawn = newTable.HomeDrawn ?? 0,
                HomeGamesLost = newTable.HomeLost ?? 0,
                HomeGoalsFor = newTable.HomeGoalsFor ?? 0,
                HomeGoalsAgainst = newTable.HomeGoalsAgainst ?? 0,
                AwayGamesWon = newTable.AwayWon ?? 0,
                AwayGamesDrawn = newTable.AwayDrawn ?? 0,
                AwayGamesLost = newTable.AwayLost ?? 0,
                AwayGoalsFor = newTable.AwayGoalsFor ?? 0,
                AwayGoalsAgainst = newTable.AwayGoalsAgainst ?? 0,
                Points = newTable.Points ?? 0,
                IsChampion = newTable.IsChampion ?? false,
                IsPromotion = newTable.IsPromotion ?? false,
                IsPlayOff = newTable.IsPlayOffs ?? false,
                IsRelegation = newTable.IsRelegated ?? false,
                IsDarlington = newTable.IsDarlington ?? false,
                Notes = newTable.Notes
            };

            try
            {
                // Add the new table entry to the database
                await _tableService.AddTableEntryAsync(tableDTO);

                // Add a success message to TempData
                TempData["Success"] = "Table has been updated";

            } catch (DFCStatsException ex)
            {
                // Add a failure message to TempData
                TempData["Failure"] = ex.Message;
            }
        }

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

        return View(newTable);
    }

    public async Task<IActionResult> Edit(string id)
    {
        // Validate that the id parameter is a valid GUID format
        // the tableId is set to the guid if the parsing is successful
        if (!Guid.TryParse(id, out var tableId))
            // If the id is not a valid GUID, return a 400 Bad Request HTTP response
            return BadRequest("Invalid ID format");

        // Get the table entry from the database
        var tableEntry = await _tableService.GetTableEntryByIdAsync(tableId, TableIncludes.Clubs);

        // If the table entry is not found, return a 404 Not Found HTTP response
        if (tableEntry == null)
            return NotFound("Table entry not found");

        // Get the season from the database - including the table
        var season = await _seasonService.GetSeasonByIdAsync(tableEntry.SeasonId, SeasonIncludes.Tables);

        // If the season record is not found, return a 404 not found HTTP response
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

        // Sets up the edit table form - this includes all the options for the dropdowns and the season id
        var viewModel = new EditTable
        {
            Id = tableEntry.Id,
            SeasonId = tableEntry.SeasonId,
            ClubId = tableEntry.ClubId,
            Played = tableEntry.GamesPlayed,
            HomeWon = tableEntry.HomeGamesWon,
            HomeDrawn = tableEntry.HomeGamesDrawn,
            HomeLost = tableEntry.HomeGamesLost,
            HomeGoalsFor = tableEntry.HomeGoalsFor,
            HomeGoalsAgainst = tableEntry.HomeGoalsAgainst,
            AwayWon = tableEntry.AwayGamesWon,
            AwayDrawn = tableEntry.AwayGamesDrawn,
            AwayLost = tableEntry.AwayGamesLost,
            AwayGoalsFor = tableEntry.AwayGoalsFor,
            AwayGoalsAgainst = tableEntry.AwayGoalsAgainst,
            Points = tableEntry.Points,
            IsChampion = tableEntry.IsChampion,
            IsPromotion = tableEntry.IsPromotion,
            IsPlayOffs = tableEntry.IsPlayOff,
            IsRelegated = tableEntry.IsRelegation,
            IsDarlington = tableEntry.IsDarlington,
            Notes = tableEntry.Notes,
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

        // To be completed later on
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditTable editTable)
    {
        if (ModelState.IsValid)
        {
            // Process the form if the model state is valid
            var x = "";
        }

        // Get the season from the database - including the table
        var season = await _seasonService.GetSeasonByIdAsync(editTable.SeasonId, SeasonIncludes.Tables);

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
        editTable.ClubOptions = clubs.Select(dto => new SelectListItem
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
        editTable.IsChampionOptions = trueFalseOptions;
        editTable.IsPromotionOptions = trueFalseOptions;
        editTable.IsPlayOffsOptions = trueFalseOptions;
        editTable.IsRelegatedOptions = trueFalseOptions;
        editTable.IsDarlingtonOptions = trueFalseOptions;

        // Convert the season's table into a model
        ViewBag.table = ConvertToTableModel(season.Table!);

        return View(editTable);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove([FromBody] DFCStats.Web.Models.Tables.Tables tableToRemove)
    {
        try
        {
            // Create a new table DTO with the id of the table entry to be removed
            var tableDTO = new DFCStats.Domain.DTOs.Tables.TableDTO { Id = tableToRemove.Id };

            // Remove the table entry from the database
            await _tableService.RemoveTableEntryAsync(tableDTO);
        }
        catch (DFCStatsException ex)
        {
            // There was a problem removing the record
            return Json(new { success = false, messageToUser = ex.Message });
        }

        return Json(new { success = true, messageToUser = "Club removed from table" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Move([FromBody] DFCStats.Web.Models.Tables.MoveTable moveTable)
    {
        try
        {
            // Create a new table DTO with the id of the table entry to be moved
            var tableDTO = new DFCStats.Domain.DTOs.Tables.TableDTO { Id = moveTable.Id };

            // Checks the direction
            if (moveTable.Direction == "up")
            {
                // Move the table entry up in the table
                await _tableService.ChangeTableEntryPositionAsync(tableDTO, TableDirections.Up);
            } else
            {
                // Move the table entry down in the table
                await _tableService.ChangeTableEntryPositionAsync(tableDTO, TableDirections.Down);
            }
        } catch (DFCStatsException ex)
        {
            // There was a problem moving the record
            return Json(new { success = false, messageToUser = ex.Message });
        }

        return Json(new { success = true, messageToUser = "Club moved in table" });
    }

    public async Task<IActionResult> RefreshTablesPanel(string seasonId)
    {
        // Get the season from the database - including the table
        var season = await _seasonService.GetSeasonByIdAsync(Guid.Parse(seasonId), SeasonIncludes.Tables);

        // Check that the season was found in the database
        if (season == null)
            // Return the partial view with a null model if the season wasn't found
            return PartialView("Partial_LeagueTable", null);

        // Convert the season's table into a model
        var tableModel = ConvertToTableModel(season.Table!);

        // Return the partial view with the table model
        return PartialView("Partial_LeagueTable", tableModel);
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