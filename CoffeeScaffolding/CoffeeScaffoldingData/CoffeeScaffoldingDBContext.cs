using CoffeeScaffolding.CoffeeScaffoldingData.EFCoreExtensions;
using CoffeeScaffolding.CoffeeScaffoldingData.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


namespace CoffeeScaffolding.CoffeeScaffoldingData
{
    public class CoffeeScaffoldingDBContext : DbContext
    {
        public CoffeeScaffoldingDBContext(DbContextOptions options) : base(options)
        {

        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.AddInterceptors(new CoffeeScaffoldingDBInterceptor());//增加拦截器
        }

        /* 
         * 利用方法反射机制，给各个实体公共部分采用fluent api进行字段栏位的配置
        */
        private static readonly MethodInfo? ConfigureEntityRootMethodInfo = typeof(CoffeeScaffoldingDBContext).GetMethod(nameof(ConfigureEntityRoot), BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>
        /// 用于配置各个表的公共字段
        /// </summary>
        /// <typeparam name="TEntity">各个实体</typeparam>
        /// <param name="modelBuilder"></param>
        protected void ConfigureEntityRoot<TEntity>(ModelBuilder modelBuilder) where TEntity : class
        {
            //flunet api 配置
            modelBuilder.Entity<TEntity>().ConfigureAllDomainColumn();
        }

        /// <summary>
        /// 创建模型时
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CoffeeScaffoldingDBContext).Assembly);

            if (ConfigureEntityRootMethodInfo != null)
            {
                foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    /*将实体类型，传入配置方法
                     * entityType.ClrType ==实体类
                     * modelBuilder
                    */                    
                    ConfigureEntityRootMethodInfo.MakeGenericMethod(entityType.ClrType).Invoke(this, new object[] { modelBuilder });
                }
            }
            
        }

        public  DbSet<SYS_USER> SYS_USER => Set<SYS_USER>();
    }
}
