using Microsoft.AspNetCore.Mvc;
using DFCStats.Business.Interfaces;

namespace DFCStats.Web.Controllers;

public class TableController : Controller
{
    private readonly ITableService _tableService;

    public TableController(ITableService tableService)
    {
        _tableService = tableService;
    }

    public IActionResult Index()
    {
        return View();
    }
}