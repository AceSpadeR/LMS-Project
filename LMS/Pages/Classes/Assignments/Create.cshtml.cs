using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using LMS.Data;
using LMS.Models;
using Microsoft.AspNetCore.Authorization;

namespace LMS.Pages.Classes.Assignments
{
    [Authorize(Roles = "Instructor")]
    public class CreateModel : PageModel
    {
        private readonly LMS.Data.LMSContext _context;

        private int classId;

        public CreateModel(LMS.Data.LMSContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int? id)
        {
            TempData["ClassId"] = id;
            return Page();
        }

        [BindProperty]
        public Assignment Assignment { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            //Assignment.ClassId = (int)TempData["ClassId"];

            //code from method
            await createAssignmentLogic(Assignment);

            var Classes = await _context.Class.FindAsync(Assignment.ClassId);

            if (Classes != null)
            {
                var instructorId = Classes.ProfId; // Accessing the InstructorId

                var newEvent = new Event
                {
                    EventType = $"{Classes.CourseNumber}: {Assignment.Title}",
                    EventDescription = $"{Assignment.Description}, {Assignment.MaxPoints}",
                    AssignmentId = Assignment.Id,
                    UserId = instructorId,
                    ClassId = Classes.Id
                };

                // Add the new event to your context and save changes
                _context.Event.Add(newEvent);
                await _context.SaveChangesAsync();

                // Create Notifications for everyone registered for the class
                foreach (var registration in _context.Registration.Where(c => c.ClassId == Classes.Id).ToList())
                {
                    var newNotification = new Notification
                    {
                        Title = "Assignment Created",
                        Message = $"{Assignment.Title}, {Classes.CourseNumber}",
                        NotifyDate = DateTime.Now,
                        AssignmentId = Assignment.Id,
                        UserId = registration.UserId
                    };

                    _context.Notification.Add(newNotification);
                    await _context.SaveChangesAsync();
                }

            }

            return RedirectToPage("/Classes/Details", new { id = Assignment.ClassId });

        }

        public async Task<int> createAssignmentLogic(Assignment x)
        {
            _context.Assignment.Add(x);
            await _context.SaveChangesAsync();

            return 0;
        }
    }
}
