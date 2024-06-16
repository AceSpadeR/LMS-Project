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

namespace LMS.Pages.Classes.Assignments
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
        public Assignment Assignment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id, int? classId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignment = await _context.Assignment.FirstOrDefaultAsync(m => m.Id == id);

            if (assignment == null)
            {
                return NotFound();
            }
            else
            {
                Assignment = assignment;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, int classId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignment = await _context.Assignment.FindAsync(id);
            if (assignment != null)
            {
                Assignment = assignment;

                var eventToDelete = await _context.Event.FirstOrDefaultAsync(e => e.ClassId == classId && e.AssignmentId == Assignment.Id && e.Id == id);


                if (eventToDelete != null)
                {
                    _context.Event.Remove(eventToDelete);
                }
                _context.Assignment.Remove(Assignment);
                await _context.SaveChangesAsync();

                // Delete Notifications for everyone who had that assignment
                foreach (var notification in _context.Notification.Where(c => c.AssignmentId == assignment.Id).ToList())
                {
                    _context.Notification.Remove(notification);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToPage("/Classes/Details", new { id = classId} );
        }
    }
}
