using LMS.Data;
using LMS.Models;
using LMS.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using LMS.Pages.Account;


namespace LMSTest
{
    [TestClass]
    public class UserAccountCreationTests
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
        public void CreateStudentUserAccountTest()
        {
            Task.Run(async () =>
            {
                // set up
                LMSContext _context = GetDbContext();

                // get Student User count
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

                SignUpModel m = new SignUpModel(_context);
                await m.CreateUserAccountLogic(newUser);

                // assert user count + 1
                List<User> usersAfter = await _context.User.Where(u => u.Role == "Student").ToListAsync();
                int studentUserCountAfter = usersAfter.Count();

                Assert.AreEqual(studentUserCountAfter, studentUserCountBefore + 1);

                // delete test user
                if (newUser != null && usersAfter.Count > studentUserCountBefore)
                {
                    _context.Attach(newUser);
                    _context.User.Remove(newUser);
                    await _context.SaveChangesAsync();
                }

                // assert user count back to original
                List<User> usersFinal = await _context.User.Where(u => u.Role == "Student").ToListAsync();
                int studentUserCountFinal = usersFinal.Count();

                Assert.AreEqual(studentUserCountFinal, studentUserCountBefore);
            }).GetAwaiter().GetResult();
        }


        [TestMethod]
        public void CreateInstructorUserAccountTest()
        {
            Task.Run(async () =>
            {
                // set up
                LMSContext _context = GetDbContext();

                // get Instructor User count
                List<User> usersBefore = await _context.User.Where(u => u.Role == "Instructor").ToListAsync();
                int instructorUserCountBefore = usersBefore.Count();

                // add Instructor User
                User newUser = new User();
                newUser.FirstName = "TestTeacherFirst";
                newUser.LastName = "TestTeacherLast";
                newUser.Email = $"testteacher{DateTime.Now.Ticks}@testteacher.com";
                newUser.BirthDate = DateTime.Parse("2000-1-1");
                newUser.Role = "Instructor";
                newUser.Password = "8B8493BF94A2C487923A085E69C5D4532BC2F2706657256AC9F73796035CA640734A02448FB72468E4F6F378750EC0FAB7951F696F18F8FBC93D4071EDBA4E1B";
                newUser.ConfirmPassword = "8B8493BF94A2C487923A085E69C5D4532BC2F2706657256AC9F73796035CA640734A02448FB72468E4F6F378750EC0FAB7951F696F18F8FBC93D4071EDBA4E1B";
                // Password: Teacher

                SignUpModel m = new SignUpModel(_context);
                await m.CreateUserAccountLogic(newUser);

                // assert user count + 1
                List<User> usersAfter = await _context.User.Where(u => u.Role == "Instructor").ToListAsync();
                int instructorUserCountAfter = usersAfter.Count();

                Assert.AreEqual(instructorUserCountAfter, instructorUserCountBefore + 1);

                // delete test user
                if (newUser != null && usersAfter.Count > instructorUserCountBefore)
                {
                    _context.Attach(newUser);
                    _context.User.Remove(newUser);
                    await _context.SaveChangesAsync();
                }

                // assert user count back to original
                List<User> usersFinal = await _context.User.Where(u => u.Role == "Instructor").ToListAsync();
                int instructorUserCountFinal = usersFinal.Count();

                Assert.AreEqual(instructorUserCountFinal, instructorUserCountBefore);
            }).GetAwaiter().GetResult();
        }

    }
}