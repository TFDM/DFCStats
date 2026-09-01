using Microsoft.AspNetCore.Mvc;
using DFCStats.Domain.Exceptions;
using DFCStats.Business.Interfaces;
using DFCStats.Web.Models.PasswordResets;
using DFCStats.Domain.DTOs.PasswordResets;

namespace DFCStats.Web.Controllers;

public class PasswordResetController : Controller
{
    private readonly IPasswordResetService _passwordResetService;
    private readonly ILogger<PasswordResetController> _logger;

    public PasswordResetController(IPasswordResetService passwordResetService, ILogger<PasswordResetController> logger)
    {
        _passwordResetService = passwordResetService;
        _logger = logger;
    }

    public async Task<IActionResult> ForgotPassword()
    {
        // Set the page heading and the page title
		ViewData["PageHeading"] = "Forgot Password";
		ViewData["Title"] = "Forgot Password";

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPassword forgotPassword)
    {
        if (ModelState.IsValid)
        {
            // Get the remote IP address of the client making the request
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
            string clientIp = "Unknown";

            // Check if the remote IP address is not null
            if (remoteIpAddress != null)
            {
                // Check if the remote IP address is a loopback address (e.g., localhost)
                // If it is a loopback address set the client IP to 127.0.0.1
                clientIp = System.Net.IPAddress.IsLoopback(remoteIpAddress) ? "127.0.0.1" : remoteIpAddress.ToString();
            }

            // Create the password reset request DTO
            var passwordResetRequestDTO = new PasswordResetRequestDTO {
                EmailAddress = forgotPassword.EmailAddress,
                IpAddress = clientIp
            };

            try
            {
                // Process the password reset request and get the result indicating whether the email was sent successfully or not
                var emailSent =await _passwordResetService.RequestPasswordResetAsync(passwordResetRequestDTO);
            } catch (DFCStatsException ex)
            {
                // Something went wrong with the password reset request, log it but don't reveal 
                // the details to the user to avoid giving away information about the system
                _logger.LogError(ex.Message);
            }
            
            // Regardless of wheather the email was sent successfully or not we will return the same message
            // to the user to avoid revealing whether the email address exists in the system or not
            TempData["Success"] = "If the email address exists in the system, a password reset link will be sent to it.";

        }

        // Set the page heading and the page title
		ViewData["PageHeading"] = "Forgot Password";
		ViewData["Title"] = "Forgot Password";

        return View(forgotPassword);
    }

    public async Task<IActionResult> ResetPassword(string token)
    {
        // Get the status of the reset token to determine if it is valid
        var tokenStatus = await _passwordResetService.ValidateResetTokenAsync(token);

        

        return View();
    }
}