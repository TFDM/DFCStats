using DFCStats.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
}