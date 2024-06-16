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
    public class EventTests
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
        public void CreateEventTest()
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

                //var _context = new Mock<LMSContext>();
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


                SignUpModel m = new SignUpModel(_context);
                await m.CreateUserAccountLogic(newUser);

                //var _context = new Mock<LMSContext>();
                List<Event> eventsBefore = await _context.Event.ToListAsync();
                // get class count
                int eventCountBefore = eventsBefore.Count();
                //create Event 
                Event newEvent = new Event();
                newEvent.EventType = newClass.CourseNumber+ ": " + newClass.Title;
                newEvent.EventDescription= newClass.DepartmentName;
                newEvent.AssignmentId = -1;
                newEvent.UserId = newUser.Id;
                newEvent.ClassId = newClass.Id;

                // assert user count + 1
                List<User> usersAfter = await _context.User.Where(u => u.Role == "Student").ToListAsync();
                int studentUserCountAfter = usersAfter.Count();
                List<Class> classesAfter = await _context.Class.ToListAsync();
                int classCountAfter = usersAfter.Count();
                List<Event> eventsAfter = await _context.Event.ToListAsync();
                int eventCountAfter = usersAfter.Count();

                Assert.AreEqual(studentUserCountAfter, studentUserCountBefore + 1);
                Assert.AreEqual(classCountAfter, studentUserCountBefore + 1);
                Assert.AreEqual(eventCountAfter, studentUserCountBefore + 1);

                // delete test user
                if (newUser != null && usersAfter.Count > studentUserCountBefore)
                {
                    _context.Attach(newUser);
                    _context.User.Remove(newUser);
                    await _context.SaveChangesAsync();
                }
                // delete class
                if (newClass != null && classesAfter.Count > classCountBefore)
                {
                    _context.Attach(newClass);
                    _context.Class.Remove(newClass);
                    await _context.SaveChangesAsync();
                }
                // delete event
                if (newEvent != null && eventsAfter.Count > eventCountBefore)
                {
                    _context.Attach(newEvent);
                    _context.Event.Remove(newEvent);
                    await _context.SaveChangesAsync();
                }
                // assert user count back to original
                List<User> usersFinal = await _context.User.Where(u => u.Role == "Student").ToListAsync();
                int studentUserCountFinal = usersFinal.Count();

                List<Class> classesFinal = await _context.Class.ToListAsync();
                int classCountFinal = classesFinal.Count();

                List<Event> eventsFinal = await _context.Event.ToListAsync();
                int eventCountFinal = eventsFinal.Count();

                Assert.AreEqual(studentUserCountFinal, studentUserCountBefore);
                Assert.AreEqual(classCountFinal, classCountBefore);
                Assert.AreEqual(eventCountFinal, eventCountBefore);
            }).GetAwaiter().GetResult();
        }

     }
}

