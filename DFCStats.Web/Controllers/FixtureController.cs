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

    public async Task<IActionResult> Index()
    {
        var x = await _fixtureService.GetFixtureByIdAsync(Guid.Parse("3EBFBBB9-7EBB-40B8-B2B7-08DE86BA5E65"));

        return View();
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

        // Set the page heading and the page title
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

    public async Task<IActionResult> Edit(string id)
    {
        //Set the page heading and the page title
		ViewData["PageHeading"] = "Edit Fixture";
		ViewData["Title"] = "Edit Fixture";

        // Validate that the id parameter is a valid GUID format
        // the fixtueId is set to the guid if the parsing is successful
        if (!Guid.TryParse(id, out var fixtureId))
            // If the id is not a valid GUID, return a 400 Bad Request HTTP response
            return BadRequest("Invalid ID format");

        // Retrieve the fixture record from the database using the validated GUID
        var fixture = await _fixtureService.GetFixtureByIdAsync(fixtureId);

        // If the fixture record is not found, return a 404 Not Found HTTP response
        if (fixture == null)
            return NotFound("Fixture not found");

        // Convert the personDTO to an EditPerson model
        var fixtureToEdit = new EditFixture
        { 
            Id = fixture.Id,
            SeasonId = fixture.SeasonId,
            Date = fixture.Date,
            ClubId = fixture.ClubId,
            CategoryId = fixture.CategoryId,
            Competition = fixture.Competition,
            VenueId = fixture.VenueId,
            DarlingtonScore = fixture.DarlingtonScore,
            OppositionScore = fixture.OppositionScore,
            PenaltiesRequired = fixture.PenaltiesRequired,
            DarlingtonPenaltyScore = fixture.DarlingtonPenaltyScore,
            OppositionPenaltyScore = fixture.OppositionPenaltyScore,
            Attendance = fixture.Attendance,
            Notes = fixture.Notes
        };

        // Get all the venues, seasons, categories and clubs to populate the dropdown lists
        var venues = await _venueService.GetAllVenuesAsync("orderNo");        
        var seasons = await _seasonService.GetAllSeasonsAsync("description");
        var categories = await _categoryService.GetAllCategoriesAsync("orderNo");
        var clubs = await _clubService.GetAllClubsAsync("name");

        ViewBag.venues = venues;
        ViewBag.seasons = seasons;
        ViewBag.categories = categories;
        ViewBag.clubs = clubs;

        return View(fixtureToEdit);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditFixture editFixture)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // Convert the EditFixture model to a EditFixtureDTO
                var editFixtureDTO = new EditFixtureDTO
                {
                    Id = editFixture.Id,
                    SeasonId = editFixture.SeasonId,
                    Date = editFixture.Date,
                    ClubId = editFixture.ClubId,
                    CategoryId = editFixture.CategoryId,
                    Competition = editFixture.Competition,
                    VenueId = editFixture.VenueId,
                    DarlingtonScore = editFixture.DarlingtonScore,
                    OppositionScore = editFixture.OppositionScore,
                    PenaltiesRequired = editFixture.PenaltiesRequired,
                    DarlingtonPenaltyScore = editFixture.DarlingtonPenaltyScore,
                    OppositionPenaltyScore = editFixture.OppositionPenaltyScore,
                    Attendance = editFixture.Attendance,
                    Notes = editFixture.Notes
                };

                // Update the fixture in the database
                await _fixtureService.UpdateFixtureAsync(editFixtureDTO);

                // Add a success message to TempData
                TempData["Success"] = "Fixture has been updated successfully";

                // Redirect to the index action
                return RedirectToAction("Index");
            } catch (DFCStatsException ex)
            {
                // Add a failure message to TempData
                TempData["Failure"] = ex.Message;
            }
        }

        // Set the page heading and the page title
		ViewData["PageHeading"] = "Edit Fixture";
		ViewData["Title"] = "Edit Fixture";

        // Get all the venues, seasons, categories and clubs to populate the dropdown lists
        var venues = await _venueService.GetAllVenuesAsync("orderNo");        
        var seasons = await _seasonService.GetAllSeasonsAsync("description");
        var categories = await _categoryService.GetAllCategoriesAsync("orderNo");
        var clubs = await _clubService.GetAllClubsAsync("name");

        ViewBag.venues = venues;
        ViewBag.seasons = seasons;
        ViewBag.categories = categories;
        ViewBag.clubs = clubs;

        return View(editFixture);
    }
}