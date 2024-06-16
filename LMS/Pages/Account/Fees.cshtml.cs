using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LMS.Data;
using LMS.Models;
using Stripe.Checkout;
using Stripe;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;


namespace LMS.Pages.Account
{
	[Authorize(Roles = "Student")]
    public class FeesModel : PageModel
    {
		private LMSContext _context;

		public FeesModel(LMSContext context)
		{
			_context = context;
		}

		[BindProperty]
		public User LoginUser { get; set; } = default!;

		public double TotalCreditHours { get; set; }


		
        public async Task<IActionResult> OnGetAsync(bool paymentSuccess = false)
		{
			//Get current user's id to get the classes associated with that user
			var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
			int userId = int.Parse(userIdClaim);

			var user = await _context.User.FirstOrDefaultAsync(u => u.Id == userId);

			//Linq query to get the total credit hours
			TotalCreditHours = await _context.Registration
				.Where(r => r.UserId == userId)
				.Join(
					_context.Class,
					registration => registration.ClassId,
					classes => classes.Id,
					(registration, classes) => classes.CreditHours
					).SumAsync();

			TotalCreditHours *= 300.23;
			
            if (user == null)
			{
				return NotFound();
			}
			else
			{
				LoginUser = user;
			}

            if (user.UserPayment == null)
            {
				user.UserPayment = 0;
				_context.Update(user);
				await _context.SaveChangesAsync();
            }
            else
            {
                TotalCreditHours = Math.Round((double)(TotalCreditHours - user.UserPayment), 2);
            }

			if(TotalCreditHours < 0)
			{
				TotalCreditHours = 0;
				user.UserPayment = 0;
				_context.Update(user);
				await _context.SaveChangesAsync();
			}

            if (paymentSuccess)
            {
                // Reload the TotalCreditHours value
                return RedirectToPage("./Fees");
            }

            return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
			int userId = int.Parse(userIdClaim);
			string? token = Request.Form["stripeToken"];
            string? paymentAmountString = Request.Form["paymentAmount"];

            var user = await _context.User.FirstOrDefaultAsync(u => u.Id == userId);

            if (!double.TryParse(paymentAmountString, out double paymentAmount))
            {
                ViewData["Error"] = "Invalid payment amount format.";
                return Page();
            }


            // Create a new Checkout Session with Stripe
            var options = new ChargeCreateOptions
			{
				Amount = (long)(paymentAmount * 100),
				Currency = "usd",
				Description = "Payment for Tuition",
				Source = token,
			};

			var service = new ChargeService();
			try
			{
				var charge = service.Create(options);
				string receipturl = charge.ReceiptUrl;
                TempData["receipt"] = receipturl;
                user.UserPayment += paymentAmount;
				
                _context.Update(user);

                await _context.SaveChangesAsync();


                return RedirectToPage("./Fees", new { paymentSuccess = true });
			}
			
			catch(Exception ex)
			{
				ViewData["Error"] = ex.Message;
				return Page();
			}
		}
	}
}
