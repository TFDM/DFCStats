using Microsoft.AspNetCore.Mvc;
using DFCStats.Business.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using DFCStats.Web.Models.Participants;
using DFCStats.Domain.Exceptions;

namespace DFCStats.Web.Controllers;

public class ParticipationController : Controller
{
    private readonly ISeasonService _seasonService;
    private readonly IFixtureService _fixtureService;
    private readonly IParticipationService _participationService;

    public ParticipationController(ISeasonService seasonService, IFixtureService fixtureService, IParticipationService participationService)
    {
        _seasonService = seasonService;
        _fixtureService = fixtureService;
        _participationService = participationService;
    }

    public async Task<IActionResult> Manage(string id)
    {
        // Validate that the id parameter is a valid GUID format the fixtureId is set to the guid if the parsing is successful
        if (!Guid.TryParse(id, out var fixtureId))
            // If the id is not a valid GUID, return a 400 Bad Request HTTP response
            return BadRequest("Invalid ID format");

        // Get the fixture from the database and include the participants
        var fixture = await _fixtureService.GetFixtureByIdAsync(fixtureId, FixtureIncludes.Participants);

        // Check if the fixture was found
        if (fixture == null)
            return NotFound("Fixture not found");

        // Get the season - required in order to get all the people attached to the season
        var season = await _seasonService.GetSeasonByIdAsync(fixture.SeasonId, SeasonIncludes.PeopleAttachedToSeason);

        // Check if the season was found - seaoson should exist if the fixture record exists
        if (season == null)
            return NotFound("Season not found");

        // Convert the people attached to the season to a selectListItem
        ViewBag.people = season.PeopleAttachedToSeason!
            .OrderBy(p => p.LastName)
            .ThenBy(p => p.FirstName)
            .Select(p => new SelectListItem()
            {
                Text = p.LastName + ", " + p.FirstName,
                Value = p.Id.ToString()
            }).ToList();

        // Check if the fixture has participants
        if (fixture.Participants != null)
        {
            // Map the participants to the participant model used in the view
            ViewBag.listOfParticipants = fixture.Participants.Select(p => new DFCStats.Web.Models.Participants.Participant {
                Id = p.Id,
                FixtureId = p.FixtureId,
                PersonId = p.PersonId,
                RoleInFixture = p.Role,
                FirstName = p.FirstName!,
                LastName = p.LastName!,
                Goals = p.Goals,
                YellowCard = p.YellowCard,
                RedCard = p.RedCard,
                ReplacedByPersonId = p.ReplacedByPersonId,
                ReplacedByFirstName = p.ReplacedByFirstName,
                ReplacedByLastName = p.ReplacedByLastName,
                ReplacedTime = p.ReplacedByTime,
                OrderNumber = p.OrderNo,
                Started = p.Started,
                Substitute = p.Sub
            }).ToList();
        } 
        
        // Set the page heading and the page title
		ViewData["PageHeading"] = fixture.TeamsAndScores;
		ViewData["Title"] = fixture.TeamsAndScores;

        return View(new DFCStats.Web.Models.Participants.AddEditParticipant { FixtureId = fixtureId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddPersonToFixture([FromBody] AddEditParticipant participant)
    {
        try
        {
            // Create a new participant dto
            var participantToAdd = new DFCStats.Domain.DTOs.Participants.ParticipationDTO
            {
                FixtureId = participant.FixtureId,
                PersonId = participant.PersonId,
                Role = participant.RoleInFixture,
                Goals = participant.Goals,
                YellowCard = participant.YellowCard,
                RedCard = participant.RedCard,
                ReplacedByPersonId = participant.ReplacedByPersonId,
                ReplacedByTime = participant.ReplacedTime
            };

            // Try to add the person to the fixture
            await _participationService.AddParticipationAsync(participantToAdd);
        } catch (DFCStatsException ex)
        {
            // There was a problem creating the participation record
            return Json(new { success = false, messageToUser = ex.Message });
        }

        // Participation record created successfully
        return Json(new { success = true, messageToUser = "Person added to fixture" });
    }

    [HttpGet]
    public async Task<IActionResult> EditParticipant(string participantId)
    {
        // Gets the participant from the database
        var participant = await _participationService.GetParticipationByIdAsync(Guid.Parse(participantId));

        // Check if the particpant record was found
        if (participant == null)
            // Return the partial view with null - the view will show an error message
            return PartialView("Partial_EditParticipant", null);

        // Get the fixture from the database and include the participants
        var fixture = await _fixtureService.GetFixtureByIdAsync(participant.FixtureId);

        // Check that the fixture was found - fixture should exist if the participant record exists
        if (fixture == null)
            // Return the partial view with null - the view will show an error message
            return PartialView("Partial_EditParticipant", null);

        // Get the season - required in order to get all the people attached to the season
        var season = await _seasonService.GetSeasonByIdAsync(fixture.SeasonId, SeasonIncludes.PeopleAttachedToSeason);

        // Check that the season was found - season should exist if the fixture record exists
        if (season == null)
            // Return the partial view with null - the view will show an error message
            return PartialView("Partial_EditParticipant", null);

        // Convert the participant to a model for the view
        var participantToEdit = new DFCStats.Web.Models.Participants.AddEditParticipant
        {
            Id = participant.Id,
            FixtureId = participant.FixtureId,
            PersonId = participant.PersonId,
            RoleInFixture = participant.Role,
            Goals = participant.Goals,
            YellowCard = participant.YellowCard,
            RedCard = participant.RedCard,
            ReplacedByPersonId = participant.ReplacedByPersonId,
            ReplacedTime = participant.ReplacedByTime
        };

        // Convert the people attached to the season to a selectListItem
        ViewBag.people = season.PeopleAttachedToSeason!
            .OrderBy(p => p.LastName)
            .ThenBy(p => p.FirstName)
            .Select(p => new SelectListItem()
            {
                Text = p.LastName + ", " + p.FirstName,
                Value = p.Id.ToString()
            }).ToList();

        return PartialView("Partial_EditParticipant", participantToEdit);
    }

    [HttpPost]
	[ValidateAntiForgeryToken]
    public async Task <IActionResult> EditParticipant([FromBody] AddEditParticipant participant)
    {
        try
        {
            // Create a new participant dto
            var participantToAUpdate = new DFCStats.Domain.DTOs.Participants.ParticipationDTO
            {
                Id = participant.Id,
                FixtureId = participant.FixtureId,
                PersonId = participant.PersonId,
                Role = participant.RoleInFixture,
                Goals = participant.Goals,
                YellowCard = participant.YellowCard,
                RedCard = participant.RedCard,
                ReplacedByPersonId = participant.ReplacedByPersonId,
                ReplacedByTime = participant.ReplacedTime
            };

            // Try to add the person to the fixture
            await _participationService.UpdateParticipationAsync(participantToAUpdate);
        } catch (DFCStatsException ex)
        {
            // There was a problem updating the record
            return Json(new { success = false, messageToUser = ex.Message });
        }
        
        return Json(new { success = true, messageToUser = "Person updated" });
    }

    [HttpPost]
	[ValidateAntiForgeryToken]
    public async Task<IActionResult> MovePersonUp([FromBody] AddEditParticipant participant)
    {
        try
        {
            // Create a new participant dto
            var participantToMove = new DFCStats.Domain.DTOs.Participants.ParticipationDTO
            {
                Id = participant.Id
            };
            
            // Move the person up in the order
            await _participationService.Move(participantToMove, "up");
        } catch (DFCStatsException ex)
        {
            //There was a problem moving the record
            return Json(new { success = false, messageToUser = ex.Message });
        }

        return Json(new { success = true, messageToUser = "Order updated" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MovePersonDown([FromBody] AddEditParticipant participant)
    {
        try
        {
            // Create a new participant dto
            var participantToMove = new DFCStats.Domain.DTOs.Participants.ParticipationDTO
            {
                Id = participant.Id
            };
            
            // Move the person up in the order
            await _participationService.Move(participantToMove, "down");
        } catch (DFCStatsException ex)
        {
            //There was a problem moving the record
            return Json(new { success = false, messageToUser = ex.Message });
        }

        return Json(new { success = true, messageToUser = "Order updated" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemovePersonFromFixture([FromBody] AddEditParticipant participant)
    {
        try
        {
            // Create a new participant dto
            var participantToRemove = new DFCStats.Domain.DTOs.Participants.ParticipationDTO
            {
                Id = participant.Id
            };

            //Try to remove the participant from the database
            await _participationService.RemoveParticipationAsync(participantToRemove);
        } catch (Exception ex)
        {
            //There was a problem removing the record
            return Json(new { success = false, messageToUser = ex.Message });
        }

        return Json(new { success = true, messageToUser = "Person removed from fixture" });
    }


    [HttpGet]
    public async Task<IActionResult> RefreshParticipantsPanel(Guid fixtureId)
    {
        // Get the fixture from the database and include the participants
        var fixture = await _fixtureService.GetFixtureByIdAsync(fixtureId, FixtureIncludes.Participants);

        if (fixture != null)
        {
            // Check if the fixture has participants
            if (fixture.Participants != null)
            {
                // Map the participants to the participant model used in the view
                var listOfParticipants = fixture.Participants.Select(p => new DFCStats.Web.Models.Participants.Participant {
                    Id = p.Id,
                    FixtureId = p.FixtureId,
                    PersonId = p.PersonId,
                    RoleInFixture = p.Role,
                    FirstName = p.FirstName!,
                    LastName = p.LastName!,
                    Goals = p.Goals,
                    YellowCard = p.YellowCard,
                    RedCard = p.RedCard,
                    ReplacedByPersonId = p.ReplacedByPersonId,
                    ReplacedByFirstName = p.ReplacedByFirstName,
                    ReplacedByLastName = p.ReplacedByLastName,
                    ReplacedTime = p.ReplacedByTime,
                    OrderNumber = p.OrderNo,
                    Started = p.Started,
                    Substitute = p.Sub
                }).ToList();

                // Returns the participants for the fixture in the partial view
                return PartialView("Partial_ParticipantsForFixture", listOfParticipants);
            } 
        }

        // Either unable to find the fixture or the fixture has no participants - return the partial view
        return PartialView("Partial_ParticipantsForFixture", null);
    }

    [HttpGet]
    public async Task<IActionResult> LoadAddParticipantForm(Guid fixtureId)
    {
        // Get the fixture from the database and include the participants
        var fixture = await _fixtureService.GetFixtureByIdAsync(fixtureId, FixtureIncludes.Participants);

        if (fixture == null)
            // Return the partial view with null - the view will show an error message
             return PartialView("Partial_AddParticipant", null);

        // Get the season - required in order to get all the people attached to the season
        var season = await _seasonService.GetSeasonByIdAsync(fixture.SeasonId, SeasonIncludes.PeopleAttachedToSeason);

        if (season == null)
            // Return the partial view with null - the view will show an error message
             return PartialView("Partial_AddParticipant", null);

        // Convert the people attached to the season to a selectListItem
        ViewBag.people = season.PeopleAttachedToSeason!
            .OrderBy(p => p.LastName)
            .ThenBy(p => p.FirstName)
            .Select(p => new SelectListItem()
            {
                Text = p.LastName + ", " + p.FirstName,
                Value = p.Id.ToString()
            }).ToList();

        // Returns the user's roles in a partial view
        return PartialView("Partial_AddParticipant");
    }

}