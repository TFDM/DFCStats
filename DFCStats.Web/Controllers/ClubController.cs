using Microsoft.AspNetCore.Mvc;
using DFCStats.Business.Interfaces;
using DFCStats.Web.Models.Clubs;
using DFCStats.Domain.Exceptions;
using DFCStats.Domain.DTOs.Clubs;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DFCStats.Web.Controllers;

public class ClubController : Controller
{
    private readonly IClubService _clubService;

    public ClubController(IClubService clubService)
    {
        _clubService = clubService;
    }

    public async Task<IActionResult> Index(string clubName,string sort, int page = 1, int pageSize = 50)
    {
        // Set the page heading and the page title
		ViewData["PageHeading"] = "Clubs";
		ViewData["Title"] = "Clubs";

        // Ensure the page and page size are above not zero or negative
        page = (page < 1) ? 1 : page;
        pageSize = (pageSize < 1) ? 50 : pageSize;

        // Creates a select list of page sizes
        ViewBag.pageSize = new List<SelectListItem>()
        {
            new SelectListItem() { Text="25", Value="25" },
            new SelectListItem() { Text="50", Value="50" },
            new SelectListItem() { Text="75", Value="75" },
            new SelectListItem() { Text="100", Value="100" }
        };

        // Search for clubs
        var (clubs, totalCount) = await _clubService.SearchForClubsAsync(
            page: page, 
            pageSize: pageSize, 
            searchName: clubName, 
            sort: sort);

        // Convert the clubs from a DTO to a model
        var listOfClubs = clubs.Select(dto => new Clubs
        {
            Id = dto.Id,
            Name = dto.Name,
            Played = dto.Played,
            Won = dto.Won,
            Drawn = dto.Drawn,
            Lost = dto.Lost,
            GoalsFor = dto.GoalsFor,
            GoalsAgainst = dto.GoalsAgainst
        }).ToList();

        // Convert to a static list
		var clubsAsIPagedList = new StaticPagedList<Clubs>(listOfClubs, page, pageSize, totalCount);

        // If the sort parameter is null or empty then we are initializing the value as descending  
        ViewBag.SortByName = string.IsNullOrEmpty(sort) ? "name_desc" : "";
        ViewBag.SortByPlayed = sort == "played" ? "played_desc" : "played";
        ViewBag.SortByWon = sort == "won" ? "won_desc" : "won";
        ViewBag.SortByDrawn = sort == "drawn" ? "drawn_desc" : "drawn";
        ViewBag.SortByLost = sort == "lost" ? "lost_desc" : "lost";
        ViewBag.SortByGoalsFor = sort == "goalsFor" ? "goalsFor_desc" : "goalsFor";
        ViewBag.SortByGoalsAgainst = sort == "goalsAgainst" ? "goalsAgainst_desc" : "goalsAgainst";
        ViewBag.Sort = sort;

        return View(clubsAsIPagedList);
    }

    public async Task<IActionResult> New()
    {
        //Set the page heading and the page title
		ViewData["PageHeading"] = "Create Club";
		ViewData["Title"] = "Create Club";

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> New(NewClub newClub)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // Convert the NewClub model to a ClubDTO
                var clubDTO = new ClubDTO { Name = newClub.Name! };

                // Add the new club to the database
                await _clubService.AddClubAsync(clubDTO);

                // Add a success message to TempData
                TempData["Success"] = $"{newClub.Name} has been added successfully";

                // Redirect to the index action
                return RedirectToAction("Index");
            } catch (DFCStatsException ex)
            {
                // Add a failure message to TempData
                TempData["Failure"] = ex.Message;
            }
        }

        //Set the page heading and the page title
		ViewData["PageHeading"] = "Create Club";
		ViewData["Title"] = "Create Club";

        // Return the view with the model to show the error
        return View(newClub);
    }

}