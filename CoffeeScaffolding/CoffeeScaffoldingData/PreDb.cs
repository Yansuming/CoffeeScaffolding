using CoffeeScaffolding.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CoffeeDomain = CoffeeScaffoldingData.Models.RABC;

namespace CoffeeScaffolding.CoffeeScaffoldingData
{
    public static class PreDb
    {        
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<CoffeeScaffoldingDBContext>();
                if (service != null)
                {
                    SeedData(service, isProd);
                }

                var serviceIdentity = scope.ServiceProvider.GetService<CoffeeIdentityDbContext>();
                if(serviceIdentity != null)
                {
                    var userManagerservice = scope.ServiceProvider.GetService<UserManager<CoffeeUser>>();
                    var roleManagerservice = scope.ServiceProvider.GetService<RoleManager<CoffeeRole>>();
                    SeedIdentityData(serviceIdentity, isProd, userManagerservice, roleManagerservice);
                }
            }//自动dispose
        }

        private static void SeedData(CoffeeScaffoldingDBContext context, bool isProd)
        {
            if(isProd)
            {
                try
                {
                    Console.WriteLine("--> Run migrations...");
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not run migrations: {ex.Message}");
                }
            }

            if (!context.CoffeeUser.Any())
            {
                Console.WriteLine("--> Seeding CoffeeUser Data");
                context.AddRange(
                    new CoffeeDomain.CoffeeUser
                    {
                        Account = "admin",
                        PasswordHash = "abc",
                        UserName = "yansuming",
                        Email = "abc@abc.com",
                        IsActive = true,
                        AccessFailedCount = 0,
                        EmailConfirmed = false,
                        PhoneNumberConfirmed = false
                    },
                    new CoffeeDomain.CoffeeUser
                    {
                        Account = "test",
                        PasswordHash = "abc",
                        UserName = "test",
                        Email = "abc@abc.com",
                        IsActive = true,
                        AccessFailedCount = 0,
                        EmailConfirmed = false,
                        PhoneNumberConfirmed = false
                    }
                );
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> Already have CoffeeUser data");
            }
        }
    
        private async static void SeedIdentityData(CoffeeIdentityDbContext context, bool isProd, UserManager<CoffeeUser>? userManagerservice, RoleManager<CoffeeRole>? roleManagerservice)
        {
            if(isProd)
            {
                try
                {
                    Console.WriteLine("--> Run Identity migrations...");
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not run Identity migrations: {ex.Message}");
                }
            }

            if(userManagerservice != null && roleManagerservice != null)
            {
                if (await roleManagerservice.RoleExistsAsync("admin") == false)
                {
                    CoffeeRole role = new CoffeeRole { Name = "admin" };
                    var result = await roleManagerservice.CreateAsync(role);
                    if(!result.Succeeded)
                    {
                        Console.WriteLine("role create failed");
                    }
                }

                if(await userManagerservice.FindByNameAsync("yansuming") == null)
                {
                   CoffeeUser user = new CoffeeUser { UserName = "yansuming" };
                    var result = await userManagerservice.CreateAsync(user,"@zY1234567890");
                    if (!result.Succeeded)
                    {
                        Console.WriteLine("user create failed");
                    }
                }
            }

        }
    }
}
