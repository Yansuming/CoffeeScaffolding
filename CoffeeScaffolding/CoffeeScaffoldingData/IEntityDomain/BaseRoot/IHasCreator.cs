using System;

namespace CoffeeScaffolding.CoffeeScaffoldingData.IEntityDomain.BaseRoot
{
    public interface IHasCreator
    {
        /// <summary>
        /// 创建者ID
        /// </summary>
        long CREATE_ID { get; }
        /// <summary>
        /// 创建者
        /// </summary>
        string CREATE_NAME { get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CREATE_TIME { get; }
    }
}
