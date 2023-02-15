using CoffeeScaffolding.CoffeeScaffoldingData.IEntityDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CoffeeScaffolding.CoffeeScaffoldingData.EntityDomain
{
    /// <summary>
    /// 基础表单
    /// 含有并发控制
    /// </summary>
    public abstract class BaseWithConcurrencyTable : BaseTable,IBaseWithConcurrencyTable
    {
        public int EDIT_VERSION { get;set; }

        internal override void Save(EntityEntry Entity)
        {
            base.Save(Entity);

            if (Entity.Entity is IBaseWithConcurrencyTable)
            {
                var BaseWithConcurrencyTable = Entity.Entity as BaseWithConcurrencyTable;
                if (BaseWithConcurrencyTable != null)
                {
                    if (Entity.State == EntityState.Added)
                    {
                        BaseWithConcurrencyTable.EDIT_VERSION = 0;
                    }
                    else if (Entity.State == EntityState.Modified)
                    {
                        //自增
                        BaseWithConcurrencyTable.EDIT_VERSION += 1;
                    }
                }
            }
        }
    }
}
