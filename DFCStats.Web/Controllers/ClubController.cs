using Microsoft.AspNetCore.Mvc;
using DFCStats.Business.Interfaces;
using DFCStats.Web.Models.Clubs;
using DFCStats.Domain.Exceptions;

namespace DFCStats.Web.Controllers;

public class ClubController : Controller
{
    private readonly IClubService _clubService;

    public ClubController(IClubService clubService)
    {
        _clubService = clubService;
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
                // Convert the NewClub model to a ClubDTO
                var clubDTO = new Domain.DTOs.ClubDTO { Name = newClub.Name! };

                // Add the new club to the database
                await _clubService.AddClubAsync(clubDTO);

                // Add a success message to TempData
                TempData["Success"] = $"{newClub.Name} has been added successfully";

                // Redirect to the index action
                return RedirectToAction("Index");
            } catch (DFCStatsException ex)
            {
                // Add a failure message to TempData
                TempData["Failure"] = ex.Message;
            }
        }

        // Return the view with the model to show the error
        return View(newClub);
    }

}