using LMS.Data;
using LMS.Models;
using LMS.Pages.Account;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSTest
{
    [TestClass]
    public class SubmissionTest
    {
        public LMSContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<LMSContext>()
                //.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .UseSqlServer("Server=tcp:stellar-software.database.windows.net,1433;Initial Catalog=Stellar_Software;Persist Security Info=False;User ID=stellar-admin;Password=StellSoft.3750;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;")
                .Options;


            return new LMSContext(options);//dbContext;
        }

        [TestMethod]
        public void SubmissionCreationTest()
        {
            Task.Run(async () =>
            {
                // set up
                LMSContext _context = GetDbContext();

                // get Count for User
                List<User> usersBefore = await _context.User.Where(u => u.Role == "Student").ToListAsync();
                int studentUserCountBefore = usersBefore.Count();

                // add Student User
                User newUser = new User();
                newUser.FirstName = "TestStudentFirst";
                newUser.LastName = "TestStudentLast";
                newUser.Email = $"teststudent{DateTime.Now.Ticks}@teststudent.com";
                newUser.BirthDate = DateTime.Parse("2000-1-1");
                newUser.Role = "Student";
                newUser.Password = "2CBD1EC8285D03FC550E7ED3F926AEC05E4B5CF3C9BCCC500FD14BBBC698392BDCC021B19B43409CE08D0CBA53DBC4143012AE3435E70DBB34BC94419DE0B23F";
                newUser.ConfirmPassword = "2CBD1EC8285D03FC550E7ED3F926AEC05E4B5CF3C9BCCC500FD14BBBC698392BDCC021B19B43409CE08D0CBA53DBC4143012AE3435E70DBB34BC94419DE0B23F";
                // Password: Student

                List<Class> classesBefore = await _context.Class.ToListAsync();
                // get class count
                int classCountBefore = classesBefore.Count();

                // add class
                Class newClass = new Class();
                newClass.DepartmentName = "Computer Science";
                newClass.CourseNumber = "CS 3750";
                newClass.Title = "Software Engineering 2";
                newClass.Location = "Norda";
                newClass.CreditHours = 4;
                newClass.StartTime = DateTime.Now;
                newClass.EndTime = DateTime.Now;
                newClass.StartDate = DateTime.Now;
                newClass.EndDate = DateTime.Now;
                List<DayOfWeek> weekDays = new List<DayOfWeek>();
                weekDays.Add(DayOfWeek.Monday);
                newClass.MeetingDays = weekDays;
                newClass.ProfId = 1;

                // Count assignments
                List<Assignment> assignmentBefore = await _context.Assignment.Where(u => u.ClassId == newClass.Id).ToListAsync();
                int assignmentCountBefore = assignmentBefore.Count();


                Assignment newAssignment = new Assignment
                {
                    Title = "Test Assignment",
                    DueDate = DateTime.Now,
                    Description = "Description",
                    MaxPoints = 100,
                    SubmissionType = "Text",
                    Classes = newClass
                };

                SignUpModel m = new SignUpModel(_context);
                await m.CreateUserAccountLogic(newUser);

                _context.Assignment.Add(newAssignment);
                await _context.SaveChangesAsync();

                List<Registration> registrationsBefore = await _context.Registration.ToListAsync();
                int registrationCountBefore = registrationsBefore.Count();
                // Register the student for the class
                Registration newRegistration = new Registration
                {
                    ClassId = newClass.Id,
                    UserId = newUser.Id,
                    Classes = newClass,
                    Users = newUser
                };

                _context.Registration.Add(newRegistration);
                await _context.SaveChangesAsync();

                List<Submission> submissionsBefore = await _context.Submission.ToListAsync();
                // get class count
                int submissionsCountBefore = submissionsBefore.Count();

                Submission newSubmission = new Submission
                {
                    AssignmentId = newAssignment.Id, 
                    Assignment = newAssignment, 
                    StudentId = newRegistration.UserId, 
                    Student = newUser, 
                    TurnInTime = DateTime.Now,

                    Points = null, 
                    SubmissionText = "This is the text of the submission.", 
                    SubmissionFileName = null, 
                    SubmissionFilePath = null 
                };

                _context.Submission.Add(newSubmission);
                await _context.SaveChangesAsync();

                // assert user count + 1
                List<User> usersAfter = await _context.User.Where(u => u.Role == "Student").ToListAsync();
                int studentUserCountAfter = usersAfter.Count();

                List<Class> classesAfter = await _context.Class.ToListAsync();
                int classCountAfter = classesAfter.Count();

                List<Assignment> assignmentAfter = await _context.Assignment.Where(u => u.ClassId == newClass.Id).ToListAsync();
                int assignmentCountAfter = assignmentAfter.Count();

                List<Registration> registrationsAfter = await _context.Registration.ToListAsync();
                int registrationCountAfter = registrationsAfter.Count();

                List<Submission> submissionAfter = await _context.Submission.ToListAsync();
                int submissionCountAfter = submissionAfter.Count();

                Assert.AreEqual(studentUserCountAfter, studentUserCountBefore + 1);
                Assert.AreEqual(classCountAfter, classCountBefore + 1);
                Assert.AreEqual(registrationCountAfter, registrationCountBefore + 1);
                Assert.AreEqual(assignmentCountAfter, assignmentCountBefore + 1);
                Assert.AreEqual(submissionCountAfter, submissionsCountBefore + 1);

                // Delete the submission
                _context.Submission.Remove(newSubmission);
                await _context.SaveChangesAsync();

                // Delete the registration
                _context.Registration.Remove(newRegistration);
                await _context.SaveChangesAsync();

                // Delete the assignment
                _context.Assignment.Remove(newAssignment);
                await _context.SaveChangesAsync();

                // Delete the class
                _context.Class.Remove(newClass);
                await _context.SaveChangesAsync();

                // Delete the user
                _context.User.Remove(newUser);
                await _context.SaveChangesAsync();


                // Verify the submission is deleted
                Submission deletedSubmission = await _context.Submission.FirstOrDefaultAsync(s => s.Id == newSubmission.Id);
                Assert.IsNull(deletedSubmission, "Submission deletion failed");

                // Get the counts after deletion
                List<User> usersFinal = await _context.User.Where(u => u.Role == "Student").ToListAsync();
                int studentUserCountFinal = usersFinal.Count();

                List<Class> classesFinal = await _context.Class.ToListAsync();
                int classCountFinal = classesFinal.Count();

                List<Assignment> assignmentFinal = await _context.Assignment.Where(u => u.ClassId == newClass.Id).ToListAsync();
                int assignmentCountFinal = assignmentFinal.Count();

                List<Registration> registrationsFinal = await _context.Registration.ToListAsync();
                int registrationCountFinal = registrationsFinal.Count();

                List<Submission> submissionFinal = await _context.Submission.ToListAsync();
                int submissionCountFinal = submissionFinal.Count();

                // Assert counts back to original
                Assert.AreEqual(studentUserCountFinal, studentUserCountBefore, "User deletion failed");
                Assert.AreEqual(classCountFinal, classCountBefore, "Class deletion failed");
                Assert.AreEqual(registrationCountFinal, registrationCountBefore, "Registration deletion failed");
                Assert.AreEqual(assignmentCountFinal, assignmentCountBefore, "Assignment deletion failed");
                Assert.AreEqual(submissionCountFinal, submissionsCountBefore, "Submission deletion failed");
            }).GetAwaiter().GetResult();

        }

    }
}
