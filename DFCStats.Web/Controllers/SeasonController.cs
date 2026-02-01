using DFCStats.Business.Interfaces;
using DFCStats.Web.Models.Seasons;
using Microsoft.AspNetCore.Mvc;
using DFCStats.Domain.Exceptions;

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
        return View();
    }

    public async Task<IActionResult> New()
    {
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
                var seasonDTO = new Domain.DTOs.SeasonDTO { Description = newSeason.Description! };

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

        return View(newSeason);
    }
}