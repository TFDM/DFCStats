using Microsoft.AspNetCore.Mvc;
using DFCStats.Business.Interfaces;
using DFCStats.Web.Models.Clubs;
using DFCStats.Domain.Exceptions;

namespace DFCStats.Web.Controllers;

public class ClubController : Controller
{
    private readonly IClubService _clubService;

    public ClubController(IClubService clubSerice)
    {
        _clubService = clubSerice;
    }

    public async Task<IActionResult> Index()
    {
        var clubs = await _clubService.GetAllClubsAsync();
        return View();
    }

    public async Task<IActionResult> New()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> New(NewClub newClub)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // Add the new club to the database
                await _clubService.AddClub(new Domain.DTOs.ClubDTO { Name = newClub.Name});

            } catch (DFCStatsException ex)
            {
                // Add a failure message to TempData
                TempData["Failure"] = ex.Message;

                // Return the view with the model to show the error
                return View(newClub);
            }
        }

        // Add a success message to TempData
        TempData["Success"] = $"{newClub.Name} has been added successfully";

        // Redirect to the index action
        return RedirectToAction("Index");
    }

}