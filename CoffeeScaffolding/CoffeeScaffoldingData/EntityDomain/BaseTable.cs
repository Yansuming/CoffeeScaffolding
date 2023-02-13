using CoffeeScaffolding.CoffeeScaffoldingData.IEntityDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CoffeeScaffolding.CoffeeScaffoldingData.EntityDomain
{
    /// <summary>
    /// 基础表单 （只含有审计节点）
    /// </summary>
    public abstract class BaseTable : IBaseTable
    {
        #region
        public long CREATE_ID { get; set; }
        public string CREATE_NAME { get; set; } = "0";
        /// <summary>
        /// 保存时统一控制，无需赋值
        /// </summary>
        public DateTime CREATE_TIME { get; protected set; }

        public long LAST_UPDATE_ID { get; set; }
        public string LAST_UPDATE_NAME { get; set; } = "0";
        /// <summary>
        /// 保存时统一控制，无需赋值
        /// </summary>
        public DateTime LAST_UPDATE_TIME { get; protected set; }

        public bool DELETED { get;  set; }
        public long? DELETED_ID { get; set; }
        public string DELETED_NAME { get; set; } = "0";
        /// <summary>
        /// 保存时统一控制，无需赋值
        /// </summary>
        public DateTime? DELETED_TIME { get; protected set; }
        #endregion
        internal virtual void Save(EntityEntry Entity)
        {
            if (Entity.Entity is IBaseTable)
            {
                var BaseTable = Entity.Entity as BaseTable;
                if (BaseTable != null)
                {
                    if (Entity.State == EntityState.Added)//新增
                    {
                        BaseTable.CREATE_TIME = DateTime.Now;
                        BaseTable.LAST_UPDATE_TIME = DateTime.Now;
                        BaseTable.DELETED = false;
                    }
                    else if (Entity.State == EntityState.Modified)//修改
                    {
                        Entity.Property(nameof(BaseTable.CREATE_ID)).IsModified = false;
                        Entity.Property(nameof(BaseTable.CREATE_NAME)).IsModified = false;
                        Entity.Property(nameof(BaseTable.CREATE_TIME)).IsModified = false;

                        if (BaseTable.DELETED) //true;进行软删除操作
                        {
                            BaseTable.DELETED_TIME = DateTime.Now;
                        }
                        else
                        {
                            Entity.Property(nameof(BaseTable.DELETED_ID)).IsModified = false;
                            Entity.Property(nameof(BaseTable.DELETED_NAME)).IsModified = false;
                            Entity.Property(nameof(BaseTable.DELETED_TIME)).IsModified = false;
                            BaseTable.LAST_UPDATE_TIME = DateTime.Now;
                        }
                    }
                }
            }
        }
    }
}
