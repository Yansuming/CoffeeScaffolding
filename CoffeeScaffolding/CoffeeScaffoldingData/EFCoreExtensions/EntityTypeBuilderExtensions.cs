using CoffeeScaffolding.CoffeeScaffoldingData.IEntityDomain.BaseRoot;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeeScaffolding.CoffeeScaffoldingData.EFCoreExtensions
{
    public static class EntityTypeBuilderExtensions
    {
        public static void ConfigureAllDomainColumn(this EntityTypeBuilder b)
        {
            b.ConfigureCreator();
            b.ConfigureModification();
            b.ConfigureSoftDelete();
            b.ConfigureConcurrency();         
        }

        private static void ConfigureCreator(this EntityTypeBuilder b)
        {
            if (b.Metadata.ClrType.IsAssignableTo(typeof(IHasCreator)))
            {
                b.Property(nameof(IHasCreator.CREATE_ID)).IsRequired().HasComment("创建人ID");
                b.Property(nameof(IHasCreator.CREATE_NAME)).HasMaxLength(20).IsRequired().HasComment("创建人");
                b.Property(nameof(IHasCreator.CREATE_TIME)).IsRequired().HasComment("创建时间");
            }
        }
        private static void ConfigureModification(this EntityTypeBuilder b)
        {
            if (b.Metadata.ClrType.IsAssignableTo(typeof(IHasModification)))
            {
                b.Property(nameof(IHasModification.LAST_UPDATE_ID)).IsRequired().HasComment("创建人ID");
                b.Property(nameof(IHasModification.LAST_UPDATE_NAME)).HasMaxLength(20).IsRequired().HasComment("创建人");
                b.Property(nameof(IHasModification.LAST_UPDATE_TIME)).IsRequired().HasComment("创建时间");
            }
        }
        private static void ConfigureSoftDelete(this EntityTypeBuilder b)
        {
            if (b.Metadata.ClrType.IsAssignableTo(typeof(ISoftDelete)))
            {
                b.Property(nameof(ISoftDelete.DELETED)).IsRequired().HasComment("是否删除");
                b.Property(nameof(ISoftDelete.DELETED_ID)).HasComment("删除人ID");
                b.Property(nameof(ISoftDelete.DELETED_NAME)).HasMaxLength(20).HasComment("删除人");
                b.Property(nameof(ISoftDelete.DELETED_TIME)).HasComment("删除时间");
            }
        }
        private static void ConfigureConcurrency(this EntityTypeBuilder b)
        {
            if (b.Metadata.ClrType.IsAssignableTo(typeof(IConcurrency)))
            {
                b.Property(nameof(IConcurrency.EDIT_VERSION)).IsRequired().HasComment("单证版本").IsConcurrencyToken();
            }
        }
    }
}
