using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Online_Art_Gallery.Models.Data;

namespace Online_Art_Gallery.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;

        public ConfirmEmailModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            // Decode the email confirmation token
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            var result = await _userManager.ConfirmEmailAsync(user, code);

            StatusMessage = result.Succeeded
                ? "✅ Your email has been successfully confirmed. You can now log in."
                : "❌ Error confirming your email. The link may have expired or is invalid.";

            return Page();
        }
    }
}
