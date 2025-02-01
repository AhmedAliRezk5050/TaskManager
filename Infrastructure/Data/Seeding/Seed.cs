using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data.Seeding;

public static class Seed
{

    public static async Task SeedData(AppDbContext dbContext,
        UserManager<ApplicationUser> userManager,
        ConfigurationManager configuration
        )
    {
        await dbContext.Database.MigrateAsync();

        var usersCount = userManager.Users.Count();

        if (usersCount == 0)
        {
            var user = new ApplicationUser
            {
                UserName = configuration["AdminInfo:UserName"]
            };

            var createUserResult = await userManager.CreateAsync(user, configuration["AdminInfo:Password"]);

            if (!createUserResult.Succeeded)
            {
                throw new Exception("Error creating user during seeding");
            }
        }

        await dbContext.SaveChangesAsync();
    }
}
