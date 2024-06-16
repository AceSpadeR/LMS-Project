using LMS.Data;
using LMS.Models;
using LMS.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using LMS.Pages.Classes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using System.Configuration;


namespace LMSTest
{
    [TestClass]
    public class CreateRegistrationTest
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
        public void StudentCreateRegistrationTest()
        {
            Task.Run(async () =>
            {

                // set up
                LMSContext _context = GetDbContext();

                //var _context = new Mock<LMSContext>();
                List<Registration> registrations = await _context.Registration.ToListAsync();

                // get registration count
                int registrationCount = registrations.Count();

                // get ID
                int classID = _context.Class.ToList().First().Id;
                int userID = _context.User.ToList().First().Id;


                // get instances
                Class existingClass = await _context.Class.FindAsync(classID);
                User existingUser = await _context.User.FindAsync(userID);

                // add registration
                Registration newRegistration = new Registration
                {
                    ClassId = classID,
                    UserId = userID,
                    Classes = existingClass,
                    Users = existingUser
                    
                };

                LMS.Pages.Registration.CreateModel m = new LMS.Pages.Registration.CreateModel(_context);
                await m.CreateRegistrationLogic(newRegistration);

                // assert registrations count + 1
                registrations = await _context.Registration.ToListAsync();
                int newRegistrationsCount = registrations.Count();

                Assert.AreEqual(newRegistrationsCount, registrationCount+1);

                //remove
                _context.Registration.Remove(newRegistration);
                await _context.SaveChangesAsync();
                registrations = await _context.Registration.ToListAsync();
                int droppedRegistrationsCount = registrations.Count();
                Assert.AreEqual(droppedRegistrationsCount, registrationCount);

            }).GetAwaiter().GetResult();
        }

    }
}