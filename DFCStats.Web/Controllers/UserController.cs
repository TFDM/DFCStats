using DFCStats.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DFCStats.Web.Models.Users;
using DFCStats.Domain.DTOs.Users;
using DFCStats.Domain.DTOs.Roles;
using DFCStats.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

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

    [Authorize(Roles = "Test Role")]
    public async Task<string> Index()
    {
        string? idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        string? email = User.FindFirstValue(ClaimTypes.Email);

        return $"User ID: {idClaim} and email: {email}";
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

    public async Task<IActionResult> Login()
    {
        // Set the page heading and the page title
		ViewData["PageHeading"] = "Login";
		ViewData["Title"] = "Login";

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(Login userLogin, string returnURL = "/User/")
    {
        if (ModelState.IsValid)
        {
            // Convert the login model to a login DTO
            var loginDTO = new LoginDTO
            {
                EmailAddress = userLogin.EmailAddress,
                Password = userLogin.Password
            };

            // Get the login in result
            var userLoginResult = await _userService.LoginAsync(loginDTO);

            // Check if the login was succesful or not
            if (userLoginResult.Succeeded)
            {
                // Sets the claims for the user
                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userLoginResult.User!.Id.ToString()));
                claims.Add(new Claim(ClaimTypes.Email, userLoginResult.User!.EmailAddress));

                // Checks if the user has any roles
                if (userLoginResult.User!.Roles != null && userLoginResult.User.Roles.Count > 0)
                {
                    // Loop over each of the roles
                    foreach (var role in userLoginResult.User.Roles)
                    {
                        // Adds the role to the claims
                        claims.Add(new Claim(ClaimTypes.Role, role.Name));
                    }
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    //AllowRefresh = <bool>,
                    // Refreshing the authentication session should be allowed.

                    //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.

                    //IsPersistent = true,
                    // Whether the authentication session is persisted across 
                    // multiple requests. When used with cookies, controls
                    // whether the cookie's lifetime is absolute (matching the
                    // lifetime of the authentication ticket) or session-based.

                    //IssuedUtc = <DateTimeOffset>,
                    // The time at which the authentication ticket was issued.

                    //RedirectUri = <string>
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return Redirect(returnURL);      
            }

            // If we get this far we can assume the user has not been sucesfully logged in

            // Set some appropriate messages for the failure temp data
            TempData["Failure"] = "Unable to login - invalid email address or password";        
        }

        // Set the page heading and the page title
		ViewData["PageHeading"] = "Login";
		ViewData["Title"] = "Login";

        return View(userLogin);
    }

    public async Task<IActionResult> Logout()
    {
        //Clear the existing external cookie to log the user out
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        //Sets a temp success message
        TempData["Success"] = "You have logged out";

        //Redirect back to the login page
        return RedirectToAction("Login");
    }
}