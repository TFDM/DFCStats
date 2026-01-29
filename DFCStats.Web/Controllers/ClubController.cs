using Microsoft.AspNetCore.Mvc;
using DFCStats.Business.Interfaces;

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
}