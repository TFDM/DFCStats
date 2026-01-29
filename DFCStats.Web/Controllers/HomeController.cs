using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DFCStats.Web.Models;
using DFCStats.Business.Interfaces;
using DFCStats.Domain.DTOs;
using DFCStats.Domain.Entities;

namespace DFCStats.Web.Controllers;

public class HomeController : Controller
{
    public HomeController()
    {

    }

    public async Task<IActionResult> Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
