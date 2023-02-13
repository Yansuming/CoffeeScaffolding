using CoffeeScaffolding.CoffeeScaffoldingData.IEntityDomain.BaseRoot;

namespace CoffeeScaffolding.CoffeeScaffoldingData.IEntityDomain
{
    public interface IBaseWithConcurrencyTable : IBaseTable,IConcurrency
    {
    }
}
