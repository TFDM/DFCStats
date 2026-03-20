using Microsoft.AspNetCore.Mvc;
using DFCStats.Business.Interfaces;
using DFCStats.Web.Models.Fixtures;
using DFCStats.Domain.DTOs.Fixtures;
using DFCStats.Domain.Exceptions;

namespace DFCStats.Web.Controllers;

public class FixtureController : Controller
{
    private readonly ISeasonService _seasonService;
    private readonly ICategoryService _categoryService;
    private readonly IVenueService _venueService;
    private readonly IClubService _clubService;
    private readonly IFixtureService _fixtureService;
    
    public FixtureController(ISeasonService seasonService, ICategoryService categoryService, IVenueService venueService, IClubService clubService, IFixtureService fixtureService)
    {
        _seasonService = seasonService;
        _categoryService = categoryService;
        _venueService = venueService;
        _clubService = clubService;
        _fixtureService = fixtureService;
    }

    public async Task<IActionResult> New()
    {
        //Set the page heading and the page title
		ViewData["PageHeading"] = "Create Fixture";
		ViewData["Title"] = "Create Fixture";

        // Get all the venues, seasons, categories and clubs to populate the dropdown lists
        var venues = await _venueService.GetAllVenuesAsync("orderNo");        
        var seasons = await _seasonService.GetAllSeasonsAsync("description");
        var categories = await _categoryService.GetAllCategoriesAsync("orderNo");
        var clubs = await _clubService.GetAllClubsAsync("name");

        ViewBag.venues = venues;
        ViewBag.seasons = seasons;
        ViewBag.categories = categories;
        ViewBag.clubs = clubs;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> New(NewFixture newFixture)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // Convert the newFixture model to a DTO
                var newFixtureDTO = new NewFixtureDTO
                {
                    SeasonId = newFixture.SeasonId,
                    Date = newFixture.Date,
                    ClubId = newFixture.ClubId,
                    CategoryId = newFixture.CategoryId,
                    Competition = newFixture.Competition,
                    VenueId = newFixture.VenueId,
                    DarlingtonScore = newFixture.DarlingtonScore,
                    OppositionScore = newFixture.OppositionScore,
                    PenaltiesRequired = newFixture.PenaltiesRequired,
                    DarlingtonPenaltyScore = newFixture.DarlingtonPenaltyScore,
                    OppositionPenaltyScore = newFixture.OppositionPenaltyScore,
                    Attendance = newFixture.Attendance,
                    Notes = newFixture.Notes
                };

                // Adds the new fixture to the database
                await _fixtureService.AddFixtureAsync(newFixtureDTO);

                // Add a success message to TempData
                TempData["Success"] = "Fixture has been added successfully";

                // Redirect to the index action
                return RedirectToAction("Index");
            } catch (DFCStatsException ex)
            {
                // Add a failure message to TempData
                TempData["Failure"] = ex.Message;
            }
        }

        //Set the page heading and the page title
		ViewData["PageHeading"] = "Create Fixture";
		ViewData["Title"] = "Create Fixture";

        // Get all the venues, seasons, categories and clubs to populate the dropdown lists
        var venues = await _venueService.GetAllVenuesAsync("orderNo");        
        var seasons = await _seasonService.GetAllSeasonsAsync("description");
        var categories = await _categoryService.GetAllCategoriesAsync("orderNo");
        var clubs = await _clubService.GetAllClubsAsync("name");

        ViewBag.venues = venues;
        ViewBag.seasons = seasons;
        ViewBag.categories = categories;
        ViewBag.clubs = clubs;

        return View(newFixture);
    }
}