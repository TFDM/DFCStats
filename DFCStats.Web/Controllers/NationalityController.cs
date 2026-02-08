using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DFCStats.Business.Interfaces;
using DFCStats.Web.Models.Nationalities;
using DFCStats.Domain.Exceptions;
using X.PagedList;

namespace DFCStats.Web.Controllers;

public class NationalityController : Controller
{
    private readonly INationalityService _nationalityService;

    public NationalityController(INationalityService nationalityService)
    {
        _nationalityService = nationalityService;
    }

    public async Task<IActionResult> Index(string country, string nationality, string sort, int page = 1, int pageSize = 50)
    {
        // Ensure the page and page size are above not zero or negative
        page = (page < 1) ? 1 : page;
        pageSize = (pageSize < 1) ? 50 : pageSize;

        // Search for nationalities
        var (nationalities, totalCount) = await _nationalityService.SearchForNationalitiesAsync(page: page,
            pageSize: pageSize,
            searchCountry: country,
            searchNationality: nationality,
            sort: sort);

        // Convert the nationalities from a DTO to a model
        var listOfNationalities = nationalities.Select(dto => new Nationalities
        {
            Id = dto.Id,
            Nationality = dto.Nationality,
            Country = dto.Country,
            Icon = dto.Icon
        }).ToList();
        
        // Convert to a static list
		var nationalitiesAsIPagedList = new StaticPagedList<Nationalities>(listOfNationalities, page, pageSize, totalCount);

        // If the sort parameter is null or empty then we are initializing the value as descending  
        ViewBag.SortByNationality = string.IsNullOrEmpty(sort) ? "nationality_desc" : "";
        ViewBag.SortByCountry = sort == "country" ? "country_desc" : "country";
        ViewBag.Sort = sort;

        // Creates a select list of page sizes
        ViewBag.pageSize = new List<SelectListItem>()
        {
            new SelectListItem() { Text="25", Value="25" },
            new SelectListItem() { Text="50", Value="50" },
            new SelectListItem() { Text="75", Value="75" },
            new SelectListItem() { Text="100", Value="100" }
        };

        return View(nationalitiesAsIPagedList);
    }

    public async Task<IActionResult> New()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> New(NewNationality newNationality)
    {
        if(ModelState.IsValid)
        {
            try
            {
                // Convert the NewNationality model to a NationalityDTO
                var nationalityDTO = new Domain.DTOs.NationalityDTO{ 
                    Nationality = newNationality.Nationality,
                    Country = newNationality.Country,
                    Icon = newNationality.Icon
                };

                // Add the nationality to the databaase
                await _nationalityService.AddNationalityAsync(nationalityDTO);

                // Add a success message to TempData
                TempData["Success"] = $"{newNationality.Nationality} has been added successfully";

                // Redirect to the index action
                return RedirectToAction("Index");
            } catch (DFCStatsException ex)
            {
                // Add a failure message to TempData
                TempData["Failure"] = ex.Message;
            }
        }

        return View(newNationality);
    }

    public async Task<IActionResult> Edit(string id)
    {
        // Validate that the id parameter is a valid GUID format
        // the nationalityId is set to the guid if the parsing is successful
        if (!Guid.TryParse(id, out var nationalityId))
            // If the id is not a valid GUID, return a 400 Bad Request HTTP response
            return BadRequest("Invalid ID format");
        
        // Retrieve the nationality record from the database using the validated GUID
        var nationality = await _nationalityService.GetNationalityByIdAsync(nationalityId);

        // If the nationality record is not found, return a 404 Not Found HTTP response
        if (nationality == null)
            return NotFound("Nationality not found");
        
        // Convert the nationalityDTO to an EditNationality model
        var nationalityToEdit = new EditNationality
        { 
            Id = nationality.Id,
            Nationality = nationality.Nationality,
            Country = nationality.Country,
            Icon = nationality.Icon
        };

        // Return the view with the nationality data (assuming the view will handle displaying it)
        return View(nationalityToEdit);
    }

    [HttpPost]
    public async Task<IActionResult> Edit (EditNationality editNationality)
    {
        if(ModelState.IsValid)
        {
            try
            {
                // Convert the EditNationality model to a NationalityDTO
                var nationalityDTO = new Domain.DTOs.NationalityDTO{ 
                    Id = editNationality.Id,
                    Nationality = editNationality.Nationality,
                    Country = editNationality.Country,
                    Icon = editNationality.Icon
                };

                // Update the nationality in the database
                await _nationalityService.UpdateNationalityAsync(nationalityDTO);

                // Add a success message to TempData
                TempData["Success"] = $"{editNationality.Nationality} has been updated successfully";

                // Redirect to the index action
                return RedirectToAction("Index");
            } catch (DFCStatsException ex)
            {
                // Add a failure message to TempData
                TempData["Failure"] = ex.Message;
            }
        }

        return View(editNationality);
    }
}