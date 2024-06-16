using LMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace LMS.Pages.Account
{
    [Authorize]
    public class ProfilePicModel : PageModel
    {
        private readonly Data.LMSContext _context;

        public ProfilePicModel(Data.LMSContext context)
        {
            _context = context;
        }

        public byte[] ImageData { get; set; }

        [BindProperty]
        public User User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                User = user;
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProfilePic", id.ToString());
                if (System.IO.File.Exists(imagePath))
                {
                    ImageData = System.IO.File.ReadAllBytes(imagePath);
                }
            }
            return Page();
        }

        public IActionResult OnPost(int id)
        {
            var photo = Request.Form.Files["Image"];
            var newFileName = id.ToString();

            if (photo != null)
            {
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProfilePic", newFileName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    photo.CopyTo(stream);
                }
            }

            return RedirectToPage(new { id });
        }

    }
}
