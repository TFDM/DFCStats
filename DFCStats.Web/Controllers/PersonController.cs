using Microsoft.AspNetCore.Mvc;
using DFCStats.Business.Interfaces;

namespace DFCStats.Web.Controllers;

public class PersonController : Controller
{
    private readonly IPersonService _personService;

    public PersonController(IPersonService personService)
    {
        _personService = personService;
    }

    public async Task<IActionResult> Index()
    {
        var person = await _personService.GetPersonByIdAsync(new Guid ("E7E255F9-91CF-4D8D-B66A-005FD374E68F"));

        var anotherPerson = await _personService.GetPersonByIdAsync(new Guid("6D4751AB-2670-4614-B154-06CE96119EBD"));


        return View();
    }
}