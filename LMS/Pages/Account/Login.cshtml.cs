using LMS.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace LMS.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly LMS.Data.LMSContext _context;

        public LoginModel(LMS.Data.LMSContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Sign out the user
                await HttpContext.SignOutAsync("MyCookieAuth");
                return RedirectToPage("./Login");
            }

            return Page();
        }

        [BindProperty]
        public string Email { get; set; } = default!;

        [BindProperty]
        public string Password { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            User user = _context.User.FirstOrDefault(u => u.Email == Email);

            if (user == null)
            {
                ModelState.AddModelError("Email", "Email doesn't exist in our system.");
                return Page();
            }
            else if (!user.CheckPassword(Password, user.Password, [1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 1, 0, 1, 0, 1, 1, 1, 0, 1, 0, 0, 1, 1, 1, 0, 1, 1, 0, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1]))//user.Password != Password)
            {
                ModelState.AddModelError("Password", "Incorrect password.");
                return Page();
            }

            //Creating the new security context
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("Id", user.Id.ToString(), ClaimValueTypes.Integer)};

            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);

            return RedirectToPage("/Index", user);
        }
    }
}


