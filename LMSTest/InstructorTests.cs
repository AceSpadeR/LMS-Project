using LMS.Data;
using LMS.Models;
using LMS.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using LMS.Pages.Classes;
using Microsoft.AspNetCore.Builder;


namespace LMSTest
{
    [TestClass]
    public class UnitTest1InstructorCreateClassTest
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
        public void InstructorCreateClassTest()
        {
            Task.Run(async () =>
            {

                // set up
                LMSContext _context = GetDbContext();

                //var _context = new Mock<LMSContext>();
                List<Class> classes = await _context.Class.ToListAsync();

                // get class count
                int classCount = classes.Count();

                // add class
                Class newClass = new Class();
                newClass.DepartmentName = "Math";
                newClass.CourseNumber = "MATH1010";
                newClass.Title = "Algebra";
                newClass.Location = "WSU101";
                newClass.CreditHours = 3;
                newClass.StartTime = DateTime.Now;
                newClass.EndTime = DateTime.Now;
                newClass.StartDate = DateTime.Now;
                newClass.EndDate = DateTime.Now;
                List<DayOfWeek> weekDays = new List<DayOfWeek>();
                weekDays.Add(DayOfWeek.Monday);
                newClass.MeetingDays = weekDays;
                newClass.ProfId = 1;

                LMS.Pages.Classes.CreateModel m = new LMS.Pages.Classes.CreateModel(_context);
                await m.CreateClassLogic(newClass);



                // assert class count + 1
                classes = await _context.Class.ToListAsync();
                int newClassCount = classes.Count();

                Assert.AreEqual(newClassCount, classCount+1);

                //Assert.AreEqual(2, i);
            }).GetAwaiter().GetResult();
        }
        
    }
}