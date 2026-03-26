using Microsoft.AspNetCore.Mvc;
using DFCStats.Business.Interfaces;
using System.Text.Json;
using DFCStats.Web.Models.People;
using DFCStats.Domain.DTOs.People;
using DFCStats.Domain.Exceptions;
using DFCStats.Business;

namespace DFCStats.Web.Controllers;

public class PersonController : Controller
{
    private readonly IPersonService _personService;
    private readonly INationalityService _nationalityService;
    private readonly ISeasonService _seasonService;

    public PersonController(IPersonService personService, INationalityService nationalityService, ISeasonService seasonService)
    {
        _personService = personService;
        _nationalityService = nationalityService;
        _seasonService = seasonService;
    }

    public async Task<IActionResult> Index()
    {
        var person = await _personService.GetPersonByIdAsync(Guid.Parse("548aa6a2-c4dd-4511-b466-02056f5b9ef7"), PersonIncludes.All);

        return View();
    }

    public async Task<IActionResult> New()
    {
        //Set the page heading and the page title
		ViewData["PageHeading"] = "Create Person";
		ViewData["Title"] = "Create Person";

        //Gets the nationalities from the database
        ViewBag.nationalities = await _nationalityService.GetAllNationalitiesAsync();

        //Gets the seasons from the database
        //The seasons are also serialized into a json list so they can be used by javascript in a dropdown menu
        var seasons = await _seasonService.GetAllSeasonsAsync("description_desc");
        var seasonsJson = JsonSerializer.Serialize(seasons.Select(s => new { s.Id, s.Description }));
        ViewBag.seasonsJson = seasonsJson;
        ViewBag.seasons = seasons;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> New(NewPerson newPerson)
    {
        if(ModelState.IsValid)
        {
            try
            {
                // Convert the NewPerson model to a DTO
                var newPersonDTO = new NewPersonDTO
                {
                    FirstName = newPerson.FirstName,
                    LastName = newPerson.LastName,
                    DateOfBirth = newPerson.DateOfBirth,
                    NationalityId = newPerson.NationalityId,
                    Biography = newPerson.Biography,
                    ListOfSeasons = newPerson.Seasons?.Select(s => s.SeasonId).ToList() ?? new List<Guid>()
                };

                // Adds the new person to the database
                var x = await _personService.AddPersonAsync(newPersonDTO);

                // Add a success message to TempData
                TempData["Success"] = $"{newPerson.FirstName} {newPerson.LastName} has been added successfully";

                // Redirect to the index action
                return RedirectToAction("Index");
            } catch (DFCStatsException ex)
            {
                // Add a failure message to TempData
                TempData["Failure"] = ex.Message;
            }
        }

        //Set the page heading and the page title
		ViewData["PageHeading"] = "Create Person";
		ViewData["Title"] = "Create Person";

        //Gets the nationalities from the database
        ViewBag.nationalities = await _nationalityService.GetAllNationalitiesAsync();

        //Gets the seasons from the database
        //The seasons are also serialized into a json list so they can be used by javascript in a dropdown menu
        var seasons = await _seasonService.GetAllSeasonsAsync("description_desc");
        var seasonsJson = JsonSerializer.Serialize(seasons.Select(s => new { s.Id, s.Description }));
        ViewBag.seasonsJson = seasonsJson;
        ViewBag.seasons = seasons;

        // Return the view with the model to show the error
        return View(newPerson);
    }

    public async Task<IActionResult> Edit(string id)
    {
        //Set the page heading and the page title
		ViewData["PageHeading"] = "Edit Person";
		ViewData["Title"] = "Edit Person";

        // Validate that the id parameter is a valid GUID format
        // the personId is set to the guid if the parsing is successful
        if (!Guid.TryParse(id, out var personId))
            // If the id is not a valid GUID, return a 400 Bad Request HTTP response
            return BadRequest("Invalid ID format");

        // Retrieve the person record from the database using the validated GUID
        var person = await _personService.GetPersonByIdAsync(personId, PersonIncludes.Nationality | PersonIncludes.Seasons | PersonIncludes.Stats );

        // If the person record is not found, return a 404 Not Found HTTP response
        if (person == null)
            return NotFound("Person not found");
        
        // Convert the personDTO to an EditPerson model
        var personToEdit = new EditPerson
        { 
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            DateOfBirth = person.DateOfBirth,
            NationalityId = person.NationalityId,
            Biography = person.Biography,
            IsManager = person.IsManager,
            Seasons = person.Seasons?.Select(ps => new DFCStats.Web.Models.People.Season { SeasonId = ps.Id, Description = ps.Description }).OrderBy(ps => ps.Description).ToList()
        };

        //Gets the nationalities from the database
        ViewBag.nationalities = await _nationalityService.GetAllNationalitiesAsync();

        //Gets the seasons from the database
        //The seasons are also serialized into a json list so they can be used by javascript in a dropdown menu
        var seasons = await _seasonService.GetAllSeasonsAsync("description_desc");
        var seasonsJson = JsonSerializer.Serialize(seasons.Select(s => new { s.Id, s.Description }));
        ViewBag.seasonsJson = seasonsJson;
        ViewBag.seasons = seasons;

        // Return the view
        return View(personToEdit);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditPerson editPerson)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // Convert the EditPerson model to an EditPersonDTO
                var editPersonDTO = new EditPersonDTO{ 
                    Id = editPerson.Id,
                    FirstName = editPerson.FirstName,
                    LastName = editPerson.LastName,
                    DateOfBirth = editPerson.DateOfBirth,
                    NationalityId = editPerson.NationalityId,
                    IsManager = editPerson.IsManager,
                    Biography = editPerson.Biography,
                    ListOfSeasons = editPerson.Seasons?.Select(s => s.SeasonId).ToList() ?? new List<Guid>()
                };

                // Update the person in the database
                var x = await _personService.UpdatePersonAsync(editPersonDTO);

                // Add a success message to TempData
                TempData["Success"] = $"{editPerson.FirstName} {editPerson.LastName} has been updated successfully";

                // Redirect to the index action
                return RedirectToAction("Index");
            } catch (DFCStatsException ex)
            {
                // Add a failure message to TempData
                TempData["Failure"] = ex.Message;
            }
        }

        //Set the page heading and the page title
		ViewData["PageHeading"] = "Edit Person";
		ViewData["Title"] = "Edit Person";

        //Gets the nationalities from the database
        ViewBag.nationalities = await _nationalityService.GetAllNationalitiesAsync();

        //Gets the seasons from the database
        //The seasons are also serialized into a json list so they can be used by javascript in a dropdown menu
        var seasons = await _seasonService.GetAllSeasonsAsync("description_desc");
        var seasonsJson = JsonSerializer.Serialize(seasons.Select(s => new { s.Id, s.Description }));
        ViewBag.seasonsJson = seasonsJson;
        ViewBag.seasons = seasons;

        return View(editPerson);
    }

    public async Task<IActionResult> Details(string id)
    {
        // Validate that the id parameter is a valid GUID format
        // the personId is set to the guid if the parsing is successful
        if (!Guid.TryParse(id, out var personId))
            // If the id is not a valid GUID, return a 400 Bad Request HTTP response
            return BadRequest("Invalid ID format");

        // Get the person from the database
        var person = await _personService.GetPersonByIdAsync(personId, PersonIncludes.Nationality | PersonIncludes.Stats);

        // Check if the person was found
        if (person == null)
            return NotFound("Person not found");

        // Set the page heading and the page title
		ViewData["PageHeading"] = String.Format("{0} {1}", person.FirstName, person.LastName);
		ViewData["Title"] = "Players and Staff";

        // Map the personDTO to a person model
        var personToDisplay = new Person
        {
            Id = person.Id,
            DateOfBirth = person.DateOfBirth,
            Nationality = person.Nationality,
            NationalityIcon = person.NationalityIcon,
            Biography = person.Biography,
            TotalStarts = person.TotalStarts,
            TotalSubs = person.TotalSubs,
            TotalGoals = person.TotalGoals,
            TotalRedCards = person.TotalRedCards,
            TotalLeagueStarts = person.TotalLeagueStarts,
            TotalLeagueSubs = person.TotalLeagueSubs,
            TotalLeagueGoals = person.TotalLeagueGoals,
            TotalCupStarts = person.TotalCupStarts,
            TotalCupSubs = person.TotalCupSubs,
            TotalCupGoals = person.TotalCupGoals,
            TotalPlayOffStarts = person.TotalPlayOffStarts,
            TotalPlayOffSubs = person.TotalPlayOffSubs,
            TotalPlayOffGoals = person.TotalPlayOffGoals,
            AppearancesBySeason = person.Appearances?.Select(a => new SeasonalAppearances
            {
                SeasonId = a.SeasonId,
                SeasonDescription = a.SeasonDescription,
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
            }).ToList()
        };

        // Check has appearances
        if (person.Appearances?.Count != 0)
        {
            // Get the fixtures the person appeared in for the final season we have appearances for them
            var participatedFixturesForFinalSeason = await _personService.GetParticipatedFixturesAsync(person.Id, (Guid)person.Appearances!.Last().SeasonId!);

            // Map to view model
            var gamesForFinalSeason = participatedFixturesForFinalSeason.Select(p => new ParticipatedFixtures
            {
                ParticipationId = p.ParticipationId,
                FixtureId = p.FixtureId,
                Date = p.Date,
                TeamsWithScore = p.TeamsWithScore,
                Competition = p.Competition,
                Scoreline = p.Scoreline,
                Outcome = p.Outcome,
                Season = p.Season,
                Role = p.Role
            }).ToList();

            ViewBag.participatedFixturesForFinalSeason = gamesForFinalSeason;
        }

        return View(personToDisplay);
    }

    public async Task<IActionResult> RefreshParticipationForSeason(string personId, string seasonId)
    {
        // Get the fixtures participated in by the person for the selected season
        var participatedFixturesForFinalSeason = await _personService.GetParticipatedFixturesAsync(Guid.Parse(personId), Guid.Parse(seasonId));

        // Map to view model
        var gamesForFinalSeason = participatedFixturesForFinalSeason.Select(p => new ParticipatedFixtures
        {
            ParticipationId = p.ParticipationId,
            FixtureId = p.FixtureId,
            Date = p.Date,
            TeamsWithScore = p.TeamsWithScore,
            Competition = p.Competition,
            Scoreline = p.Scoreline,
            Outcome = p.Outcome,
            Season = p.Season,
            Role = p.Role
        }).ToList();

        return PartialView("Partial_FixtureParticipationForSeason", gamesForFinalSeason);
    }

}