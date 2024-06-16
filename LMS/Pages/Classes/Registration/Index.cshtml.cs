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
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;

namespace LMS.Pages.Registration
{
    [Authorize(Roles = "Student")]
    public class IndexModel : PageModel
    {
        private readonly LMS.Data.LMSContext _context;

        private int _studentID;

        public IndexModel(LMS.Data.LMSContext context)
        {
            _context = context;
        }

        //public IList<Models.Registration> Registration { get;set; } = default!;

        public IList<Models.Class> Classes { get; set; } = default!;
        //public IList<Models.User> Users { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
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

            // Check if the class list is stored in the cookie
            var classListCookie = HttpContext.Request.Cookies["ClassList"];
            if (!string.IsNullOrEmpty(classListCookie))
            {
                // If cached, retrieve the class list from the cache
                Classes = JsonConvert.DeserializeObject<List<Class>>(classListCookie);
            }
            else
            {
                List<int> studentClassIDs = new List<int>();

                foreach (var x in _context.Registration.Where(c => c.UserId == _studentID).ToList())
                {
                    studentClassIDs.Add(x.ClassId);
                }

                Classes = await _context.Class.Where(c => studentClassIDs.Contains(c.Id)).ToListAsync();

                // Serialize the class list and store it in the user's cookie
                Response.Cookies.Append("ClassList", JsonConvert.SerializeObject(Classes));
            }


            //Users = await _context.User.ToListAsync();
            //Registration = await _context.Registration
            //    .Include(r => r.Classes)
            //    .Include(r => r.Users).ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostDrop()
        {
            //get user id
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
            var dropList = _context.Registration.Where(r =>  r.UserId == _studentID && r.ClassId == dropClassID).ToList();

            foreach (var d in dropList)
            {
                if (d != null)
                {
                    var eventToDelete = await _context.Event.FirstOrDefaultAsync(e => e.ClassId == dropClassID && e.UserId == _studentID);
                    if (eventToDelete != null)
                    {
                        _context.Event.Remove(eventToDelete);
                    }

                    _context.Registration.Remove(d);
                    await _context.SaveChangesAsync();
                }
            }

            //populate class list
            List<int> studentClassIDs = new List<int>();

            foreach (var x in _context.Registration.Where(c => c.UserId == _studentID).ToList())
            {
                studentClassIDs.Add(x.ClassId);
            }

            Classes = await _context.Class.Where(c => studentClassIDs.Contains(c.Id)).ToListAsync();

            // Serialize the class list and store it in the user's cookie
            Response.Cookies.Append("ClassList", JsonConvert.SerializeObject(Classes));

            return Page();
        }

    }
}
