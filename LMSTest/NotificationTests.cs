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
    public class NotificationTests
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
        public void CreateNotificationTest()
        {
            Task.Run(async () =>
            {
                // set up
                LMSContext _context = GetDbContext();

                // get Notification count
                List<Notification> notificationsBefore = await _context.Notification.ToListAsync();
                int notificationCountBefore = notificationsBefore.Count();

                // add Notification
                Notification newNotification = new Notification();
                newNotification.Title = "Test";
                newNotification.Message = "Test";
                newNotification.NotifyDate = DateTime.Now;
                newNotification.AssignmentId = _context.Assignment.ToList().First().Id;
                newNotification.UserId = _context.User.ToList().First().Id;

                _context.Notification.Add(newNotification);
                await _context.SaveChangesAsync();

                // assert notification count + 1
                List<Notification> notificationsAfter = await _context.Notification.ToListAsync();
                int notificationCountAfter = notificationsAfter.Count();

                Assert.AreEqual(notificationCountAfter, notificationCountBefore + 1);

                // delete test Notification
                if (newNotification != null && notificationsAfter.Count > notificationCountBefore)
                {
                    _context.Attach(newNotification);
                    _context.Notification.Remove(newNotification);
                    await _context.SaveChangesAsync();
                }

                // assert notification count back to original
                List<Notification> notificationsFinal = await _context.Notification.ToListAsync();
                int notificationCountFinal = notificationsFinal.Count();

                Assert.AreEqual(notificationCountFinal, notificationCountBefore);
            }).GetAwaiter().GetResult();
        }


        [TestMethod]
        public void DeleteNotificationTest()
        {
            Task.Run(async () =>
            {
                // set up
                LMSContext _context = GetDbContext();

                // get Notification count
                List<Notification> notificationsBefore = await _context.Notification.ToListAsync();
                int notificationCountBefore = notificationsBefore.Count();

                // add Notification
                Notification newNotification = new Notification();
                newNotification.Title = "Test";
                newNotification.Message = "Test";
                newNotification.NotifyDate = DateTime.Now;
                newNotification.AssignmentId = _context.Assignment.ToList().First().Id;
                newNotification.UserId = _context.User.ToList().First().Id;

                _context.Notification.Add(newNotification);
                await _context.SaveChangesAsync();

                // assert notification count + 1
                List<Notification> notificationsAfter = await _context.Notification.ToListAsync();
                int notificationCountAfter = notificationsAfter.Count();

                Assert.AreEqual(notificationCountAfter, notificationCountBefore + 1);

                // delete test Notification
                NotificationsModel m = new NotificationsModel(_context);
                await m.DeleteNotificationLogic(newNotification);

                // assert notification count back to original
                List<Notification> notificationsFinal = await _context.Notification.ToListAsync();
                int notificationCountFinal = notificationsFinal.Count();

                Assert.AreEqual(notificationCountFinal, notificationCountBefore);
            }).GetAwaiter().GetResult();
        }
    }
}