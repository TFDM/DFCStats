using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DFCStats.Business.Interfaces;
using DFCStats.Web.Models.Managers;
using X.PagedList;
using X.PagedList.Extensions;
using DFCStats.Domain.DTOs.Managers;
using DFCStats.Domain.Exceptions;

namespace DFCStats.Web.Controllers;

public class ManagerController : Controller
{
    private readonly IManagerService _managerService;
    private readonly IPersonService _personService;
    
    public ManagerController(IManagerService managerService, IPersonService personService)
    {
        _managerService = managerService;
        _personService = personService;
    }

    public async Task<IActionResult> Index(bool includeCaretakers, string sort, int page = 1, int pageSize = 50)
    {
        // Set the page heading and the page title
		ViewData["PageHeading"] = "Managers";
		ViewData["Title"] = "Managers";

        // Ensure the page and page size are above not zero or negative
        page = (page < 1) ? 1 : page;
        pageSize = (pageSize < 1) ? 50 : pageSize;

        // Creates a select list for the include caretakers dropdown
        ViewBag.includeCaretakers = new List<SelectListItem>()
        {
            new SelectListItem() { Text="Yes", Value="true" },
            new SelectListItem() { Text="No", Value="false" }
        };

        // Search for management records
        var (managers, totalCount) = await _managerService.GetManagementRecordsWithPaginationAsync(
            page: page, 
            pageSize: pageSize, 
            includeCaretakers: includeCaretakers, 
            sort: sort);

        // Convert the managers from a DTO to a model
        var listOfManagers = managers.Select(dto => new Manager
        {
            Id = dto.Id,
            Name = dto.LastNameFirstName!,
            Nationality = dto.Nationality,
            NationalityIcon = dto.NationalityIcon,
            DateFrom = dto.StartDate,
            DateTo = dto.EndDate,
            TimeInCharge = dto.TimeInChargeAsString!,
            CurrentlyOnGoing = (dto.EndDate == null) ? true : false,
            Caretaker = dto.IsCaretaker,
            GamesManaged = dto.NumberOfGamesManaged!.Value,
            Wins = dto.Wins!.Value,
            Draws = dto.Draws!.Value,
            Loses = dto.Losses!.Value,
            GoalsFor = dto.GoalsFor!.Value,
            GoalsAgainst = dto.GoalsAgainst!.Value,
            WinPercentage = (dto.WinPercentage != null) ? string.Format("{0:0}%", dto.WinPercentage) : null
        }).ToList();

        // Convert to a static list
		var managersAsIPagedList = new StaticPagedList<Manager>(listOfManagers, page, pageSize, totalCount);

        // If the sort parameter is null or empty then we are initializing the value as descending
        ViewBag.SortByStartDate = string.IsNullOrEmpty(sort) ? "startDate" : "";
        ViewBag.SortByEndDate = sort == "endDate" ? "endDate_desc" : "endDate";
        ViewBag.SortByName = sort == "managerName" ? "managerName_desc" : "managerName";
        ViewBag.SortByTimeInCharge = sort == "timeInCharge" ? "timeInCharge_desc" : "timeInCharge";
        ViewBag.SortByNationality = sort == "nationality" ? "nationality_desc" : "nationality";
        ViewBag.SortByGamesManaged = sort == "gamesManaged" ? "gamesManaged_desc" : "gamesManaged";
        ViewBag.SortByGamesWon = sort == "won" ? "won_desc" : "won";
        ViewBag.SortByGamesDrawn = sort == "drawn" ? "drawn_desc" : "drawn";
        ViewBag.SortByGamesLost = sort == "lost" ? "lost_desc" : "lost";
        ViewBag.SortByWinPercentage = sort == "winsPercentage" ? "winsPercentage_desc" : "winsPercentage";
        ViewBag.SortByGoalsFor = sort == "goalsFor" ? "goalsFor_desc" : "goalsFor";
        ViewBag.SortByGoalsAgainst = sort == "goalsAgainst" ? "goalsAgainst_desc" : "goalsAgainst";
        ViewBag.Sort = sort;

        return View(managersAsIPagedList);
    }

