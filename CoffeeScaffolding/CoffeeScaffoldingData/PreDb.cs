using CoffeeScaffolding.CoffeeScaffoldingData.Models;
using CoffeeScaffolding.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

            if (!context.SYS_USER.Any())
            {
                Console.WriteLine("--> Seeding SysUser Data");
                context.AddRange(
                    new SYS_USER() { ACCOUNT = "admin", PASSWORD = "abc", USER_NAME = "yansuming", USER_NAME_EN = "yan", USER_EMAIL = "abc@abc.com" },
                    new SYS_USER() { ACCOUNT = "test", PASSWORD = "abc", USER_NAME = "test", USER_NAME_EN = "t", USER_EMAIL = "abc@abc.com" }
                    );
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> Already have SysUser data");
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
