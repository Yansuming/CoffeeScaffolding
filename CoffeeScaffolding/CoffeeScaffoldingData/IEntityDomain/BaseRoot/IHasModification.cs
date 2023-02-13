using System;


namespace CoffeeScaffolding.CoffeeScaffoldingData.IEntityDomain.BaseRoot
{
    public interface IHasModification
    {
        /// <summary>
        /// 修改者ID
        /// </summary>
        long LAST_UPDATE_ID { get; }
        /// <summary>
        /// 修改者
        /// </summary>
        string LAST_UPDATE_NAME { get; }
        /// <summary>
        /// 修改时间
        /// </summary>
        DateTime LAST_UPDATE_TIME { get; }
    }
}
