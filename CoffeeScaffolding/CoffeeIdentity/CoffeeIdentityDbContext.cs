using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoffeeScaffolding.Identity
{
    public class CoffeeIdentityDbContext: IdentityDbContext<CoffeeUser,CoffeeRole,long>
    {
        public CoffeeIdentityDbContext(DbContextOptions<CoffeeIdentityDbContext> options) : base(options)
        {
        }
    }
}
