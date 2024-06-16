using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LMS.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Sign out user
            await HttpContext.SignOutAsync("MyCookieAuth");

            // Remove the "ClassList" cookie
            Response.Cookies.Delete("ClassList");

            return RedirectToPage("/Account/Login");
        }
    }
}
