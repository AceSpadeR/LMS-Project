using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LMS.Data;
using LMS.Models;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace LMS.Pages.Classes
{
    [Authorize(Roles = "Instructor")]
    public class DeleteModel : PageModel
    {
        private readonly LMS.Data.LMSContext _context;

        public DeleteModel(LMS.Data.LMSContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Class Classes { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classes = await _context.Class.FirstOrDefaultAsync(m => m.Id == id);

            if (classes == null)
            {
                return NotFound();
            }
            else
            {
                Classes = classes;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Add current user's id to the new Classes object to add to database
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
            var instructorID = int.Parse(userIdClaim);

            var classes = await _context.Class.FindAsync(id);
            if (classes != null)
            {

                var eventToDelete = await _context.Event.FirstOrDefaultAsync(e => e.ClassId == classes.Id);


                if (eventToDelete != null)
                {
                    _context.Event.Remove(eventToDelete);
                }

                _context.Class.Remove(classes);
                await _context.SaveChangesAsync();

                var classList = _context.Class.Where(c => c.ProfId == instructorID).ToList();

                // Serialize the class list and store it in the user's cookie
                Response.Cookies.Append("ClassList", JsonConvert.SerializeObject(classList));
            }

            return RedirectToPage("./Index");
        }

    }
}
