using Microsoft.AspNetCore.Mvc;
using DFCStats.Business.Interfaces;
using System.Text.Json;
using DFCStats.Web.Models.People;
using DFCStats.Domain.DTOs.People;
using DFCStats.Domain.Exceptions;

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
        var person = await _personService.GetPersonByIdAsync(Guid.Parse("86EB50B8-53E0-4DFD-8EA5-08DE883568A6"));

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

}