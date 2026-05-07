using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DFCStats.Business.Interfaces;
using DFCStats.Web.Models.Managers;
using X.PagedList;

namespace DFCStats.Web.Controllers;

public class ManagerController : Controller
{
    private readonly IManagerService _managerService;
    
    public ManagerController(IManagerService managerService)
    {
        _managerService = managerService;
    }

    public async Task<IActionResult> Index(bool includeCaretakers, string sort, int page = 1, int pageSize = 50)
    {
        // Set the page heading and the page title
		ViewData["PageHeading"] = "Managers";
		ViewData["Title"] = "Managers";

        // Ensure the page and page size are above not zero or negative
        page = (page < 1) ? 1 : page;
        pageSize = (pageSize < 1) ? 50 : pageSize;

        // Creates a select list for the include caretakers dropdown
        ViewBag.includeCaretakers = new List<SelectListItem>()
        {
            new SelectListItem() { Text="Yes", Value="true" },
            new SelectListItem() { Text="No", Value="false" }
        };

        // Search for management records
        var (managers, totalCount) = await _managerService.GetManagementRecordsWithPaginationAsync(
            page: page, 
            pageSize: pageSize, 
            includeCaretakers: includeCaretakers, 
            sort: sort);

        // Convert the managers from a DTO to a model
        var listOfManagers = managers.Select(dto => new Manager
        {
            Id = dto.Id,
            Name = dto.LastNameFirstName,
            Nationality = dto.Nationality,
            NationalityIcon = dto.NationalityIcon,
            DateFrom = dto.StartDate,
            DateTo = dto.EndDate,
            TimeInCharge = dto.TimeInChargeAsString,
            CurrentlyOnGoing = (dto.EndDate == null) ? true : false,
            Caretaker = dto.IsCaretaker,
            GamesManaged = dto.NumberOfGamesManaged,
            Wins = dto.Wins,
            Draws = dto.Draws,
            Loses = dto.Losses,
            GoalsFor = dto.GoalsFor,
            GoalsAgainst = dto.GoalsAgainst,
            WinPercentage = (dto.WinPercentage != null) ? string.Format("{0:0}%", dto.WinPercentage) : null
        }).ToList();

        // Convert to a static list
		var managersAsIPagedList = new StaticPagedList<Manager>(listOfManagers, page, pageSize, totalCount);

        // If the sort parameter is null or empty then we are initializing the value as descending
        ViewBag.SortByStartDate = string.IsNullOrEmpty(sort) ? "startDate" : "";
        ViewBag.SortByEndDate = sort == "endDate" ? "endDate_desc" : "endDate";
        ViewBag.SortByName = sort == "managerName" ? "managerName_desc" : "managerName";
        ViewBag.SortByTimeInCharge = sort == "timeInCharge" ? "timeInCharge_desc" : "timeInCharge";
        ViewBag.SortByNationality = sort == "nationality" ? "nationality_desc" : "nationality";
        ViewBag.SortByGamesManaged = sort == "gamesManaged" ? "gamesManaged_desc" : "gamesManaged";
        ViewBag.SortByGamesWon = sort == "won" ? "won_desc" : "won";
        ViewBag.SortByGamesDrawn = sort == "drawn" ? "drawn_desc" : "drawn";
        ViewBag.SortByGamesLost = sort == "lost" ? "lost_desc" : "lost";
        ViewBag.SortByWinPercentage = sort == "winsPercentage" ? "winsPercentage_desc" : "winsPercentage";
        ViewBag.SortByGoalsFor = sort == "goalsFor" ? "goalsFor_desc" : "goalsFor";
        ViewBag.SortByGoalsAgainst = sort == "goalsAgainst" ? "goalsAgainst_desc" : "goalsAgainst";
        ViewBag.Sort = sort;

        return View(managersAsIPagedList);
    }
}