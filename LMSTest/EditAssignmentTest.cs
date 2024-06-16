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
    public class EditAssignmentTest
    {
        public LMSContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<LMSContext>()
                //.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=LMSContext-d7d26f64-0e73-4b7a-a169-7903d2073083")
                .Options;

            //var dbContext = new LMSContext(options);

            // Add any data you need for your tests here

            return new LMSContext(options);//dbContext;
        }


        [TestMethod]
        public void EditAssignmentTestMethod()
        {
            Task.Run(async () =>
            {
                LMSContext _context = GetDbContext();

                //Find existing assignment
                Assignment existingAssignment = await _context.Assignment.FirstOrDefaultAsync(a => a.ClassId == 1);
                Assert.IsNotNull(existingAssignment);

                //Save all the original information of the assignment
                string originalTitle = existingAssignment.Title;
                DateTime originalDueDate = existingAssignment.DueDate;
                string originalDescription = existingAssignment.Description;
                int originalMaxPoints = existingAssignment.MaxPoints;
                string originalSubmissionType = existingAssignment.SubmissionType;

                //Edit the assignment with new information
                existingAssignment.Title = "New Test Assignment";
                existingAssignment.DueDate = DateTime.Now.AddDays(7); 
                existingAssignment.Description = "New Description";
                existingAssignment.MaxPoints = 150; 
                existingAssignment.SubmissionType = "File"; 

                await _context.SaveChangesAsync();

                // get the assignment
                Assignment editedAssignment = await _context.Assignment.FirstOrDefaultAsync(a => a.ClassId == 1 && a.Id == existingAssignment.Id);
                Assert.IsNotNull(editedAssignment);

                //make sure the assignment was edited properly
                Assert.AreEqual("New Test Assignment", editedAssignment.Title);
                Assert.AreEqual(DateTime.Today.AddDays(7).Date, editedAssignment.DueDate.Date);
                Assert.AreEqual("New Description", editedAssignment.Description);
                Assert.AreEqual(150, editedAssignment.MaxPoints);
                Assert.AreEqual("File", editedAssignment.SubmissionType);

                //Change back all information of the assignment
                editedAssignment.Title = originalTitle;
                editedAssignment.DueDate = originalDueDate;
                editedAssignment.Description = originalDescription;
                editedAssignment.MaxPoints = originalMaxPoints;
                editedAssignment.SubmissionType = originalSubmissionType;

                await _context.SaveChangesAsync();
            }).GetAwaiter().GetResult();
        }
    }
}