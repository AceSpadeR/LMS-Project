using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LMS.Data;
using LMS.Models;
using Microsoft.AspNetCore.Authorization;

namespace LMS.Pages.Classes
{
    [Authorize(Roles = "Instructor")]
    public class EditModel : PageModel
    {
        private readonly LMS.Data.LMSContext _context;

        public EditModel(LMS.Data.LMSContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Class Classes { get; set; } = default!;

        public List<SelectListItem> DepartmentList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            DepartmentList = new List<SelectListItem>
                {
                    new SelectListItem { Value = "ComputerScience", Text = "Computer Science" },
                    new SelectListItem { Value = "Math", Text = "Math" },
                    new SelectListItem { Value = "Physics", Text = "Physics" },
                    new SelectListItem { Value = "Chemistry", Text = "Chemistry" }
                };

            if (id == null)
            {
                return NotFound();
            }

            var classes =  await _context.Class.FirstOrDefaultAsync(m => m.Id == id);
            if (classes == null)
            {
                return NotFound();
            }
            Classes = classes;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //Add current user's id to the new Classes object to keep in database
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
            Classes.ProfId = int.Parse(userIdClaim);

            _context.Attach(Classes).State = EntityState.Modified;

            // Find the associated event
            var eventToUpdate = await _context.Event.FirstOrDefaultAsync(e => e.ClassId == Classes.Id && e.AssignmentId == -1);

            // If the event exists, update it
            if (eventToUpdate != null)
            {
                eventToUpdate.EventType = $"{Classes.CourseNumber} {Classes.Title}";
                eventToUpdate.EventDescription = $"{Classes.Location} {Classes.DepartmentName}";
                _context.Attach(eventToUpdate).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassesExists(Classes.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }


        private bool ClassesExists(int id)
        {
            return _context.Class.Any(e => e.Id == id);
        }
    }
}
