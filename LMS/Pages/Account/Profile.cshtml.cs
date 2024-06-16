using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LMS.Data;
using LMS.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace LMS.Pages.Account
{
    [Authorize]

    public class ProfileModel : PageModel
    {
        private LMSContext _context;

		public ProfileModel(LMSContext context)
		{
			_context = context;
		}

		[BindProperty]
		public User LoginUser { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync()
		{

            //Get current user's id to get the classes associated with that user
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
            int userId = int.Parse(userIdClaim);

            var user = await _context.User.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
			{
				return NotFound();
			}
			else
			{
				LoginUser = user;
			}

			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
			int userId = int.Parse(userIdClaim);

			var user = await _context.User.FirstOrDefaultAsync(u => u.Id == userId);
			string currentUserEmail = user.Email;

			bool isDuplicateEmail = _context.User.Any(u => u.Email == LoginUser.Email && u.Email != currentUserEmail);

            if (!ModelState.IsValid || isDuplicateEmail)
            {
                if (isDuplicateEmail)
                {
                    ModelState.AddModelError("LoginUser.Email", "Email already exists in our system.");
                }

                return Page();
            }

			user.FirstName = LoginUser.FirstName;
			user.LastName = LoginUser.LastName;
			user.BirthDate = LoginUser.BirthDate;
			user.Email = LoginUser.Email;
			user.Address1 = LoginUser.Address1;
			user.Address2 = LoginUser.Address2;
			user.City = LoginUser.City;
			user.State = LoginUser.State;
			user.ZipCode = LoginUser.ZipCode;
			user.Link1 = LoginUser.Link1;
			user.Link2 = LoginUser.Link2;
			user.Link3 = LoginUser.Link3;
			user.Phone = LoginUser.Phone;

			try
			{
				_context.Update(user);
                await _context.SaveChangesAsync();
            }
			catch (DbUpdateConcurrencyException)
			{
				if (!_context.User.Any(e => e.Id == user.Id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}
			return RedirectToPage("./Profile");

		}

        public async Task<IActionResult> OnPostDelete()
        {

            //Get current user's id to get the classes associated with that user
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
            int userId = int.Parse(userIdClaim);

            var user = await _context.User.FirstOrDefaultAsync(u => u.Id == userId);

            if (user != null)
            {
                _context.User.Remove(user);
                await _context.SaveChangesAsync();

                // Sign out the user
                HttpContext.SignOutAsync("MyCookieAuth");
                return RedirectToPage("./Login");
            }

            return Page();
        }
    }
}
