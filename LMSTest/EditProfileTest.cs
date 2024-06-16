using System.Threading.Tasks;
using LMS.Data;
using LMS.Pages.Account;
using LMS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LMSTest
{
	[TestClass]
	public class EditProfileTest
	{

		private DbContextOptions<LMSContext> _options;

		public EditProfileTest()
		{
			// Set up the options for your regular database
			var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=LMSContext-d7d26f64-0e73-4b7a-a169-7903d2073083;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
			_options = new DbContextOptionsBuilder<LMSContext>()
				.UseSqlServer(connectionString)
				.Options;
		}

		[TestMethod]
		public async Task UpdateUserProperty_ChangesPropertyInDatabase()
		{
			User userToUpdate;

			using (var context = new LMSContext(_options))
			{
				userToUpdate = await context.User.FirstOrDefaultAsync(u => u.Id == 4);
			}

			Assert.IsNotNull(userToUpdate, "User with ID 4 was not found in the database.");

			userToUpdate.FirstName = "Jane";

			using (var context = new LMSContext(_options))
			{
				context.User.Update(userToUpdate);
				await context.SaveChangesAsync();
			}

			using (var context = new LMSContext(_options))
			{
				var updatedUser = await context.User.FirstOrDefaultAsync(u => u.Id == 4);
				Assert.AreEqual("Jane", updatedUser.FirstName); // Assert that the first name has been updated
			}

		}
	}
}