using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using LMS.Data;
using LMS.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Identity.Client;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Data;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Hosting;
using System.Diagnostics;


namespace LMS.Pages.Submission
{
    [Authorize(Roles = "Student")]
    public class IndexModel : PageModel
    {
        private readonly LMS.Data.LMSContext _context;

        private int _studentID;

        private int? _classID;

        public bool isSubmitted;

        [FromQuery(Name = "id")]
        public string? ClassIDParam { get; set; }

        public Models.Assignment Assignment { get; set; } = default!;

        [BindProperty]
        public Models.Submission Submission { get; set; } = default!;

        public IndexModel(LMS.Data.LMSContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Get user ID
            if (!User.Identity.IsAuthenticated)
            {
                // Sign out the user
                HttpContext.SignOutAsync("MyCookieAuth");
                return RedirectToPage(".../Account/Login");
            }

            //Get current user's id to get the classes associated with that user
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
            _studentID = int.Parse(userIdClaim);

            //get assignment

            int tempClassID;
            _classID = int.TryParse(ClassIDParam, out tempClassID) ? tempClassID : (int?)null;

            if (_classID is not null)
            {
                Assignment = _context.Assignment.Where(a => a.Id == _classID).Include(a => a.Classes).First(); //will be one or none
            }

            //check if submitted
            var SubmissionList = _context.Submission.Where(s => s.StudentId == _studentID && s.AssignmentId == Assignment.Id);

            if (!SubmissionList.IsNullOrEmpty())
            {
                isSubmitted = true;
                Submission = SubmissionList.First();
                try
                {
                    Debug.WriteLine("************" + Assignment.Id + "************");
                    var userSubmissions = await _context.Submission
                        .Include(s => s.Assignment)
                        .Where(s => s.AssignmentId == Assignment.Id && s.Points.HasValue)
                        .ToListAsync();

                    var gradesData = userSubmissions.Select(s => CalculateGrade(s.Points, s.Assignment.MaxPoints))
                        .GroupBy(grade => grade)
                        .OrderByDescending(g => g.Key, new GradeComparer())
                        .Select(g => new object[] { g.Key, g.Count() })
                        .ToList();

                    // Add the header to the beginning of the list
                    gradesData.Insert(0, new object[] { "Letter Grade", "Student Grades" });

                    Debug.WriteLine("************" + gradesData + "************");
                    ViewData["GradesData"] = gradesData;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            else
            {
                isSubmitted = false;
            }

            return Page();
            //return RedirectToRoute("./Index", new { id = _classID });
        }


        public async Task<IActionResult> OnPostSubmitT()
        {
            // Get user ID
            if (!User.Identity.IsAuthenticated)
            {
                // Sign out the user
                HttpContext.SignOutAsync("MyCookieAuth");
                return RedirectToPage(".../Account/Login");
            }

            //Get current user's id to get the classes associated with that user
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
            _studentID = int.Parse(userIdClaim);

            Submission.Student = _context.User.Where(u => u.Id == _studentID).First();

            // get assignment ID
            _classID = int.Parse(Request.Form["aid"]);

            //get text
            string assignmentText = Request.Form["TextareaEnabled"];

            //build submission
            Submission.StudentId = _studentID;
            Submission.AssignmentId = _classID ?? -1;
            Submission.TurnInTime = DateTime.Now;
            Submission.SubmissionText = assignmentText;
            _context.Submission.Add(Submission);
            await _context.SaveChangesAsync();

            //set assignment info
            if (_classID is not null)
            {
                Assignment = _context.Assignment.Where(a => a.Id == _classID).Include(a => a.Classes).First(); //will be one or none
            }

            isSubmitted = true;
            Response.Cookies.Append("ConfettiTriggered", "true");

            // Create Notification for Student's Graded Assignment
            var newNotification = new Notification
            {
                Title = "Assignment Submitted",
                Message = $"{Submission.Student.FirstName} {Submission.Student.LastName} - {Submission.Assignment.Title}, {Submission.Assignment.Classes.CourseNumber}",
                NotifyDate = DateTime.Now,
                AssignmentId = Submission.AssignmentId,
                UserId = Submission.Assignment.Classes.ProfId
            };

            _context.Notification.Add(newNotification);
            await _context.SaveChangesAsync();

            //return RedirectToRoute("./Index", new {id = _classID});
            return Page();
        }


        /*
        public async Task OnGetGrades(int assignmentId)
        {
            try
            {
                Debug.WriteLine("************" + assignmentId + "************");
                var userSubmissions = await _context.Submission
                    .Include(s => s.Assignment)
                    .Where(s => s.AssignmentId == assignmentId)
                    .ToListAsync();

                var gradesData = userSubmissions.Select(s => CalculateGrade(s.Points, s.Assignment.MaxPoints))
                    .GroupBy(grade => grade)
                    .Select(g => new object[] { g.Key, g.Count() })
                    .ToList();

                Debug.WriteLine("************" + gradesData + "************");
                ViewData["GradesData"] = gradesData;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
        */

        public class GradeComparer : IComparer<string>
        {
            private static readonly List<string> GradeOrder = new List<string> { "F", "D-", "D", "D+", "C-", "C", "C+", "B-", "B", "B+", "A-", "A" };

            public int Compare(string x, string y)
            {
                return GradeOrder.IndexOf(x).CompareTo(GradeOrder.IndexOf(y));
            }
        }


        public string CalculateGrade(int? score, int maxPoints)
        {
            // Calculate the percentage
            double percent = (double)score / maxPoints;

            if (percent >= 0.94)
            {
                return "A";
            }
            else if (percent >= 0.9)
            {
                return "A-";
            }
            else if (percent >= 0.87)
            {
                return "B+";
            }
            else if (percent >= 0.84)
            {
                return "B";
            }
            else if (percent >= 0.8)
            {
                return "B-";
            }
            else if (percent >= 0.77)
            {
                return "C+";
            }
            else if (percent >= 0.74)
            {
                return "C";
            }
            else if (percent >= 0.7)
            {
                return "C-";
            }
            else if (percent >= 0.67)
            {
                return "D+";
            }
            else if (percent >= 0.64)
            {
                return "D";
            }
            else if (percent >= 0.6)
            {
                return "D-";
            }
            else
            {
                return "F";
            }
        }

public async Task<IActionResult> OnPostSubmitF()
        {
            // Get user ID
            if (!User.Identity.IsAuthenticated)
            {
                // Sign out the user
                HttpContext.SignOutAsync("MyCookieAuth");
                return RedirectToPage(".../Account/Login");
            }

            //Get current user's id to get the classes associated with that user
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
            _studentID = int.Parse(userIdClaim);

            // get assignment ID
            _classID = int.Parse(Request.Form["aid"]);

            var file = Request.Form.Files["SFile"];

            if (file is not null && file.Length > 0)
            {
                var originalFileName = file.FileName;
                var fileExtension = Path.GetExtension(file.FileName);
                var uniqueFileName = $"{originalFileName}_{Guid.NewGuid()}{fileExtension}";

                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "file_submissions", uniqueFileName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Update the Submission model with the file information
                Submission.SubmissionFileName = uniqueFileName;
                Submission.SubmissionFilePath = savePath; // Store the full path for reference if needed
            }

            //build submission
            Submission.StudentId = _studentID;
            Submission.AssignmentId = _classID ?? -1;
            Submission.TurnInTime = DateTime.Now;
            _context.Submission.Add(Submission);
            await _context.SaveChangesAsync();

            //set assignment info
            if (_classID is not null)
            {
                Assignment = _context.Assignment.Where(a => a.Id == _classID).Include(a => a.Classes).First(); //will be one or none
            }

            isSubmitted = true;
            Response.Cookies.Append("ConfettiTriggered", "true");

            return Page();
        }

        public async Task<IActionResult> OnPostRemove()
        {
            // Get user ID
            if (!User.Identity.IsAuthenticated)
            {
                // Sign out the user
                HttpContext.SignOutAsync("MyCookieAuth");
                return RedirectToPage(".../Account/Login");
            }

            //Get current user's id to get the classes associated with that user
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
            _studentID = int.Parse(userIdClaim);

            // get submission ID
            int subID = int.Parse(Request.Form["sid"]);

            // get submission
            var submissionToDelete = await _context.Submission.FindAsync(subID);

            if (!string.IsNullOrEmpty(submissionToDelete.SubmissionFileName))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "file_submissions", submissionToDelete.SubmissionFileName);

                // Check if the file exists before attempting to delete it
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            // get assignment ID
            //Assignment = await _context.Assignment.FindAsync(submissionToDelete.AssignmentId);
            Assignment = _context.Assignment.Where(s => s.Id == submissionToDelete.AssignmentId).Include(a => a.Classes).First();

            _context.Submission.Remove(submissionToDelete);
            await _context.SaveChangesAsync();

            //check if submitted
            var SubmissionList = _context.Submission.Where(s => s.StudentId == _studentID && s.AssignmentId == Assignment.Id);

            if (!SubmissionList.IsNullOrEmpty())
            {
                isSubmitted = true;
                Submission = SubmissionList.First();
            }
            else
            {
                isSubmitted = false;
            }


            return Page();
        }

        public IActionResult OnGetDownloadFile(int submissionId)
        {
            var submission = _context.Submission.FirstOrDefault(s => s.Id == submissionId);

            if (submission == null || string.IsNullOrEmpty(submission.SubmissionFileName))
            {
                // Handle case where submission or file does not exist
                return NotFound();
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "file_submissions", submission.SubmissionFileName);

            var fileStream = new FileStream(filePath, FileMode.Open);

            // Use FileResult to return the file
            return File(fileStream, "application/octet-stream", submission.SubmissionFileName);
        }
    }
}
