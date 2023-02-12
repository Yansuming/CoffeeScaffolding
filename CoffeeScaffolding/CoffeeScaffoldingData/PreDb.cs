using CoffeeScaffolding.CoffeeScaffoldingData.Models;
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
            }
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
    }
}
