// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Online_Art_Gallery.Models.Data;

namespace Online_Art_Gallery.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(SignInManager<AppUser> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        // This handles the logout POST request
        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");

            return RedirectToPage("/Index", new { area = "" }); // redirect to home
        }

        // This handles GET request to show the logout confirmation message
        public void OnGet()
        {
            // No processing needed, just show the page
        }
    }
}
