namespace CoffeeScaffolding.CoffeeScaffoldingData.IEntityDomain.BaseRoot
{
    public interface IConcurrency
    {
        /// <summary>
        /// 行版本 rowVersion
        /// </summary>
        int EDIT_VERSION { get; }
    }
}
