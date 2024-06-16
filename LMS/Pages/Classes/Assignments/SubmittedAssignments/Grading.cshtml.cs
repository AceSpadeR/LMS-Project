using LMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LMS.Pages.Classes.Assignments.SubmittedAssignments
{
    [Authorize(Roles = "Instructor")]
    public class GradingModel : PageModel
    {
        private readonly LMS.Data.LMSContext _context;

        public GradingModel(LMS.Data.LMSContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Submission currentSubmission { get; set; } = default!;

        [BindProperty]
        public int grade { get; set; }

        public int submissionId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var submission = await _context.Submission
                .Include(s => s.Assignment)
                .Include(s => s.Student)
                .Include(s => s.Assignment.Classes)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (submission == null)
            {
                return NotFound();
            }

            currentSubmission = submission;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (currentSubmission == null || currentSubmission.Id <= 0)
            {
                return NotFound();
            }

            //currentSubmission.Points = grade;
            currentSubmission.Assignment = await _context.Assignment.Where(a => a.Id == currentSubmission.AssignmentId).FirstAsync();
            currentSubmission.Student = await _context.User.Where(u => u.Id == currentSubmission.StudentId).FirstAsync();
            currentSubmission.Assignment.Classes = await _context.Class.Where(u => u.Id == currentSubmission.Assignment.ClassId).FirstAsync();

            _context.Attach(currentSubmission).Property(x => x.Points).IsModified = true;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubmissionExists(currentSubmission.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Create Notification for Student's Graded Assignment
            var newNotification = new Notification
            {
                Title = "Assignment Graded",
                Message = $"{currentSubmission.Assignment.Title}, {currentSubmission.Assignment.Classes.CourseNumber}",
                NotifyDate = DateTime.Now,
                AssignmentId = currentSubmission.AssignmentId,
                UserId = currentSubmission.StudentId
            };

            _context.Notification.Add(newNotification);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { id = currentSubmission.AssignmentId });
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

        private bool SubmissionExists(int id)
        {
            return _context.Submission.Any(e => e.Id == id);
        }
    }
}
