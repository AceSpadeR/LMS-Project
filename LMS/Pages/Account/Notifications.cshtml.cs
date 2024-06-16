using LMS.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LMS.Pages.Account
{
    public class NotificationsModel : PageModel
    {
        private readonly LMS.Data.LMSContext _context;

        public NotificationsModel(LMS.Data.LMSContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int NotificationIdToDelete { get; set; }


        public IActionResult OnGet()
        {
            return NotFound();
        }

        public IActionResult OnGetNotifications()
        {
            //Get current user's id to get the classes associated with that user
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
            var userId = int.Parse(userIdClaim);

            var notifications = _context.Notification.Where(n => n.UserId == userId).OrderByDescending(n => n.NotifyDate).ToList();

            // Return notifications as JSON
            return new JsonResult(notifications);
        }


        public async Task<IActionResult> OnPostDeleteNotificationAsync()
        {
            var notificationToDelete = await _context.Notification.FindAsync(NotificationIdToDelete);

            if (notificationToDelete == null)
            {
                return NotFound();
            }

            //code from method
            await DeleteNotificationLogic(notificationToDelete);

            return new NoContentResult();
        }

        public async Task<int> DeleteNotificationLogic(Notification notificationToDelete)
        {

            _context.Notification.Remove(notificationToDelete);
            await _context.SaveChangesAsync();

            return 0;//done

        }
    }
}
