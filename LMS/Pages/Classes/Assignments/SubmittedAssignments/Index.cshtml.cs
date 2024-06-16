using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LMS.Data;
using LMS.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Authorization;

namespace LMS.Pages.Classes.Assignments.SubmittedAssignments
{
    [Authorize(Roles = "Instructor")]
    public class IndexModel : PageModel
    {
        private readonly LMS.Data.LMSContext _context;

        public IndexModel(LMS.Data.LMSContext context)
        {
            _context = context;
        }

        public IList<Models.Submission> Submission { get;set; } = default!;

        public Assignment assignment { get; set; } = default!;

        public int[] studentGrades = new int[12];

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            assignment = await _context.Assignment.FirstOrDefaultAsync(m => m.Id == id);

            Submission = await _context.Submission
                .Include(s => s.Assignment)
                .Include(s => s.Student)
                .Where(s =>  s.AssignmentId == id)
                .ToListAsync();

            if(Submission != null)
            {
                getStudentGrades(studentGrades);
            }

            return Page();
        }

        //Create function that will be used to find all submissions and store them into an array
        //A = Index 0, A- = Index 1, B+ = Index 2 ...
        public int[] getStudentGrades(int[] studentGrades)
        {
            double percent;
            foreach (var submission in Submission)
            {
                if(submission.Points != null)
                {
                    percent = (double)submission.Points / assignment.MaxPoints;
                    if (percent >= 0.94)
                    {
                        studentGrades[0] += 1;
                    }
                    else if (percent >= 0.9)
                    {
                        studentGrades[1] += 1;
                    }
                    else if (percent >= 0.87)
                    {
                        studentGrades[2] += 1;
                    }
                    else if (percent >= 0.84)
                    {
                        studentGrades[3] += 1;
                    }
                    else if (percent >= 0.8)
                    {
                        studentGrades[4] += 1;
                    }
                    else if (percent >= 0.77)
                    {
                        studentGrades[5] += 1;
                    }
                    else if (percent >= 0.74)
                    {
                        studentGrades[6] += 1;
                    }
                    else if (percent >= 0.7)
                    {
                        studentGrades[7] += 1;
                    }
                    else if (percent >= 0.67)
                    {
                        studentGrades[8] += 1;
                    }
                    else if (percent >= 0.64)
                    {
                        studentGrades[9] += 1;
                    }
                    else if (percent >= 0.6)
                    {
                        studentGrades[10] += 1;
                    }
                    else
                    {
                        studentGrades[11] += 1;
                    }
                }
            }

            return studentGrades;
        }
    }
}
