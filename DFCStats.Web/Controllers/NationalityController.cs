using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DFCStats.Business.Interfaces;
using DFCStats.Web.Models.Nationalities;
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
            Nationality = dto.Name,
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
}