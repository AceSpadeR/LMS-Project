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


namespace LMS.Pages.Classes
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly LMS.Data.LMSContext _context;

        public DetailsModel(LMS.Data.LMSContext context)
        {
            _context = context;
        }

        public Models.Class Classes { get; set; } = default!;

        public IList<Assignment> Assignment { get; set; } = default!;

        public IList<Models.Submission> Submissions { get; set; } = default!;

        public double FinalGradePercentage { get; set; }

        public int TotalPointsReceived { get; set; }

        public int TotalMaxPoints { get; set; }

        public int[] studentGrades = new int[12];

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Get current user's id to get the classes associated with that user
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
            var userId = int.Parse(userIdClaim);

            var classes = await _context.Class.FirstOrDefaultAsync(m => m.Id == id);
            var assignment = await _context.Assignment.Where(a => a.ClassId == id).OrderBy(a => a.DueDate).ToListAsync();
            var submissions = await _context.Submission.Where(s => s.StudentId == userId).ToListAsync();

            if (User.IsInRole("Instructor"))
            {
                var assignmentIds = assignment.Select(a => a.Id).ToList();
                submissions = await _context.Submission.Where(s => assignmentIds.Contains(s.AssignmentId)).ToListAsync();

                var registeredUsers = _context.Registration
                        .Where(r => r.ClassId == id)
                        .Select(r => r.Users) 
                        .ToList();

                await GetStudentGrades(studentGrades, registeredUsers, id);
            }
            else if (User.IsInRole("Student"))
            {
                FinalGradePercentage = await returnStudentFinalGrade(userId, id);
            }

            if (classes == null)
            {
                return NotFound();
            }
            else
            {
                Classes = classes;
                Assignment = assignment;
                Submissions = submissions;
            }

            return Page();
        }

        public async Task<double> returnStudentFinalGrade(int userId, int? id)
        {
            var assignment = await _context.Assignment.Where(a => a.ClassId == id).OrderBy(a => a.DueDate).ToListAsync();

            double grade;
            // Calculate total points received from graded submissions
            var assignmentIds = assignment.Select(a => a.Id).ToList();
            var gradedSubmissions = await _context.Submission.Where(s => s.StudentId == userId && assignmentIds.Contains(s.AssignmentId) && s.Points != null).ToListAsync();
            TotalPointsReceived = (int)gradedSubmissions.Sum(s => s.Points);

            // Calculate total max points from assignments that have been graded 
            var gradedAssignmentIds = gradedSubmissions.Select(s => s.AssignmentId).Distinct().ToList();
            var gradedAssignments = assignment.Where(a => gradedAssignmentIds.Contains(a.Id)).ToList();
            TotalMaxPoints = gradedAssignments.Sum(a => a.MaxPoints);

            grade = (double)TotalPointsReceived / TotalMaxPoints;

            return grade;
        }

        //Create function that will be used to find all submissions and store them into an array
        //A = Index 0, A- = Index 1, B+ = Index 2 ...
        public async Task GetStudentGrades(int[] studentGrades, List<User>? registeredUsers, int? id)
        {
            double percent;
            if (registeredUsers != null)
            {
                foreach (var user in registeredUsers)
                {
                    percent = await returnStudentFinalGrade(user.Id, id);
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
        }
    }
}
