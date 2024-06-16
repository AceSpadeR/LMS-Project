using LMS.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace LMS.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly LMS.Data.LMSContext _context;

        public IEnumerable<Class> ClassList;
        public IEnumerable<Assignment> AssignmentList;

        public IndexModel(LMS.Data.LMSContext context)
        {
            _context = context;
            ClassList = new List<Class>();
            AssignmentList = new List<Assignment>();
        }



        public async Task<IActionResult> OnGet()
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Sign out the user
                await HttpContext.SignOutAsync("MyCookieAuth");
                return RedirectToPage("./Account/Login");
            }

            //Get current user's id to get the classes associated with that user
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
            int userId = int.Parse(userIdClaim);

            // Check if the class list is stored in the cookie
            var classListCookie = HttpContext.Request.Cookies["ClassList"];
            if (!string.IsNullOrEmpty(classListCookie))
            {
                // If cached, retrieve the class list from the cache
                ClassList = JsonConvert.DeserializeObject<List<Class>>(classListCookie);

                // Retrieve the assignment list (no matter if class list is cached or not)
                await RetrieveAssignmentList();
            }
            else
            {

                List<int> classIDs = new List<int>();

                if (User.IsInRole("Instructor"))
                {
                    ClassList = _context.Class.Where(c => c.ProfId == userId).ToList();

                    classIDs = ClassList.Select(c => c.Id).ToList();

                    AssignmentList = _context.Assignment.Where(a => classIDs.Contains(a.ClassId) && _context.Submission.Any(s => s.AssignmentId == a.Id && s.Points == null)).OrderBy(a => a.DueDate).Take(5).ToList();
                }
                else if (User.IsInRole("Student"))
                {
                    classIDs = _context.Registration.Where(c => c.UserId == userId).Select(c => c.ClassId).ToList();

                    var submittedAssignmentIds = _context.Submission.Where(s => s.StudentId == userId).Select(s => s.AssignmentId).ToList();

                    ClassList = _context.Class.Where(c => classIDs.Contains(c.Id)).ToList();
                    AssignmentList = _context.Assignment.Where(a => classIDs.Contains(a.ClassId) && !submittedAssignmentIds.Contains(a.Id)).OrderBy(a => a.DueDate).Take(5).ToList();
                }

                // Serialize the class list and store it in the user's cookie
                Response.Cookies.Append("ClassList", JsonConvert.SerializeObject(ClassList));
            }

            return Page();
        }
        private async Task RetrieveAssignmentList()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
            int userId = int.Parse(userIdClaim);

            List<int> classIDs;

            if (User.IsInRole("Instructor"))
            {
                classIDs = ClassList.Select(c => c.Id).ToList();

                AssignmentList = _context.Assignment.Where(a => classIDs.Contains(a.ClassId) && _context.Submission.Any(s => s.AssignmentId == a.Id && s.Points == null)).Include(a => a.Classes).OrderBy(a => a.DueDate).Take(5).ToList();
            }
            else if (User.IsInRole("Student"))
            {
                classIDs = _context.Registration.Where(c => c.UserId == userId).Select(c => c.ClassId).ToList();

                var submittedAssignmentIds = _context.Submission.Where(s => s.StudentId == userId).Select(s => s.AssignmentId).ToList();

                AssignmentList = _context.Assignment.Where(a => classIDs.Contains(a.ClassId) && !submittedAssignmentIds.Contains(a.Id)).Include(a => a.Classes).OrderBy(a => a.DueDate).Take(5).ToList();
            }
        }
    }
}
