using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;



namespace KTR
{
    public class SetupSecurity
    {

		public static void SeedUsers(UserManager<IdentityUser> userManager)
		{
			IdentityUser admin = userManager.FindByEmailAsync("admin@KTR.com").Result;

			if (admin == null)
			{
				IdentityUser sysadmin = new IdentityUser();
				sysadmin.Email = "admin@KTR.com";
				sysadmin.UserName = "admin@KTR.com";

				IdentityResult result = userManager.CreateAsync(sysadmin, "@Admin1").Result;

				if (result.Succeeded)
				{
					userManager.AddToRoleAsync(sysadmin, "Administrator").Wait();
				}
			}
		}


		public static void SeedRoles(RoleManager<IdentityRole> roleManager)
		{
			if (!roleManager.RoleExistsAsync("RegisteredUser").Result)
			{
				IdentityRole role = new IdentityRole();
				role.Name = "RegisteredUser";
				IdentityResult roleResult = roleManager.CreateAsync(role).Result;
			}

			if (!roleManager.RoleExistsAsync("Administrator").Result)
			{
				IdentityRole role = new IdentityRole();
				role.Name = "Administrator";
				IdentityResult roleResult = roleManager.CreateAsync(role).Result;
			}
		}


	}
}
