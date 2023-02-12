using CoffeeScaffolding.CoffeeScaffoldingData.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeScaffolding.CoffeeScaffoldingData
{
    public class CoffeeScaffoldingDBContext : DbContext
    {
        public CoffeeScaffoldingDBContext(DbContextOptions options) : base(options)
        {

        }
        public  DbSet<SYS_USER> SYS_USER => Set<SYS_USER>();
    }
}
