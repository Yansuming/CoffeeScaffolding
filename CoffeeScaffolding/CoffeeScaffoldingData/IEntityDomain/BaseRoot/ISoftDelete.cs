using System;

namespace CoffeeScaffolding.CoffeeScaffoldingData.IEntityDomain.BaseRoot
{
    public interface ISoftDelete
    {
        /// <summary>
        /// 是否删除
        /// </summary>
        bool DELETED { get; }
        /// <summary>
        /// 删除ID
        /// </summary>
        long? DELETED_ID { get; }
        /// <summary>
        /// 删除人
        /// </summary>
        string DELETED_NAME { get; }
        /// <summary>
        /// 删除时间
        /// </summary>
        DateTime? DELETED_TIME { get; }
    }
}
