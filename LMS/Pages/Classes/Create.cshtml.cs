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
using Newtonsoft.Json;

namespace LMS.Pages.Classes
{
    [Authorize(Roles = "Instructor")]
    public class CreateModel : PageModel
    {
        private readonly LMS.Data.LMSContext _context;

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

            return Page();
        }

        [BindProperty]
        public Models.Class Classes { get; set; } = default!;

        [BindProperty]
        public List<SelectListItem> DepartmentList { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //Add current user's id to the new Classes object to add to database
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
            var instructorID = int.Parse(userIdClaim);
            Classes.ProfId = instructorID;


            //code from method
            await CreateClassLogic(Classes);

            var classList = _context.Class.Where(c => c.ProfId == instructorID).ToList();

            // Serialize the class list and store it in the user's cookie
            Response.Cookies.Append("ClassList", JsonConvert.SerializeObject(classList));

            // Create a new event
            var newEvent = new Event
            {
                EventType = $"{Classes.CourseNumber}: {Classes.Title}",
                EventDescription = $"{Classes.DepartmentName}, {Classes.Location}",
                UserId = Classes.ProfId,
                ClassId = Classes.Id
            };

            _context.Event.Add(newEvent);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        public async Task<int> CreateClassLogic(Models.Class x)
        {

            _context.Class.Add(x);
             await _context.SaveChangesAsync();

            return 0;//done

        }
        
    }
}
