using DFCStats.Business.Interfaces;
using DFCStats.Web.Models.Seasons;
using Microsoft.AspNetCore.Mvc;
using DFCStats.Domain.Exceptions;
using DFCStats.Domain.DTOs.Seasons;

namespace DFCStats.Web.Controllers;

public class SeasonController : Controller
{
    private readonly ISeasonService _seasonService;

    public SeasonController(ISeasonService seasonService)
    {
        _seasonService = seasonService;
    }

    public async Task<IActionResult> Index()
    {
        //Set the page heading and the page title
		ViewData["PageHeading"] = "Seasons";
		ViewData["Title"] = "Seasons";

        return View();
    }

    public async Task<IActionResult> New()
    {
        //Set the page heading and the page title
		ViewData["PageHeading"] = "Create Season";
		ViewData["Title"] = "Create Season";

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> New(NewSeason newSeason)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // Convert the NewSeason model to a SeasonDTO
                var seasonDTO = new SeasonDTO { Description = newSeason.Description! };

                // Add the new season to the database
                await _seasonService.AddSeasonAsync(seasonDTO);
                
                // Add a success message to TempData
                TempData["Success"] = $"{newSeason.Description} has been added successfully";

                // Redirect to the index action
                return RedirectToAction("Index");
            } catch (DFCStatsException ex)
            {
                // Add a failure message to TempData
                TempData["Failure"] = ex.Message;
            }
        }

        //Set the page heading and the page title
		ViewData["PageHeading"] = "Create Season";
		ViewData["Title"] = "Create Season";

        return View(newSeason);
    }
}