using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository._Identity
{
	public static class ApplicationIdentityContextSeed
	{
		public static async Task SeedUserAsync(UserManager<ApplicationUser> userManager )
		{
			if (!userManager.Users.Any())
			{
				var user = new ApplicationUser()
				{
					DisplayName = "Ahmed Emam",
					Email = "bondokahmed@gmail.com",
					UserName = "ahmed.emam",
					PhoneNumber = "01111111111"
				}; 

				await userManager.CreateAsync( user , "P@ssw0rd");
			}
		}
	}
}
