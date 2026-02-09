using Microsoft.AspNetCore.Mvc;
using DFCStats.Business.Interfaces;
using System.Text.Json;
using DFCStats.Web.Models.People;
using DFCStats.Domain.DTOs;
using DFCStats.Domain.Exceptions;

namespace DFCStats.Web.Controllers;

public class PersonController : Controller
{
    private readonly IPersonService _personService;
    private readonly INationalityService _nationalities;
    private readonly ISeasonService _seasons;

    public PersonController(IPersonService personService, INationalityService nationalities, ISeasonService seasons)
    {
        _personService = personService;
        _nationalities = nationalities;
        _seasons = seasons;
    }

    public async Task<IActionResult> New()
    {
        //Gets the nationalities from the database
        ViewBag.nationalities = await _nationalities.GetAllNationalitiesAsync();

        //Gets the seasons from the database
        //The seasons are also serialized into a json list so they can be used by javascript in a dropdown menu
        var seasons = await _seasons.GetAllSeasonsAsync();
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
                    ListOfSeasons = newPerson.Seasons?.Select(s => s.SeasonID).ToList() ?? new List<Guid>()
                };

                // Adds the new person to the database
                await _personService.AddPersonAsync(newPersonDTO);

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

        //Gets the nationalities from the database
        ViewBag.nationalities = await _nationalities.GetAllNationalitiesAsync();

        //Gets the seasons from the database
        //The seasons are also serialized into a json list so they can be used by javascript in a dropdown menu
        var seasons = await _seasons.GetAllSeasonsAsync();
        var seasonsJson = JsonSerializer.Serialize(seasons.Select(s => new { s.Id, s.Description }));
        ViewBag.seasonsJson = seasonsJson;
        ViewBag.seasons = seasons;

        // Return the view with the model to show the error
        return View(newPerson);
    }

}