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
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication;

namespace LMS.Pages
{
    [Authorize]
    public class CalendarModel : PageModel
    {
        private readonly LMS.Data.LMSContext _context;
        public IEnumerable<Class> ClassList;
        public CalendarModel(LMS.Data.LMSContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Sign out the user
                await HttpContext.SignOutAsync("MyCookieAuth");
                return RedirectToPage("./Account/Login");
            }


            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
            int userId = int.Parse(userIdClaim);


            return Page();
        }

        [BindProperty]
        public Event Event { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Event.Add(Event);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnGetEventsAsync()
        {
            if (User.IsInRole("Instructor"))
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
                int userId = int.Parse(userIdClaim);


                var userEvents = await _context.Event
                    .Include(e => e.Class) // Include the Class entity
                    .Where(e => e.UserId == userId && e.AssignmentId == -1)
                    .ToListAsync();
                var fullCalendarEvents = userEvents.Select(e => new
                {
                    title = e.EventType,
                    start = e.Class.StartDate.ToString("s"), // Get from Class
                    end = e.Class.EndDate.ToString("s"), // Get from Class
                    daysOfWeek = e.Class.MeetingDays.Select(d => (int)d).ToArray(),  // Get from Class
                    startTime = e.Class.StartTime.TimeOfDay.ToString(@"hh\:mm"), // Get from Class
                    endTime = e.Class.EndTime.TimeOfDay.ToString(@"hh\:mm") // Get from Class
                }).ToList();
                return new JsonResult(fullCalendarEvents);

            }
            else
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
                int userId = int.Parse(userIdClaim);

                var ClassEvents = await _context.Event
                    .Include(e => e.Class) // Include the Class entity
                    .Where(e => e.UserId == userId && e.AssignmentId == -1)
                    .ToListAsync();
                var fullCalendarClassEvents = ClassEvents.Select(e => new
                {
                    title = e.EventType,
                    start = e.Class.StartDate.ToString("s"), // Get from Class
                    end = e.Class.EndDate.ToString("s"), // Get from Class
                    daysOfWeek = e.Class.MeetingDays.Select(d => (int)d).ToArray(),  // Get from Class
                    startTime = e.Class.StartTime.TimeOfDay.ToString(@"hh\:mm"), // Get from Class
                    endTime = e.Class.EndTime.TimeOfDay.ToString(@"hh\:mm") // Get from Class
                }).ToList();
                var Assignmentsclass = await _context.Assignment
                    .Include(a => a.Classes)
                    .ToListAsync();

                var AssignmentsEvents = await _context.Event
                    .Include(e => e.Class)
                    .Where(e => e.AssignmentId > -1)
                    .ToListAsync();

                var fullCalendarAssignmentEvents = from e in AssignmentsEvents
                                                   join a in Assignmentsclass on e.AssignmentId equals a.Id
                                                   select new
                                                   {
                                                       title = e.EventType,
                                                       start = a.DueDate.AddHours(a.DueDate.TimeOfDay.TotalHours).ToString("s"),
                                                       end = a.DueDate.AddHours(a.DueDate.TimeOfDay.TotalHours).ToString("s"),
                                                       allDay = false,
                                                       url = $"/Classes/Assignments/Submission?id={a.Id}"
                                                   };

                fullCalendarAssignmentEvents = fullCalendarAssignmentEvents.ToList();




                var fullCalendarEvents = fullCalendarClassEvents.Cast<object>().Concat(fullCalendarAssignmentEvents.Cast<object>()).ToList();
                return new JsonResult(fullCalendarEvents);


            }




        }
    }
}
