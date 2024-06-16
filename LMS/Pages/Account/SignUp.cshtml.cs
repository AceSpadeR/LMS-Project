using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using LMS.Data;
using LMS.Models;
using System.Security.Cryptography;
using System.Text;
//using LMS.Migrations;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace LMS.Pages.Account
{
    public class SignUpModel : PageModel
    {
        private readonly LMS.Data.LMSContext _context;

        public SignUpModel(LMS.Data.LMSContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public User User { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            bool isDuplicateEmail = _context.User.Any(u => u.Email == User.Email);

            if (!ModelState.IsValid || isDuplicateEmail || !User.Password.Equals(User.ConfirmPassword) || _context.User == null || User == null)
            {
                if (isDuplicateEmail)
                {
                    ModelState.AddModelError("User.Email", "Email already exists in our system.");
                }

                if (!User.Password.Equals(User.ConfirmPassword))
                {
                    ModelState.AddModelError("User.ConfirmPassword", "Password and Confirmation Password do not match.");
                }

                return Page();
            }

            User.Password = HashPassword(User.Password, out var salt);
            User.ConfirmPassword = User.Password;

            //code from method
            await CreateUserAccountLogic(User);

            //Creating the new security context
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, User.Email),
                new Claim(ClaimTypes.Role, User.Role),
                new Claim("Id", User.Id.ToString(), ClaimValueTypes.Integer)};

            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);

            return RedirectToPage("/Index", User);
        }

        private string HashPassword(string password, out byte[] salt) // function influenced by "https://code-maze.com/csharp-hashing-salting-passwords-best-practices/"
        {
            salt = new byte[64] { 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 1, 0, 1, 0, 1, 1, 1, 0, 1, 0, 0, 1, 1, 1, 0, 1, 1, 0, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1 }; //temp fixed salt //TODO: remove //RandomNumberGenerator.GetBytes(64); //64 = keysize
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                350000, //iterations
                HashAlgorithmName.SHA512,
                64);//key size
            return Convert.ToHexString(hash);
        }

        public async Task<int> CreateUserAccountLogic(User x)
        {

            _context.User.Add(x);
            await _context.SaveChangesAsync();

            return 0;//done

        }
    }
}
