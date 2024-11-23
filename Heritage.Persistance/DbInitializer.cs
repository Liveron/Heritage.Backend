using Heritage.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Heritage.Persistance;

public static class DbInitializer
{
    public static void Initialize(IServiceProvider provider)
    {
        using IServiceScope scope = provider.CreateScope();
        IServiceProvider scopeServices = scope.ServiceProvider;

		try
		{
			var context = scopeServices.GetRequiredService<HeritageDbContext>();
			var userManager = scopeServices.GetRequiredService<UserManager<User>>();
			var roleManager = scopeServices.GetRequiredService <RoleManager<IdentityRole>>();
			context.Database.EnsureCreated();

			var admin = new User
			{
				UserName = "admin",
				Email = "lenkov.leni@yandex.ru",
				FirstName = "Leni",
				LastName = "Lenkov",
			};

			var user = userManager.FindByNameAsync("admin")
				.GetAwaiter().GetResult();

			if (user == null)
			{
				var result = userManager.CreateAsync(admin, "kitach9843")
					.GetAwaiter().GetResult();

                if (result.Succeeded)
                {
					var resultRole = userManager.AddToRoleAsync(admin, "Administrator")
						.GetAwaiter().GetResult();
                }
            }
		}
		catch (Exception)
		{
			throw new Exception("An error occured creating the DB.");
		}
    }
}
