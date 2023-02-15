using CoffeeScaffolding.CoffeeScaffoldingData.IEntityDomain.BaseRoot;

namespace CoffeeScaffolding.CoffeeScaffoldingData.IEntityDomain
{
    public interface IBaseTable:IHasCreator, IHasModification, ISoftDelete
    {
    }
}
