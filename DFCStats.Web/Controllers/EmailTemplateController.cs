using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DFCStats.Business.Interfaces;
using DFCStats.Web.Models.EmailTemplates;
using DFCStats.Domain.Exceptions;
using DFCStats.Domain.DTOs.EmailTemplates;

namespace DFCStats.Web.Controllers;

public class EmailTemplateController : Controller
{
    private readonly IEmailTemplateService _emailTemplateService;

    public EmailTemplateController(IEmailTemplateService emailTemplateService)
    {
        _emailTemplateService = emailTemplateService;
    }

    public async Task<IActionResult> Index()
    {
        // Set the page heading and the page title
		ViewData["PageHeading"] = "Email Templates";
		ViewData["Title"] = "Email Templates";

        // Get all the email templates from the database
        var emailTemplates = await _emailTemplateService.GetAllEmailTemplatesAsync();
    
        // Convert the list of EmailTemplateDTO into a list of EmailTemplate models
        var listOfTemplates = emailTemplates.Select(dto => new EmailTemplate
        {
            Id = dto.Id,
            Title = dto.Title,
            Template = dto.Template,
            IsHtml = dto.IsHtml
        }).ToList();

        return View(listOfTemplates);
    }

    public async Task<IActionResult> New()
    {
        // Set the page heading and the page title
		ViewData["PageHeading"] = "Create Email Template";
		ViewData["Title"] = "Create Email Template";

        // Sets up the true/false options for the dropdowns
        var trueFalseOptions = new List<SelectListItem>
        {
            new SelectListItem { Text = "Yes", Value = "true" },
            new SelectListItem { Text = "No", Value = "false" }
        };

        // Create a new email tempalte model
        var viewModel = new NewEmailTemplate()
        {
            IsHtmlOptions = trueFalseOptions
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> New(NewEmailTemplate newEmailTemplate)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // Create an email template DTO
                var emailTemplateDTO = new EmailTemplateDTO
                {
                    Title = newEmailTemplate.Title,
                    Template = newEmailTemplate.Template,
                    IsHtml = newEmailTemplate.IsHtml ?? false
                };

                // Attempt to create the email template
                await _emailTemplateService.AddTemplateAsync(emailTemplateDTO);

                // Add a success message to TempData
                TempData["Success"] = $"{newEmailTemplate.Title} has been added successfully";

                // Redirect to the index action
                return RedirectToAction("Index");
            } catch (DFCStatsException ex)
            {
                // Add a failure message to TempData
                TempData["Failure"] = ex.Message;
            }
        }

        // Sets up the true/false options for the dropdowns
        var trueFalseOptions = new List<SelectListItem>
        {
            new SelectListItem { Text = "Yes", Value = "true" },
            new SelectListItem { Text = "No", Value = "false" }
        };

        // Set the page heading and the page title
		ViewData["PageHeading"] = "Create Email Template";
		ViewData["Title"] = "Create Email Template";

        // Set the IsHtml options on the model
        newEmailTemplate.IsHtmlOptions = trueFalseOptions;

        return View(newEmailTemplate);
    }

    public async Task<IActionResult> Edit(string id)
    {
        // Set the page heading and the page title
		ViewData["PageHeading"] = "Edit Template";
		ViewData["Title"] = "Edit Template";

        // Validate that the id parameter is a valid GUID format
        // the emailTemplateId is set to the guid if the parsing is successful
        if (!Guid.TryParse(id, out var emailTemplateId))
            // If the id is not a valid GUID, return a 400 Bad Request HTTP response
            return BadRequest("Invalid ID format");

        var emailTemplate = await _emailTemplateService.GetTemplateByIdAsync(emailTemplateId);

        // If the email template record is not found, return a 404 Not Found HTTP response
        if (emailTemplate == null)
            return NotFound("Email template not found");

        // Sets up the true/false options for the dropdowns
        var trueFalseOptions = new List<SelectListItem>
        {
            new SelectListItem { Text = "Yes", Value = "true" },
            new SelectListItem { Text = "No", Value = "false" }
        };

        // Conver the emailTemplateDTO to an edit template model
        var emailTemplateToEdit = new EditEmailTemplate
        {
            Id = emailTemplate.Id,
            Title = emailTemplate.Title,
            Template = emailTemplate.Template,
            IsHtml = emailTemplate.IsHtml,
            IsHtmlOptions = trueFalseOptions
        };

        return View(emailTemplateToEdit);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditEmailTemplate templateToEdit)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // Create an email template DTO
                var emailTemplateDTO = new EmailTemplateDTO
                {
                    Id = templateToEdit.Id,
                    Title = templateToEdit.Title,
                    Template = templateToEdit.Template,
                    IsHtml = templateToEdit.IsHtml ?? false
                };

                // Attempt to update the email template
                await _emailTemplateService.UpdateTemplateAsync(emailTemplateDTO);

                // Add a success message to TempData
                TempData["Success"] = $"{templateToEdit.Title} has been added successfully";

                // Redirect to the index action
                return RedirectToAction("Index");
            } catch (DFCStatsException ex)
            {
                // Add a failure message to TempData
                TempData["Failure"] = ex.Message;
            }
        }

        // Set the page heading and the page title
		ViewData["PageHeading"] = "Edit Template";
		ViewData["Title"] = "Edit Template";

        // Sets up the true/false options for the dropdowns
        var trueFalseOptions = new List<SelectListItem>
        {
            new SelectListItem { Text = "Yes", Value = "true" },
            new SelectListItem { Text = "No", Value = "false" }
        };

        // Set the IsHtml options on the model
        templateToEdit.IsHtmlOptions = trueFalseOptions;

        return View(templateToEdit);
    }
}