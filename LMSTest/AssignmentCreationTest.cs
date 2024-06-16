using LMS.Data;
using LMS.Models;
using LMS.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using LMS.Pages.Classes.Assignments;


namespace LMSTest
{
    [TestClass]
    public class AssignmentCreationTest
    {
        public LMSContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<LMSContext>()
                //.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .UseSqlServer("Server=tcp:stellar-software.database.windows.net,1433;Initial Catalog=Stellar_Software;Persist Security Info=False;User ID=stellar-admin;Password=StellSoft.3750;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;")
                .Options;

            //var dbContext = new LMSContext(options);

            // Add any data you need for your tests here

            return new LMSContext(options);//dbContext;
        }


        [TestMethod]
        public void CreateAssignmentTestMethod()
        {
            Task.Run(async () =>
            {

                int assignmentCountBefore = 0;
                // set up
                LMSContext _context = GetDbContext();

                // get assignment count
                List<Assignment> assignmentBefore = await _context.Assignment.Where(u => u.ClassId == 1).ToListAsync();
                if(assignmentBefore != null) {
                    assignmentCountBefore = assignmentBefore.Count();
                }

                // get class instance
                Class existingClass = await _context.Class.FindAsync(1);

                // add new assignment
                Assignment newAssignment = new Assignment
                {
                    Title = "Test Assignment",
                    DueDate = DateTime.Now,
                    Description = "Description",
                    MaxPoints = 100,
                    SubmissionType = "Text",
                    Classes = existingClass
                };


                LMS.Pages.Classes.Assignments.CreateModel m = new CreateModel(_context);
                await m.createAssignmentLogic(newAssignment);

                // assert asignment count + 1
                List<Assignment> assignmentsAfter = await _context.Assignment.Where(u => u.ClassId == 1).ToListAsync();
                int assignmentCountAfter = assignmentsAfter.Count();

                Assert.AreEqual(assignmentCountAfter, assignmentCountBefore + 1);

                // delete test assignment
                if (newAssignment != null && assignmentsAfter.Count > assignmentCountBefore)
                {
                    _context.Attach(newAssignment);
                    _context.Assignment.Remove(newAssignment);
                    await _context.SaveChangesAsync();
                }

                // assert assignment count back to original
                List<Assignment> assignmentFinal = await _context.Assignment.Where(u => u.ClassId == 1).ToListAsync();
                int assginmentCountFinal = assignmentFinal.Count();

                //Assert.AreEqual(assginmentCountFinal, assignmentCountBefore);
            }).GetAwaiter().GetResult();
        }
    }
}