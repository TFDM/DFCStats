using DFCStats.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DFCStats.Web.Models.Users;
using DFCStats.Domain.DTOs.Users;
using DFCStats.Domain.DTOs.Roles;
using DFCStats.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DFCStats.Web.Controllers;

public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IRoleService _roleService;

    public UserController(IUserService userService, IRoleService roleService)
    {
        _userService = userService;
        _roleService = roleService;
    }

    public async Task<IActionResult> New()
    {
        // Set the page heading and the page title
		ViewData["PageHeading"] = "Create User";
		ViewData["Title"] = "Create User";

        // Get the roles from the database
        var roles = await _roleService.GetAllRolesAsync();

        // Sets up the true/false options for the dropdowns
        var trueFalseOptions = new List<SelectListItem>
        {
            new SelectListItem { Text = "Yes", Value = "true" },
            new SelectListItem { Text = "No", Value = "false" }
        };

        // Create a new user model and use the roles to create a list of checkboxes
        var viewModel = new NewUser()
        {
            Roles = roles.Select(dto => new RoleCheckBox
            {
                RoleId = dto.Id,
                RoleName = dto.Name,
                IsSelected = false
            }).ToList(),
            AllowLoginOptions = trueFalseOptions
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> New(NewUser newUser)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // Create the userDTO
                var userDTO = new UserDTO
                {
                    EmailAddress = newUser.EmailAddress,
                    AllowLogin = newUser.AllowLogin ?? false,
                    Password = newUser.Password,
                    ConfirmPassword = newUser.ConfirmPassword,
                    Roles = newUser.Roles.Where(r => r.IsSelected).Select(r => new RoleDTO
                    {
                        Id = r.RoleId,
                        Name = r.RoleName
                    }).ToList()
                };

                // Attempt to create the user
                await _userService.RegisterUserAsync(userDTO);

                // Redirect to the index action
                return RedirectToAction("Index");
            } catch (DFCStatsException ex)
            {
                // Add a failure message to TempData
                TempData["Failure"] = ex.Message;
            }
        }

        // Set the page heading and the page title
		ViewData["PageHeading"] = "Create User";
		ViewData["Title"] = "Create User";

        // Sets up the true/false options for the dropdowns
        var trueFalseOptions = new List<SelectListItem>
        {
            new SelectListItem { Text = "Yes", Value = "true" },
            new SelectListItem { Text = "No", Value = "false" }
        };

        // Set the allow login options on the model
        newUser.AllowLoginOptions = trueFalseOptions;

        return View(newUser);
    }

    public async Task<IActionResult> Edit(string id)
    {
        // Set the page heading and the page title
		ViewData["PageHeading"] = "Edit User";
		ViewData["Title"] = "Edit User";

        // Validate that the id parameter is a valid GUID format
        // the userId is set to the guid if the parsing is successful
        if (!Guid.TryParse(id, out var userId))
            // If the id is not a valid GUID, return a 400 Bad Request HTTP response
            return BadRequest("Invalid ID format");

        // Get the user from the database using the validated GUID and include their roles
        var user = await _userService.GetUserById(userId, UserIncludes.Roles);

        // If the user record is not found, return a 404 Not Found HTTP response
        if (user == null)
            return NotFound("User not found");

        // Get the roles from the database
        var roles = await _roleService.GetAllRolesAsync();

        // Sets up the true/false options for the dropdowns
        var trueFalseOptions = new List<SelectListItem>
        {
            new SelectListItem { Text = "Yes", Value = "true" },
            new SelectListItem { Text = "No", Value = "false" }
        };

        // Conver the userDTO to an edit user model
        var userToEdit = new EditUser
        {
            Id = user.Id,
            EmailAddress = user.EmailAddress,
            AllowLogin = user.AllowLogin,
            Roles = roles.Select(dto => new RoleCheckBox
            {
                RoleId = dto.Id,
                RoleName = dto.Name,
                IsSelected = user.Roles != null && user.Roles.Any(r => r.Id == dto.Id)
            }).ToList(),
            AllowLoginOptions = trueFalseOptions
        };

        return View(userToEdit);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditUser editUser)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // Create the userDTO
                var userDTO = new UserDTO
                {
                    Id = editUser.Id,
                    EmailAddress = editUser.EmailAddress,
                    AllowLogin = editUser.AllowLogin ?? false,
                    Roles = editUser.Roles.Where(r => r.IsSelected).Select(r => new RoleDTO
                    {
                        Id = r.RoleId,
                        Name = r.RoleName
                    }).ToList()
                };

                // Attempt to update the user
                await _userService.UpdateUserAsync(userDTO);

                // Redirect to the index action
                return RedirectToAction("Index");
            } catch (DFCStatsException ex)
            {
                // Add a failure message to TempData
                TempData["Failure"] = ex.Message;
            }
        }

        // Set the page heading and the page title
		ViewData["PageHeading"] = "Create User";
		ViewData["Title"] = "Create User";

        // Sets up the true/false options for the dropdowns
        var trueFalseOptions = new List<SelectListItem>
        {
            new SelectListItem { Text = "Yes", Value = "true" },
            new SelectListItem { Text = "No", Value = "false" }
        };

        // Set the allow login options on the model
        editUser.AllowLoginOptions = trueFalseOptions;

        return View(editUser);
    }
}