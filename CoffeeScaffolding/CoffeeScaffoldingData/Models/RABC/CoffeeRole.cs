using CoffeeScaffolding.CoffeeScaffoldingData.EntityDomain;

namespace CoffeeScaffoldingData.Models.RABC
{
    public class CoffeeRole: BaseWithConcurrencyTable
    {
        public long Id { get; set; }
        public string? RoleName { get; set; }
        public string? Description { get; set; }
    }
}