using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using LMS.Data;
using LMS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace LMS.Pages.Registration
{
    [Authorize(Roles = "Student")]
    public class CreateModel : PageModel
    {
        private readonly LMS.Data.LMSContext _context;

        private int _studentID;

        public string Department { get; set; } = default!;

        public CreateModel(LMS.Data.LMSContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            DepartmentList = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Computer Science", Text = "Computer Science" },
                    new SelectListItem { Value = "Math", Text = "Math" },
                    new SelectListItem { Value = "Physics", Text = "Physics" },
                    new SelectListItem { Value = "Chemistry", Text = "Chemistry" }
                };

            // Get user ID
            if (!User.Identity.IsAuthenticated)
            {
                // Sign out the user
                HttpContext.SignOutAsync("MyCookieAuth");
                return RedirectToPage("../Account/Login");
            }

            //Get current user's id to get the classes associated with that user
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
            _studentID = int.Parse(userIdClaim);

            Classes = _context.Class.ToList();

            ViewData["ClassId"] = new SelectList(_context.Class, "Id", "CourseNumber");
            //ViewData["UserId"] = new SelectList(_context.User, "Id", "ConfirmPassword");



            var registrations = _context.Registration.Where(r => r.UserId == _studentID).ToList();
            if (registrations is not null)
            {
                List<int> classIDs = new List<int>();
                foreach (var r in registrations)
                {
                    classIDs.Add(r.ClassId);
                }
                RegisteredClassIDs = classIDs;
            }

            return Page();
        }

        public List<SelectListItem> DepartmentList { get; set; } = default;
        public IList<Models.Class> Classes { get; set; } = default!;

        public List<int> RegisteredClassIDs { get; set; } = default!;



        [BindProperty]
        public Models.Registration Registration { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD

        public void OnPostDepartment()
        {
            // Populate the Departments list here
            Department = Request.Form["dept"];

            DepartmentList = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Computer Science", Text = "Computer Science" },
                    new SelectListItem { Value = "Math", Text = "Math" },
                    new SelectListItem { Value = "Physics", Text = "Physics" },
                    new SelectListItem { Value = "Chemistry", Text = "Chemistry" }
                };

            // Get user ID
            if (!User.Identity.IsAuthenticated)
            {
                // Sign out the user
                HttpContext.SignOutAsync("MyCookieAuth");
            }

            //Get current user's id to get the classes associated with that user
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
            _studentID = int.Parse(userIdClaim);

            Classes = _context.Class.ToList();

            // populate registered class list
            var registrations = _context.Registration.Where(r => r.UserId == _studentID).ToList();
            if (registrations is not null)
            {
                List<int> classIDs = new List<int>();
                foreach (var r in registrations)
                {
                    classIDs.Add(r.ClassId);
                }
                RegisteredClassIDs = classIDs;
            }
        }

        public async Task<IActionResult> OnPostAdd()
        {
            //if (!ModelState.IsValid)
            //{
            //    Classes = _context.Class.ToList();
            //    return Page();
            //}

            // Get user ID
            if (!User.Identity.IsAuthenticated)
            {
                // Sign out the user
                HttpContext.SignOutAsync("MyCookieAuth");
                return RedirectToPage("../Account/Login");
            }

            //Get current user's id to get the classes associated with that user
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
            _studentID = int.Parse(userIdClaim);

            Registration.UserId = _studentID;
            Registration.ClassId = Int32.Parse(Request.Form["ClassId"]);

            await CreateRegistrationLogic(Registration);

            // populate registered class list
            var registrations = _context.Registration.Where(r => r.UserId == _studentID).ToList();
            if (registrations is not null)
            {
                List<int> classIDs = new List<int>();
                foreach (var r in registrations)
                {
                    classIDs.Add(r.ClassId);
                }
                RegisteredClassIDs = classIDs;

                var classList = await _context.Class.Where(c => classIDs.Contains(c.Id)).ToListAsync();

                // Serialize the class list and store it in the user's cookie
                Response.Cookies.Append("ClassList", JsonConvert.SerializeObject(classList));
            }
            var Classes = await _context.Class.FindAsync(Registration.ClassId);

            if (Classes != null)
            {
                var newEvent = new Event
                {
                    EventType = $"{Classes.CourseNumber}: {Classes.Title}",
                    EventDescription = $"{Classes.DepartmentName}, {Classes.Location}",
                    UserId = Registration.UserId,
                    ClassId = Classes.Id
                };

                // Add the new event to your context and save changes
                _context.Event.Add(newEvent);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("./Index");
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Registration.Add(Registration);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostDrop()
        {
            // Get user ID
            if (!User.Identity.IsAuthenticated)
            {
                // Sign out the user
                HttpContext.SignOutAsync("MyCookieAuth");
                return RedirectToPage("./Account/Login");
            }

            //Get current user's id to get the classes associated with that user
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
            _studentID = int.Parse(userIdClaim);

            //get class id
            int dropClassID = Int32.Parse(Request.Form["ClassId"]);

            //drop registration
            var dropList = _context.Registration.Where(r => r.UserId == _studentID && r.ClassId == dropClassID).ToList();

            foreach (var d in dropList)
            {
                if (d != null)
                {
                    var eventToDelete = await _context.Event.FirstOrDefaultAsync(e => e.ClassId == dropClassID && e.UserId == _studentID && e.AssignmentId == -1);
                    if (eventToDelete != null)
                    {
                        _context.Event.Remove(eventToDelete);
                    }

                    _context.Registration.Remove(d);
                    await _context.SaveChangesAsync();
                }
            }

            // populate registered class list
            var registrations = _context.Registration.Where(r => r.UserId == _studentID).ToList();
            if (registrations is not null)
            {
                List<int> classIDs = new List<int>();
                foreach (var r in registrations)
                {
                    classIDs.Add(r.ClassId);
                }
                RegisteredClassIDs = classIDs;

                var classList = await _context.Class.Where(c => classIDs.Contains(c.Id)).ToListAsync();

                // Serialize the class list and store it in the user's cookie
                Response.Cookies.Append("ClassList", JsonConvert.SerializeObject(classList));
            }

            return RedirectToPage("./Index");
        }

        public async Task<int> CreateRegistrationLogic(Models.Registration r)
        {
            _context.Registration.Add(r);
            await _context.SaveChangesAsync();

            return 0;
        }
    }
}