    public async Task<IActionResult> New()
    {
        //Set the page heading and the page title
		ViewData["PageHeading"] = "Create Manager Record";
		ViewData["Title"] = "Create Manager Record";

        // Get all the people who are managers
        var managers = await _personService.GetPeopleWhoAreManagersAsync(sort: "lastName");

        // Creates the view model and populates the manager options and caretaker options
        var viewModel = new NewManager
        {
            ManagerOptions = managers.Select(dto => new SelectListItem
            {
                Value = dto.Id.ToString(),
                Text = dto.LastNameFirstName
            }).ToList(),
    
            CaretakerOptions = new List<SelectListItem>()
            {
                new SelectListItem() { Text="Yes", Value="true" },
                new SelectListItem() { Text="No", Value="false" }
            }
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> New(NewManager newManager)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // Convert the model to a DTO
                var managementRecordDTO = new ManagementRecordDTO {
                    PersonId = newManager.PersonId, 
                    StartDate = newManager.StartDate!.Value, 
                    EndDate = newManager.EndDate, 
                    IsCaretaker = newManager.IsCaretaker!.Value };

                // Attempt to add the new management record
                await _managerService.AddManagerRecordAsync(managementRecordDTO);

                // Add a success message to TempData
                TempData["Success"] = "Management record added successfully";

                // Redirect to the index page
                return RedirectToAction("Index");
            } catch (DFCStatsException ex)
            {
                // Add a failure message to TempData
                TempData["Failure"] = ex.Message;
            }
        }

        //Set the page heading and the page title
		ViewData["PageHeading"] = "Create Manager Record";
		ViewData["Title"] = "Create Manager Record";

        // If the model state is not valid, repopulate the manager options and caretaker options
        var managers = await _personService.GetPeopleWhoAreManagersAsync(sort: "lastName");

        // Set the manager options and caretaker options for the view model
        newManager.ManagerOptions = managers.Select(dto => new SelectListItem
        {
            Value = dto.Id.ToString(),
            Text = dto.LastNameFirstName
        }).ToList();

        newManager.CaretakerOptions = new List<SelectListItem>()
        {
            new SelectListItem() { Text="Yes", Value="true" },
            new SelectListItem() { Text="No", Value="false" }
        };

        return View(newManager);
    }

    public async Task<IActionResult> Edit(string id)
    {
        //Set the page heading and the page title
		ViewData["PageHeading"] = "Edit Manager Record";
		ViewData["Title"] = "Edit Manager Record";

        // Validate that the id parameter is a valid GUID format
        // the managerId is set to the guid if the parsing is successful
        if (!Guid.TryParse(id, out var managerId))
            // If the id is not a valid GUID, return a 400 Bad Request HTTP response
            return BadRequest("Invalid ID format");

        // Retrieve the manager record from the database using the validated GUID
        var managerRecord = await _managerService.GetManagementRecordByIdAsync(managerId, ManagerIncludes.All);

        // If the manager record is not found, return a 404 Not Found HTTP response
        if (managerRecord == null)
            return NotFound("Manager record not found");

        // Get all the people who are managers
        var managers = await _personService.GetPeopleWhoAreManagersAsync(sort: "lastName");

        // Creates the view model and populates the manager options and caretaker options
        var viewModel = new EditManager
        {
            // Set the properties of the view model using the retrieved manager record
            Id = managerRecord.Id,
            PersonId = managerRecord.PersonId,
            StartDate = managerRecord.StartDate,
            EndDate = managerRecord.EndDate,
            IsCaretaker = managerRecord.IsCaretaker,

            ManagerOptions = managers.Select(dto => new SelectListItem
            {
                Value = dto.Id.ToString(),
                Text = dto.LastNameFirstName
            }).ToList(),
    
            CaretakerOptions = new List<SelectListItem>()
            {
                new SelectListItem() { Text="Yes", Value="true" },
                new SelectListItem() { Text="No", Value="false" }
            }
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditManager editManager)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // Convert the model to a DTO
                var managementRecordDTO = new ManagementRecordDTO {
                    Id = editManager.Id,
                    PersonId = editManager.PersonId, 
                    StartDate = editManager.StartDate!.Value, 
                    EndDate = editManager.EndDate, 
                    IsCaretaker = editManager.IsCaretaker!.Value };

                // Attempt to update the management record
                await _managerService.UpdateManagerRecordAsync(managementRecordDTO);

                // Add a success message to TempData
                TempData["Success"] = "Management record updated successfully";

                return RedirectToAction("Index");
            } catch (DFCStatsException ex)
            {
                // Add a failure message to TempData
                TempData["Failure"] = ex.Message;
            }
        }

        //Set the page heading and the page title
		ViewData["PageHeading"] = "Edit Manager Record";
		ViewData["Title"] = "Edit Manager Record";

        // If the model state is not valid, repopulate the manager options and caretaker options
        var managers = await _personService.GetPeopleWhoAreManagersAsync(sort: "lastName");

        // Set the manager options and caretaker options for the view model
        editManager.ManagerOptions = managers.Select(dto => new SelectListItem
        {
            Value = dto.Id.ToString(),
            Text = dto.LastNameFirstName
        }).ToList();

        editManager.CaretakerOptions = new List<SelectListItem>()
        {
            new SelectListItem() { Text="Yes", Value="true" },
            new SelectListItem() { Text="No", Value="false" }
        };

        return View(editManager);
    }
}