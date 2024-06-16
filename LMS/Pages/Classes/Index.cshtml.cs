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
    public class IndexModel : PageModel
    {
        private readonly LMS.Data.LMSContext _context;

        public IndexModel(LMS.Data.LMSContext context)
        {
            _context = context;
        }

        public IList<Models.Class> Classes { get;set; } = default!;

        public async Task OnGetAsync()
        {
            //Get current user's id to get the classes associated with that user
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
            var instructorID = int.Parse(userIdClaim);

            // Check if the class list is stored in the cookie
            var classListCookie = HttpContext.Request.Cookies["ClassList"];
            if (!string.IsNullOrEmpty(classListCookie))
            {
                // If cached, retrieve the class list from the cache
                Classes = JsonConvert.DeserializeObject<List<Class>>(classListCookie);
            }
            else
            {
                Classes = await _context.Class.Where(c => c.ProfId == instructorID).ToListAsync();

                // Serialize the class list and store it in the user's cookie
                Response.Cookies.Append("ClassList", JsonConvert.SerializeObject(Classes));
            }
        }
    }
}
