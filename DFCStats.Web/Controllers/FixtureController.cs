using Microsoft.AspNetCore.Mvc;
using DFCStats.Business.Interfaces;
using DFCStats.Web.Models.Fixtures;
using DFCStats.Domain.DTOs.Fixtures;
using DFCStats.Domain.Exceptions;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DFCStats.Web.Controllers;

public class FixtureController : Controller
{
    private readonly ISeasonService _seasonService;
    private readonly ICategoryService _categoryService;
    private readonly IVenueService _venueService;
    private readonly IClubService _clubService;
    private readonly IFixtureService _fixtureService;
    private readonly IParticipationService _participationService;
    
    public FixtureController(ISeasonService seasonService, ICategoryService categoryService, IVenueService venueService, IClubService clubService, IFixtureService fixtureService, 
    IParticipationService participationService)
    {
        _seasonService = seasonService;
        _categoryService = categoryService;
        _venueService = venueService;
        _clubService = clubService;
        _fixtureService = fixtureService;
        _participationService = participationService;
    }

    public async Task<IActionResult> Index(string season, string opponent, string competition, string venue, string outcome, string category, string sort, int page = 1, int pageSize = 50)
    {
        // Set the page heading and the page title
		ViewData["PageHeading"] = "Fixtures and Results";
		ViewData["Title"] = "Fixtures and Results";

        // Ensure the page and page size are above not zero or negative
        page = (page < 1) ? 1 : page;
        pageSize = (pageSize < 1) ? 50 : pageSize;

        // Gets all the venues, clubs, seasons and categories and seasons from the database
        var venues = await _venueService.GetAllVenuesAsync("orderNo");
        var seasons = await _seasonService.GetAllSeasonsAsync("description");
        var categories = await _categoryService.GetAllCategoriesAsync("orderNo");
        var clubs = await _clubService.GetAllClubsAsync("name");

        ViewBag.venues = venues;
        ViewBag.seasons = seasons;
        ViewBag.categories = categories;
        ViewBag.clubs = clubs;

        // Creates a select list from the outcomes
        ViewBag.outcomes = new List<SelectListItem>()
        {
            new SelectListItem() { Text="Win", Value="W" },
            new SelectListItem() { Text="Loss", Value="L" },
            new SelectListItem() { Text="Draw", Value="D" }
        };

        // Creates a select list of page sizes
        ViewBag.pageSize = new List<SelectListItem>()
        {
            new SelectListItem() { Text="25", Value="25" },
            new SelectListItem() { Text="50", Value="50" },
            new SelectListItem() { Text="75", Value="75" },
            new SelectListItem() { Text="100", Value="100" }
        };

        // Search for fixtures
        var (fixtures, totalCount) = await _fixtureService.SearchForFixturesAsync(page: page,
            pageSize: pageSize,
            searchSeason: season,
            searchOpponent: opponent,
            searchCompetition: competition,
            searchVenue: venue,
            searchOutcome: outcome,
            searchCategory: category,
            sort: sort);

        // Convert the nationalities from a DTO to a model
        var listOfFixtures = fixtures.Select(dto => new Fixtures
        {
            Id = dto.Id,
            Teams = dto.Teams,
            Competition = dto.Competition,
            Outcome = dto.Outcome,
            Season = dto.Season,
            Scoreline = dto.Scoreline,
            Date = dto.Date,
            Attendance = string.Format("{0:n0}", dto.Attendance)
        }).ToList();
        
        // Convert to a static list
		var fixturesAsIPagedList = new StaticPagedList<Fixtures>(listOfFixtures, page, pageSize, totalCount);

        // If the sort parameter is null or empty then we are initializing the value as descending  
        ViewBag.SortByDate = string.IsNullOrEmpty(sort) ? "date" : "";
        ViewBag.SortBySeason = sort == "season" ? "season_desc" : "season";
        ViewBag.SortByAttendance = sort == "attendance" ? "attendance_desc" : "attendance";
        ViewBag.Sort = sort;

        return View(fixturesAsIPagedList);
    }

    public async Task<IActionResult> Details(string id)
    {
        // Validate that the id parameter is a valid GUID format
        // the fixtureId is set to the guid if the parsing is successful
        if (!Guid.TryParse(id, out var fixtureId))
            // If the id is not a valid GUID, return a 400 Bad Request HTTP response
            return BadRequest("Invalid ID format");

        // Get the fixture from the database and include the participants
        var fixture = await _fixtureService.GetFixtureByIdAsync(fixtureId, FixtureIncludes.Participants);

        // Check if the fixture was found
        if (fixture == null)
            return NotFound("Fixture not found");

        // Set the page heading and the page title
		ViewData["PageHeading"] = fixture.TeamsAndScores;
		ViewData["Title"] = fixture.TeamsAndScores;

        // Map the fixture to the model for displaying on the page
        var fixtureToDisplay = new Fixtures
        {
            Id = fixture.Id,
            Season = fixture.Season,
            Date = fixture.Date,
            Attendance = string.Format("{0:n0}", fixture.Attendance),
            Competition = fixture.Competition,
            Outcome = fixture.Outcome,
            Scoreline = fixture.Scoreline,
            Teams = fixture.Teams,
            TeamsWithScore = fixture.TeamsAndScores,
            PenaltiesRequired = fixture.PenaltiesRequired,
            PenaltyScoreWithOutcome = fixture.PenaltyScoreWithOutcome,
            Venue = fixture.VenueShort,
            LineUp = fixture.Participants?.Select(p => new LineUp
            {
                Id = p.Id,
                OrderNo = p.OrderNo,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Role = p.Role,
                Started = p.Started,
                Sub = p.Sub,
                YellowCard = p.YellowCard,
                RedCard = p.RedCard,
                ReplacedByPersonId = p.ReplacedByPersonId,
                ReplacedByFirstName = p.ReplacedByFirstName,
                ReplacedByLastName = p.ReplacedByLastName,
                ReplacedByTime = p.ReplacedByTime
            }).ToList()
        };

        return View(fixtureToDisplay);
    }

    public async Task<IActionResult> New()
    {
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
                var newFixtureDTO = new FixtureDTO
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
                var editFixtureDTO = new FixtureDTO
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